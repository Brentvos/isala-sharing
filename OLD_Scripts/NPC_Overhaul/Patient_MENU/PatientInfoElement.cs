using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OLD
{
    public class PatientInfoElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI basicInfoText;
        [SerializeField] private TextMeshProUGUI treatmentStateText;
        [SerializeField] private Meter meter;

        [SerializeField] private Image progressionBackgroundImage;
        [SerializeField] private Image progressionImage;
        [SerializeField] private Image deathImage;

        [SerializeField] private Color backgroundColourOnInactive;
        [SerializeField] private Color backgroundColourOnActive;
        [SerializeField] private Color progressionColourOnActive;
        [SerializeField] private Color progressionColourOnInactive;

        [Space(10)]
        [Header("Insulin")]
        [SerializeField] private TextMeshProUGUI weightInfo_TXT;
        [SerializeField] private TextMeshProUGUI initialSugarLevels_TXT;
        // [SerializeField] private TextMeshProUGUI currentSugarLevels_TXT; // Extra

        [SerializeField] private Image weightInfoHiddenPanel_IMG;
        [SerializeField] private Image currentSugarLevelsHiddenPanel_IMG;


        private PatientData patientData;

        public void Initialize(PatientData patient)
        {
            patientData = patient;
            basicInfoText.text = patient.GetName.ToString() + "_" + patient.GetCode.ToString();
            meter.Initialize(patientData.GetTreatmentData.GetDuration, patient.GetTreatmentData.TimeSinceTreatmentStarted);

            Patient_OnStateChange(patient.GetState);
            patient.OnPatientDeath += Patient_OnPatientDeath;
            patient.OnStateChange += Patient_OnStateChange;
            patient.OnInsulinInfoUpdated += Patient_OnHiddenInfoUpdated;

            if (patientData.IsInsulinPatient)
            {
                weightInfo_TXT.text = "Weight: " + patientData.GetInsulinData.GetWeight.ToString();
                initialSugarLevels_TXT.text = "Sugar Levels: " + patientData.GetInsulinData.GetInitialSugarLevels.ToString();

                weightInfoHiddenPanel_IMG.enabled = !patientData.GetInsulinData.IsWeightHidden;
                currentSugarLevelsHiddenPanel_IMG.enabled = !patientData.GetInsulinData.IsSugarLevelsHidden;
            }
        }

        private void Patient_OnHiddenInfoUpdated(InfoType type)
        {
            switch (type)
            {
                case InfoType.Weight:
                    weightInfoHiddenPanel_IMG.enabled = patientData.GetInsulinData.IsWeightHidden;
                    break;
                case InfoType.SugarLevels:
                    currentSugarLevelsHiddenPanel_IMG.enabled = patientData.GetInsulinData.IsSugarLevelsHidden;
                    break;
            }
        }

        private void Update()
        {
            meter.UpdateMeterVisualsSimple(patientData.GetTreatmentData.TimeSinceTreatmentStarted);

            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    REVEAL_INFO_CHEAT();
            //}
        }

        private void Patient_OnPatientDeath()
        {
            treatmentStateText.text = "DISEASED";
            progressionImage.transform.parent.gameObject.SetActive(false);

            foreach (Image img in GetComponentsInChildren<Image>())
            {
                img.color = Color.red;
            }

            deathImage.gameObject.SetActive(true);
            deathImage.color = Color.white;

            this.enabled = false;
        }

        private void Patient_OnStateChange(PatientState state)
        {
            switch (state)
            {
                case PatientState.InDatabase:
                    treatmentStateText.text = "No nurse required.";
                    progressionBackgroundImage.color = backgroundColourOnInactive;
                    progressionImage.color = progressionColourOnInactive;
                    break;
                case PatientState.Default:
                    treatmentStateText.text = "No nurse required.";
                    progressionBackgroundImage.color = backgroundColourOnInactive;
                    progressionImage.color = progressionColourOnInactive;
                    break;
                case PatientState.OpenForTreatment:
                    treatmentStateText.text = "Requires nurse!";
                    progressionBackgroundImage.color = backgroundColourOnActive;
                    progressionImage.color = progressionColourOnActive;
                    break;
                case PatientState.InTreatment:
                    treatmentStateText.text = "Being treated..";
                    progressionBackgroundImage.color = backgroundColourOnActive;
                    progressionImage.color = progressionColourOnActive;
                    break;
                case PatientState.RequireAdditionalData:
                    treatmentStateText.text = "Nurse needs HELP!";
                    progressionBackgroundImage.color = backgroundColourOnActive;
                    progressionImage.color = progressionColourOnInactive;
                    break;
                case PatientState.InTreatmentPhaseTwo:
                    treatmentStateText.text = "Being treated..";
                    progressionBackgroundImage.color = backgroundColourOnActive;
                    progressionImage.color = progressionColourOnActive;
                    break;
                case PatientState.RecentlyTreated:
                    treatmentStateText.text = "No nurse required.";
                    progressionBackgroundImage.color = backgroundColourOnInactive;
                    progressionImage.color = progressionColourOnInactive;
                    break;
            }
        }


        /// <summary>
        /// Testing purposes!
        /// </summary>
        private void REVEAL_INFO_CHEAT()
        {
            if (patientData.IsInsulinPatient)
            {
                patientData.UpdateInfo(true, InfoType.SugarLevels);
                patientData.UpdateInfo(true, InfoType.Weight);
            }
        }
    }
}