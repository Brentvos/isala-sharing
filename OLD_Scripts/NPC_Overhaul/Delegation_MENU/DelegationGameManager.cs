using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLD
{


    public class DelegationGameManager : MonoBehaviour
    {
        [SerializeField] private bool fillInteractivePatientElements;

        [SerializeField] private GameObject patientInteractiveElement;

        // !!!!!!!!!!!!! The LENGTH should ALWAYS be EQUAL to AMOUNT OF PATIENTS GENERATED !!!!!!!!!!!!
        // !!!!!!!!!!!!! NpcDatabase.amountOfPatientsToGenerate !!!!!!!!!!
        [SerializeField] private List<Transform> patientSpawnPoints = new List<Transform>();

        [SerializeField] private Color patientUnavailableColor;
        [SerializeField] private Color patientAvailableColor;
        [SerializeField] private Color patientInTreatmentColor;
        [SerializeField] private Color patientRequireDataColor;

        public Color GetPatientUnavailableColor { get { return patientUnavailableColor; } }
        public Color GetPatientAvailableColor { get { return patientAvailableColor; } }
        public Color GetPatientInTreatmentColor { get { return patientInTreatmentColor; } }
        public Color GetPatientRequireDataColor { get { return patientRequireDataColor; } }

        public static DelegationGameManager Instance;

        private void Awake()
        {
            if (Instance != this)
                Instance = this;
        }

        private void Start()
        {
            NpcDatabase.Instance.OnPatientsGenerated += OnPatientsGenerated;
        }

        private void OnPatientsGenerated(List<PatientData> patients)
        {
            if (!fillInteractivePatientElements)
                return;

            for (int i = 0; i < patients.Count; i++)
            {
                GameObject patientElement = Instantiate(patientInteractiveElement, patientSpawnPoints[i], false);
                patientElement.GetComponent<PatientInteractiveElement>().Initialize(patients[i]);
            }
        }

        // Cheat code for now
        public void CHEAT_UpdateData()
        {
            foreach (PatientData patient in NpcDatabase.Instance.GetAllPatients)
            {
                if (patient.GetState == PatientState.RequireAdditionalData)
                {
                    patient.ChangeState(PatientState.InTreatmentPhaseTwo);
                }
            }
        }
    }
}