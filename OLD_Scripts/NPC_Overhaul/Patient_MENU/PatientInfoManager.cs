using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLD
{
    public class PatientInfoManager : MonoBehaviour
    {
        [SerializeField] private GameObject patientInfoElement;
        [SerializeField] private GameObject insulinPatientInfoElement;
        [SerializeField] private Transform patientNotebook;


        private void Start()
        {
            NpcDatabase.Instance.OnPatientsGenerated += OnPatientsGenerated;
        }

        private void OnPatientsGenerated(List<PatientData> patients)
        {
            for (int i = 0; i < patients.Count; i++)
            {
                GameObject patientElement = null;

                if (patients[i].IsInsulinPatient)
                    patientElement = Instantiate(insulinPatientInfoElement, patientNotebook, false);

                else
                    patientElement = Instantiate(patientInfoElement, patientNotebook, false);

                patientElement.GetComponent<PatientInfoElement>().Initialize(patients[i]);
            }
        }
    }
}