using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace New
{
    public class PatientInfoDisplayElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI code_TXT;  
        [SerializeField] private TextMeshProUGUI name_TXT;
        [SerializeField] private Button detailedInfo_BTN;

        private PatientRoot patient;

        [SerializeField] private Image closeupAvatar;

        public void Initialize(PatientRoot patient)
        {
            this.patient = patient;

            code_TXT.text = patient.GetPatientInfo.GetCode.ToString();
            name_TXT.text = patient.GetPatientInfo.GetNameWithTitle();
            closeupAvatar.sprite = patient.GetPatientInfo.GetCloseUpAvatar;

            detailedInfo_BTN.onClick.AddListener(() => 
            {
                PatientInfoManager.Instance.ProcessPatient(patient);
            });
        }
    }
}