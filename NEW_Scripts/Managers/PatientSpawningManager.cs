using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace New
{
    public class PatientSpawningManager : MonoBehaviour
    {
        // Add IsFull event?
        // Add percentage chance for insulin patient spawn, currently it gets instantly replaced

        [Header("Prefabs")]
        [SerializeField] private GameObject patientComponent_PREFAB;
        [SerializeField] private GameObject patientInfuusComponent_PREFAB;
        [SerializeField] protected GameObject patientTaskElement_PREFAB;

        [Header("Components")]
        [SerializeField] private GameObject patientComponent_parent_OBJ;
        [SerializeField] private GameObject patientInfo_parent_OBJ;
        [SerializeField] private GameObject patientTask_parent_OBJ;

        private List<PatientRoot> existingPatients = new List<PatientRoot>();

        public GameObject GetPatientInfoParent { get { return patientInfo_parent_OBJ; } }
        public GameObject GetPatientTaskParent { get { return patientTask_parent_OBJ; } }
        public GameObject GetPatientTaskElement_PREFAB { get { return patientTaskElement_PREFAB; } }

        public event Action<PatientRoot> OnPatientDataComponentCreated;
        public event Action<PatientRoot> OnFieldPatientSpawned;
        public event Action<PatientRoot> OnPatientRemoved; // Add ENUM on how they were removed, still requires implementation

        public static PatientSpawningManager Instance;

        private float timeSinceLastPatientSpawned;
        private float intervalToSpawnNextPatient = 999;

        private void Awake()
        {
            if (Instance != this)
                Instance = this;
        }

        private void Start()
        {
            intervalToSpawnNextPatient = Utility_Functions.GetNumberDeviated(
                BalancingManager.Instance.GetData.GET_TIME_BETWEEN_PATIENTS_SPAWN_FIRST,
                BalancingManager.Instance.GetData.GET_TIME_BETWEEN_PATIENTS_SPAWN_FIRST_DEVIATION
                , false, true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreatePatientDataComponent();
            }

            if (existingPatients.Count <= BalancingManager.Instance.GetData.GET_PATIENT_TYPES_TO_EXIST.Count)
            {
                timeSinceLastPatientSpawned += Time.deltaTime;
            }

            if (timeSinceLastPatientSpawned >= intervalToSpawnNextPatient)
            {
                CreatePatientDataComponent();

                intervalToSpawnNextPatient = Utility_Functions.GetNumberDeviated(
                        BalancingManager.Instance.GetData.GET_TIME_BETWEEN_PATIENTS_SPAWN_BASE,
                        BalancingManager.Instance.GetData.GET_TIME_BETWEEN_PATIENTS_SPAWN_DEVIATION);

                timeSinceLastPatientSpawned = 0;
            }
        }

        private void CreatePatientDataComponent()
        {
            int amountOfInfuusPatients = existingPatients.FindAll(p => p.GetPatientInfo.GetPatientType == PATIENT_TYPE.INFUUS).Count;
            int amountOfDefaultPatients = existingPatients.FindAll(p => p.GetPatientInfo.GetPatientType == PATIENT_TYPE.DEFAULT).Count;
            int amountOfSymptomPatients = existingPatients.FindAll(p => p.GetPatientInfo.GetPatientType == PATIENT_TYPE.SYMPTOM).Count;

            List<PATIENT_TYPE> eligibleSpawnTypes = new List<PATIENT_TYPE>();

            if (amountOfInfuusPatients < BalancingManager.Instance.GetData.GET_PATIENT_TYPES_TO_EXIST
                .FindAll(type => type == PATIENT_TYPE.INFUUS).Count)
            {
                eligibleSpawnTypes.Add(PATIENT_TYPE.INFUUS);
            }

            if (amountOfSymptomPatients < BalancingManager.Instance.GetData.GET_PATIENT_TYPES_TO_EXIST
                .FindAll(type => type == PATIENT_TYPE.SYMPTOM).Count)
            {
                eligibleSpawnTypes.Add(PATIENT_TYPE.SYMPTOM);
            }

            if (amountOfDefaultPatients < BalancingManager.Instance.GetData.GET_PATIENT_TYPES_TO_EXIST
                .FindAll(type => type == PATIENT_TYPE.DEFAULT).Count)
            {
                eligibleSpawnTypes.Add(PATIENT_TYPE.DEFAULT);
            }

            if (eligibleSpawnTypes.Count == 0) // Should never happen
                return;

            PATIENT_TYPE typeToSpawn = eligibleSpawnTypes[UnityEngine.Random.Range(0, eligibleSpawnTypes.Count)];
            CreatePatientDataComponent(typeToSpawn);
        }

        private void CreatePatientDataComponent(PATIENT_TYPE type) // Manual version of CreatePatientDataComponent, CAN override max atm
        {
            PatientRoot patient = null;

            if (type == PATIENT_TYPE.INFUUS)
                patient = Instantiate(patientInfuusComponent_PREFAB, patientComponent_parent_OBJ.transform)
                    .GetComponent<PatientRoot>();

            else
                patient = Instantiate(patientComponent_PREFAB, patientComponent_parent_OBJ.transform)
                    .GetComponent<PatientRoot>();

            patient.Initialize(Utility_Functions.CreateBasicPatientInfo(GetExistingNames(), GetExistingCodes(), type));
            existingPatients.Add(patient);
            OnPatientDataComponentCreated?.Invoke(patient);
        }

        public void SpawnPatientOnField(PatientRoot patient, Transform parent, bool flipped)
        {
            patient.SetupPatientInfoElement();
            patient.SetupTaskInfoElement();
            patient.SetupPatientBedElement(parent, flipped);
            OnFieldPatientSpawned?.Invoke(patient);
            // Actually put the patient on the field
        }

        public void RemovePatient(PatientRoot patient, bool isDead)
        {
            if (isDead)
            {
                ScoreManager.instance.UpdateScore(SCORE_TYPES.PATIENT_LOST_ON_HEALTH, patient);

                if (patient.GetTaskElement == null)
                {
                    BeeperManager.Instance.PatientRemovedFromGame(patient);
                    existingPatients.Remove(patient);
                    Destroy(patient.gameObject);
                    return;
                }

                NotebookTaskManager.Instance.Remove(patient.GetTaskElement);
                BeeperManager.Instance.PatientRemovedFromGame(patient);
            }

            OnPatientRemoved?.Invoke(patient); // PatientInfoManager, add TaskManager later
            existingPatients.Remove(patient); 
            patient.Destroy(); // Remove patient + elements
        }

        private int[] GetExistingCodes()
        {
            return existingPatients
                .Select(patient => patient.GetPatientInfo.GetCode)
                .ToArray();
        }

        private LAST_NAME[] GetExistingNames()
        {
            return existingPatients
                .Select(patient => patient.GetPatientInfo.GetLastName)
                .ToArray();
        }
    }
}