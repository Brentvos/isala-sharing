using System;
using System.Collections;
using UnityEngine;

namespace New
{
    /// <summary>
    /// This is the root patient script, it cannot be put on an object
    /// </summary>
    public abstract class PatientRoot : MonoBehaviour
    {
        protected PatientInfo patientInfo;
        protected ClinicalStatusInfo clinicalStatus;
        protected CaseSO caseSO;

        // Treatment
        [SerializeField] private Treatment[] treatmentFlow;
        private Treatment currentTreatmentBlock;
        private int currentTreatmentIndex;
        private float timeOnWaitingTreatment = 0;

        [Header("Element Prefabs")]
        [SerializeField] protected GameObject patientInBedElement_PREFAB;
        [SerializeField] protected GameObject patientInfoElement_PREFAB;

        protected PatientBedDisplayElement patientBedElement;
        protected PatientInfoDisplayElement patientInfoElement;
        protected PatientTaskElement patientTaskElement;


        public Treatment[] GetTreatmentFlow { get { return treatmentFlow; } }
        public Treatment GetCurrentTreatmentBlock { get { return currentTreatmentBlock; } }
        public PatientInfo GetPatientInfo { get { return patientInfo; } }
        public ClinicalStatusInfo GetClinicalStatus { get { return clinicalStatus; } }
        public CaseSO GetCaseDataSO { get { return caseSO; } }
        public PatientInfoDisplayElement GetPatientInfoElement { get { return patientInfoElement; } }
        public PatientTaskElement GetTaskElement { get { return patientTaskElement; } }

        public event Action<float> OnTaskTimeChanged;

        public virtual void Initialize(PatientInfo basicInfo)
        {
            this.patientInfo = basicInfo;
            clinicalStatus = new ClinicalStatusInfo();
            clinicalStatus.SetBalancingStats();

            currentTreatmentBlock = treatmentFlow[0];
            caseSO = DialogueManager.Instance.GetCase(basicInfo.GetPatientType);

            if (caseSO.HasSymptoms)
                patientInfo.SetSymptoms(caseSO.GetSymptoms);

            if (caseSO.HasDiagnose)
                patientInfo.SetDiagnose(caseSO.GetDiagnose);


            GetClinicalStatus.OnChangeValue += CheckIfDead;
        }

        private void CheckIfDead(float value)
        {
            if (value <= 0)
                PatientSpawningManager.Instance.RemovePatient(this, true);
        }

        private void Update()
        {
            if (clinicalStatus != null)
            {
                if (BeeperManager.Instance.GetCurrentPatient == this && BeeperManager.Instance.IsPatientOnCall)
                    return;

                if (GetClinicalStatus.IsDegressionOverridden)
                    GetClinicalStatus.ChangeValue(GetClinicalStatus.GetRoomProgressionBuff * Time.deltaTime);

                else
                    GetClinicalStatus.ChangeValue(-clinicalStatus.GetTotalDegression * Time.deltaTime);

                //if (Input.GetKeyDown(KeyCode.Alpha3))
                //{
                //    GetClinicalStatus.ChangeValue(-100);
                //}
            }
        }

        public void Destroy()
        {
            RoomRoot[] patientRooms = Utility_Functions.GetAllRoomsOfType(ROOM_TYPE.PATIENT); // Out of room

            foreach (PatientRoom room in patientRooms)
            {
                if (room.IsPatientInRoom(this))
                {
                    room.ReleasePatient(this);
                    break;
                }
            }

            Destroy(patientBedElement.gameObject);
            Destroy(patientInfoElement.gameObject);
            Destroy(gameObject);
        }
        public void ProceedTreatment(bool isDelayed)
        {
            if (isDelayed)
            {
                if (patientTaskElement != null)
                {
                    patientTaskElement.UpdateState(this);
                    patientBedElement.LightActivation(true);
                }

                return;
            }

            currentTreatmentIndex++;
            currentTreatmentBlock = treatmentFlow[currentTreatmentIndex];

            if (currentTreatmentIndex < treatmentFlow.Length - 1)
            {
                if (currentTreatmentBlock.GetTreatmentType == TREATMENT_TYPE.WAITING)
                {
                    if (patientTaskElement != null)
                    {
                        patientTaskElement.UpdateState(this);
                        patientBedElement.LightActivation(false);
                    }

                    StartCoroutine(WaitForTreatment());
                }

                else
                {
                    BeeperManager.Instance.OnAddTaskToBeeper(this);
                    patientBedElement.LightActivation(true);
                    patientTaskElement.UpdateState(this, true);
                }
            }

            else
            {
                patientTaskElement.UpdateState(this);
                patientBedElement.LightActivation(false);
                PatientSpawningManager.Instance.RemovePatient(this, false);
                return;
            }
        }

        private IEnumerator WaitForTreatment()
        {
            while(timeOnWaitingTreatment < BalancingManager.Instance.GetData.GET_TIME_TO_WAIT_FOR_TASK_BASE) // currentTreatmentBlock.GetTimeToWait
            {
                yield return new WaitForSeconds(0.02f); // Interval
                timeOnWaitingTreatment += 0.02f;
                OnTaskTimeChanged?.Invoke(timeOnWaitingTreatment);
            }

            timeOnWaitingTreatment = 0;
            ProceedTreatment(false);
        }

        #region Element setup
        public virtual void SetupPatientInfoElement()
        {
            PatientInfoDisplayElement element = Instantiate(patientInfoElement_PREFAB,
                PatientSpawningManager.Instance.GetPatientInfoParent.transform)
                .GetComponent<PatientInfoDisplayElement>();

            element.Initialize(this);
            patientInfoElement = element;
        }

        public virtual void SetupTaskInfoElement()
        {
            GameObject patientTaskInfoObj = Instantiate(PatientSpawningManager.Instance.GetPatientTaskElement_PREFAB,
                PatientSpawningManager.Instance.GetPatientTaskParent.transform, false);

            patientTaskElement = patientTaskInfoObj.GetComponent<PatientTaskElement>();
            patientTaskElement.Initialize(this);
        }

        public void SetupPatientBedElement(Transform parent, bool flipped)
        {
            GameObject patientInBed = Instantiate(patientInBedElement_PREFAB, parent, false);
            patientBedElement = patientInBed.GetComponent<PatientBedDisplayElement>();
            patientBedElement.Initialize(patientInfo.GetNameWithTitle(), clinicalStatus, flipped);
        }
        #endregion
    }
}