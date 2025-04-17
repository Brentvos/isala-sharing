using New;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace New
{

    public class PatientTaskElement : MonoBehaviour
    {
        [Header("Static")]
        [SerializeField] private Color pendingColour;
        [SerializeField] private Sprite pendingSprite;
        [Space(5)]
        [SerializeField] private Color toDoColour;
        [SerializeField] private Sprite toDoSprite;
        [Space(5)]
        [SerializeField] private Color doneColour;
        [SerializeField] private Sprite doneSprite;

        [SerializeField] private Button taskStart_BTN;

        [Header("Dynamic")]
        [SerializeField] private TextMeshProUGUI name_TXT;
        [SerializeField] private Image taskTypeImage_IMG;
        [SerializeField] private Image background_IMG;
       // [SerializeField] private Meter progressionMeter;
        [SerializeField] private Image patientAvatar_IMG;

        private TREATMENT_STATE state;
        public TREATMENT_STATE GetTreatmentState { get { return state; } }
        // [SerializeField] private Image avatar;

        public void Initialize(PatientRoot patient)
        {
            name_TXT.text = patient.GetPatientInfo.GetNameWithTitle();
            patientAvatar_IMG.sprite = patient.GetPatientInfo.GetCloseUpAvatar;
            UpdateState(patient);
           // progressionMeter.Initialize(patient);
            taskStart_BTN.interactable = false;

            NotebookTaskManager.Instance.Add(this);
        }

        public void UpdateState(PatientRoot patient, bool setToDo = false)
        {
            taskStart_BTN.onClick.RemoveAllListeners();

            if (setToDo)
            {
               // taskType_TXT.text = "ON BEEPER";
                return;
            }

            if (patient.GetCurrentTreatmentBlock.GetTreatmentType == TREATMENT_TYPE.WAITING)
            {
              //  taskType_TXT.text = "PENDING";
                taskTypeImage_IMG.sprite = pendingSprite;
                background_IMG.color = pendingColour;
                taskStart_BTN.interactable = false;

                state = TREATMENT_STATE.PENDING;
            }

            else if (patient.GetCurrentTreatmentBlock.GetTreatmentType != TREATMENT_TYPE.WAITING
                && patient.GetCurrentTreatmentBlock.GetTreatmentType != TREATMENT_TYPE.DONE)
            {
            //    taskType_TXT.text = "TO DO";
                taskTypeImage_IMG.sprite = toDoSprite;
                background_IMG.color = toDoColour;

                taskStart_BTN.interactable = true;
                taskStart_BTN.onClick.AddListener(() => BeeperManager.Instance.OnClickPatientTaskInNotebook(patient, taskStart_BTN));

                state = TREATMENT_STATE.TO_DO;
            }
            
            else if (patient.GetCurrentTreatmentBlock.GetTreatmentType == TREATMENT_TYPE.DONE)
            {
               // taskType_TXT.text = "DONE";
                taskTypeImage_IMG.sprite = doneSprite;
                background_IMG.color = doneColour;
                taskStart_BTN.interactable = false;

                state = TREATMENT_STATE.DONE;
            }

          //  progressionMeter.ResetBar();
            NotebookTaskManager.Instance.Sort();
        }
    }
}