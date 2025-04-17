using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLD
{


    public class DataRequestManager : MonoBehaviour
    {
        [SerializeField] private GameObject dataRequestElement;
        [SerializeField] private GameObject dataRequestParent;

        private void Start()
        {
            NpcDatabase.Instance.OnPatientsGenerated += SetupPatientStateListeners;
        }

        private void SetupPatientStateListeners(List<PatientData> patients)
        {
            foreach (PatientData patient in patients)
            {
                patient.OnStateChangeDetailed += OnPatientStateChange;
            }
        }

        private void OnPatientStateChange(PatientData patient)
        {
            if (patient.GetState != PatientState.RequireAdditionalData)
                return;

            GameObject dataElement = Instantiate(dataRequestElement, dataRequestParent.transform, false);
            dataElement.GetComponent<DataRequestElement>().Initialize(patient);
        }
    }
}