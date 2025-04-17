using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OLD
{


    public class DataRequestElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI patientCodeText, dataFailText, dataSuccessNameText, dataSuccessCodeText;
        [SerializeField] private TMP_Dropdown nameDropdown;
        [SerializeField] private Button confirmationButton;

        [SerializeField] private Image dataFailImage, dataSuccessImage, dataPendingImage;

        [SerializeField] private float dataFailInterval, dataSuccessInterval;
        private float timeSinceDataFail, timeSinceDataSuccess;
        private bool runDataFailTimer, runDataSuccessTimer;

        private PatientData patientData;

        public void Initialize(PatientData patient)
        {
            patientData = patient;
            patientCodeText.text = "[" + patientData.GetCode + "]";
            confirmationButton.onClick.AddListener(() => OnDataConfirmation());

            PopulateDropdown();
            StartDataRequest();
        }

        private void Update()
        {
            if (runDataFailTimer)
            {
                if (timeSinceDataFail < dataFailInterval)
                {
                    timeSinceDataFail += Time.deltaTime;
                    dataFailText.text = "Wait [" + (dataFailInterval - timeSinceDataFail).ToString("F1") + "s..]";
                }

                else
                    StartDataRequest();
            }

            if (runDataSuccessTimer)
            {
                if (timeSinceDataSuccess < dataSuccessInterval)
                    timeSinceDataSuccess += Time.deltaTime;

                else
                    DataPanelCleanup();
            }
        }

        private void StartDataRequest()
        {
            runDataFailTimer = false;
            timeSinceDataFail = 0;

            dataPendingImage.gameObject.SetActive(true);
            dataSuccessImage.gameObject.SetActive(false);
            dataFailImage.gameObject.SetActive(false);

            confirmationButton.interactable = true;
            // nameDropdown.interactable = true; // Maybe also disable this on fail?
        }

        private void StartDataSuccessProcess()
        {
            patientData.ChangeState(PatientState.InTreatmentPhaseTwo);

            dataSuccessNameText.text = patientData.GetName.ToString(); // Can also be done on start, as answer is already clear by then
            dataSuccessCodeText.text = patientData.GetCode.ToString();

            dataSuccessImage.gameObject.SetActive(true);
            dataPendingImage.gameObject.SetActive(false);
            dataFailImage.gameObject.SetActive(false);

            timeSinceDataSuccess = 0;
            runDataSuccessTimer = true;

            confirmationButton.gameObject.SetActive(false);
        }

        private void StartDataFailureProcess()
        {
            dataFailImage.gameObject.SetActive(true);
            dataPendingImage.gameObject.SetActive(false);
            dataSuccessImage.gameObject.SetActive(false);

            timeSinceDataFail = 0;
            runDataFailTimer = true;

            confirmationButton.interactable = false;
            patientData.DataIncorrectlyGiven();
            // nameDropdown.interactable = false; // Maybe also disable this?
        }

        private void DataPanelCleanup()
        {
            Destroy(gameObject);
        }

        private void PopulateDropdown()
        {
            nameDropdown.options.Clear(); // Clear existing options

            First_Names[] allNames = Enum.GetValues(typeof(First_Names)).Cast<First_Names>().ToArray();
            var sortedNames = allNames.OrderBy(l => l.ToString());

            foreach (var name in sortedNames)
            {
                nameDropdown.options.Add(new TMP_Dropdown.OptionData(name.ToString()));
            }
        }

        private void OnDataConfirmation()
        {
            if (nameDropdown.options[nameDropdown.value].text == patientData.GetName.ToString())
            {
                StartDataSuccessProcess();
                return;
            }

            StartDataFailureProcess();
        }
    }
}