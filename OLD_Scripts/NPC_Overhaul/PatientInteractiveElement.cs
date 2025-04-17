using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OLD
{
    public class PatientInteractiveElement : MonoBehaviour
    {
        [SerializeField] private PatientData attachedPatient; // Temporary for tracking
        public PatientData GetPatientData { get { return attachedPatient; } }

        [SerializeField] private TextMeshProUGUI codeText;
        [SerializeField] private Meter healthMeter;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private GameObject healthParent;

        [SerializeField] private Image patientDeathImage;
        private bool isInactive = false;

        public void Initialize(PatientData patient)
        {
            attachedPatient = patient;
            codeText.text = attachedPatient.GetCode.ToString();
            attachedPatient.OnStateChange += OnStateChange;

            OnStateChange(attachedPatient.GetState);
            healthMeter.Initialize(patient.GetHealthData.GetMaxHealth, patient.GetHealthData.CurrentHealth);

            attachedPatient.OnPatientDeath += AttachedPatient_OnPatientDeath;
            Invoke("ActivatePatient", attachedPatient.GetTreatmentData.GetStartInterval);
        }

        private void Start()
        {
            StartCoroutine(healthMeter.UpdateProgressionPrediction());
        }

        private void AttachedPatient_OnPatientDeath()
        {
            healthText.text = "DEAD";
            patientDeathImage.gameObject.SetActive(true);
            isInactive = true;

            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Button>().interactable = false;
            healthParent.SetActive(false);
            this.enabled = false;
        }

        private void HealthManaging()
        {
            if (attachedPatient.GetHealthData.GetHealthManageState != HealthManageState.Dead)
            {
                healthText.text = Mathf.FloorToInt(attachedPatient.GetHealthData.CurrentHealth).ToString();

                if (attachedPatient.GetHealthData.GetHealthManageState != HealthManageState.None)
                {
                    attachedPatient.GetHealthData.ChangeValue(attachedPatient.GetHealthData.GetChangeRate * Time.deltaTime, attachedPatient, false);
                    healthMeter.UpdateMeterVisuals(attachedPatient.GetHealthData.CurrentHealth, false);
                }
            }
        }

        private void OnStateChange(PatientState state)
        {
            Image img = GetComponent<Image>();

            switch (state)
            {
                case PatientState.InDatabase:
                    img.color = DelegationGameManager.Instance.GetPatientUnavailableColor;
                    GetPatientData.GetHealthData.ChangeHealthState(HealthManageState.HealthyGain);
                    break;
                case PatientState.Default:
                    img.color = DelegationGameManager.Instance.GetPatientUnavailableColor;
                    GetPatientData.GetHealthData.ChangeHealthState(HealthManageState.HealthyGain);
                    break;
                case PatientState.OpenForTreatment:
                    img.color = DelegationGameManager.Instance.GetPatientAvailableColor;
                    GetPatientData.GetHealthData.ChangeHealthState(HealthManageState.WaitingForTreatmentLoss);
                    break;
                case PatientState.InTreatment:
                    img.color = DelegationGameManager.Instance.GetPatientInTreatmentColor;
                    GetPatientData.GetHealthData.ChangeHealthState(HealthManageState.TreatmentGain);
                    break;
                case PatientState.RequireAdditionalData:
                    img.color = DelegationGameManager.Instance.GetPatientRequireDataColor;
                    GetPatientData.GetHealthData.ChangeHealthState(HealthManageState.WaitingForDataLoss);
                    break;
                case PatientState.InTreatmentPhaseTwo:
                    img.color = DelegationGameManager.Instance.GetPatientInTreatmentColor;
                    GetPatientData.GetHealthData.ChangeHealthState(HealthManageState.TreatmentGain);
                    break;
                case PatientState.RecentlyTreated:
                    img.color = DelegationGameManager.Instance.GetPatientUnavailableColor;
                    GetPatientData.GetHealthData.ChangeValue(GetPatientData.GetHealthData.GetHealthOnTaskComplete, attachedPatient);
                    GetPatientData.GetHealthData.ChangeHealthState(HealthManageState.HealthyGain);
                    break;
            }
        }

        private void ActivatePatient()
        {
            attachedPatient.ChangeState(PatientState.OpenForTreatment);
        }

        public void StartTreatment(int nurseCode)
        {
            attachedPatient.GetTreatmentData.TimeSinceTreatmentStarted = 0;
            attachedPatient.GetTreatmentData.TimeSinceTreatmentCompleted = 0;

            foreach (NurseInfoElement activeNurseElement in FindObjectsOfType<NurseInfoElement>())
            {
                if (activeNurseElement.GetNurseData.GetCode == nurseCode)
                {
                    attachedPatient.CurrentAttachedNurseElementReference = activeNurseElement;
                }
            }

            attachedPatient.ChangeState(PatientState.InTreatment);
        }

        private void Update()
        {
            if (isInactive)
                return;

            HealthManaging();

            switch (attachedPatient.GetState)
            {
                case PatientState.InTreatment:
                    if (attachedPatient.GetTreatmentData.TimeSinceTreatmentStarted < attachedPatient.GetTreatmentData.GetDataRequestPoint)
                        attachedPatient.GetTreatmentData.TimeSinceTreatmentStarted += Time.deltaTime;

                    else
                    {
                        attachedPatient.ChangeState(PatientState.RequireAdditionalData);
                    }

                    break;


                case PatientState.RequireAdditionalData:
                    break;


                case PatientState.InTreatmentPhaseTwo:
                    if (attachedPatient.GetTreatmentData.TimeSinceTreatmentStarted < attachedPatient.GetTreatmentData.GetDuration)
                        attachedPatient.GetTreatmentData.TimeSinceTreatmentStarted += Time.deltaTime;

                    else
                    {
                        attachedPatient.ChangeState(PatientState.RecentlyTreated);
                    }
                    break;

                case PatientState.RecentlyTreated:
                    if (attachedPatient.GetTreatmentData.TimeSinceTreatmentCompleted < attachedPatient.GetTreatmentData.GetInterval)
                    {
                        attachedPatient.GetTreatmentData.TimeSinceTreatmentStarted = 0;
                        attachedPatient.GetTreatmentData.TimeSinceTreatmentCompleted += Time.deltaTime;
                    }

                    else
                    {
                        attachedPatient.GetTreatmentData.TimeSinceTreatmentCompleted = 0;
                        attachedPatient.GenerateNewTreatmentData();
                        attachedPatient.ChangeState(PatientState.OpenForTreatment);
                    }

                    break;
            }
        }
    }
}