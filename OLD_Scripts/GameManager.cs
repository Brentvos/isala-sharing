using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace OLD
{


    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI patientsHelpedText;
        [SerializeField] private TextMeshProUGUI dataIncorrectlyGivenText;
        // [SerializeField] private TextMeshProUGUI patientsDiedText;
        [SerializeField] private TextMeshProUGUI nursesNeglectedText;

        private int patientsHelpedCounter;
        private int patientsDiedCounter;
        private int dataIncorrectlyGivenCounter;
        private int nursesNeglectedCounter;

        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private float totalTimer = 600;
        private float currentGlobalTimer;


        private void Start()
        {
            NpcDatabase.Instance.OnNursesGenerated += Instance_OnNursesGenerated;
            NpcDatabase.Instance.OnPatientsGenerated += Instance_OnPatientsGenerated;

            patientsHelpedText.text = "Patients helped: 0";
            // patientsDiedText.text = "Patients died: 0";
            nursesNeglectedText.text = "Nurses neglected: 0";
            dataIncorrectlyGivenText.text = "Data incorrectly given: 0";

            currentGlobalTimer = totalTimer;
        }
        private void Update()
        {
            currentGlobalTimer = Mathf.Clamp(currentGlobalTimer - Time.deltaTime, 0, totalTimer);
            ConvertTimeToStopwatchFormat(timerText, currentGlobalTimer);
        }

        private void Instance_OnPatientsGenerated(List<PatientData> patients)
        {
            foreach (PatientData patient in patients)
            {
                patient.OnPatientHelped += Patient_OnPatientHelped;
                patient.OnDataIncorrectlyGiven += Patient_OnDataIncorrectlyGiven;
                patient.OnPatientDeath += Patient_OnPatientDeath;
            }
        }

        private void Instance_OnNursesGenerated(List<NurseData> nurses)
        {
            foreach (NurseData nurse in nurses)
            {
                nurse.OnMoraleEmpty += Nurse_OnMoraleEmpty;
            }
        }
        private void Patient_OnPatientDeath()
        {
            patientsDiedCounter++;
            //  patientsDiedText.text = "Patients died: " + patientsDiedCounter.ToString();
        }

        private void Nurse_OnMoraleEmpty()
        {
            nursesNeglectedCounter++;
            nursesNeglectedText.text = "Nurses neglected: " + nursesNeglectedCounter;
        }

        private void Patient_OnDataIncorrectlyGiven()
        {
            dataIncorrectlyGivenCounter++;
            dataIncorrectlyGivenText.text = "Data incorrectly given: "
                + dataIncorrectlyGivenCounter;
        }

        private void Patient_OnPatientHelped()
        {
            patientsHelpedCounter++;
            patientsHelpedText.text = "Patients helped: " + patientsHelpedCounter;
        }

        private void ConvertTimeToStopwatchFormat(TextMeshProUGUI textToDisplay, float timer)
        {
            int minutes = Mathf.FloorToInt((timer % 3600) / 60F);
            int seconds = Mathf.FloorToInt(timer % 60);

            textToDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}