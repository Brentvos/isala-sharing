using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace New
{
    public class TreatmentPopupRoot : MonoBehaviour // Abstractify and add others? (maybe unnecessary)
    {
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Transform choicesParent;
        [SerializeField] private GameObject choice_PREFAB;
        [SerializeField] private Image nurse_img;

        private void Awake()
        {
        }

        public void Destroy()
        {
            foreach(Image img in transform.GetComponentsInChildren<Image>())
            {
                img.DOFade(0, 0.25f);
            }

            foreach (TextMeshProUGUI txt in transform.GetComponentsInChildren<TextMeshProUGUI>())
            {
                txt.DOFade(0, 0.25f);
            }

            transform.DOScale(0.1f, 0.25f).OnComplete(() => Destroy(gameObject));
        }

        public void Initialize(PatientRoot patient, bool isFromNotebook, bool isCurrentlyWaiting)
        {
            nurse_img.sprite = DialogueManager.Instance.GetRandomNurseSprite();

            switch (patient.GetCurrentTreatmentBlock.GetTreatmentType)
            {
                case TREATMENT_TYPE.INTAKE:
                    CreateIntakePopup(patient, isFromNotebook, isCurrentlyWaiting);
                    break;
                case TREATMENT_TYPE.CODE:
                    CreateCodePopup(patient, isFromNotebook, isCurrentlyWaiting);
                    break;
                case TREATMENT_TYPE.CASUS_GENERIC:
                    CreateCasusPopup(patient, isFromNotebook, isCurrentlyWaiting);
                    break;
                case TREATMENT_TYPE.INFUUS_LIQUID:
                    CreateCasusPopup(patient, isFromNotebook, isCurrentlyWaiting);
                    break;
                case TREATMENT_TYPE.INFUUS_WEIGHT:
                    CreateInfuusPopupWeight(patient, INFUUS_WEIGHT_POPUP_TYPE.MULTIPLE_CHOICE, isFromNotebook, isCurrentlyWaiting);
                    break;
                case TREATMENT_TYPE.INSULIN:
                    // Implement later maybe
                    break;
            }
        }

        private void UI_ANIM()
        {
            foreach (Image img in transform.GetComponentsInChildren<Image>())
            {
                Sequence seq = DOTween.Sequence();

                if (img != GetComponent<Image>())
                    seq.Append(img.DOFade(1, 0.25f));

                else
                    seq.Append(img.DOFade(0.78f, 0.25f));
            }

            foreach (TextMeshProUGUI txt in transform.GetComponentsInChildren<TextMeshProUGUI>())
            {
                Sequence seq = DOTween.Sequence();

                seq.Append(txt.DOFade(1, 0.25f));
            }

            transform.DOScale(1, 0.25f);
        }

        #region Casus
        private void CreateCasusPopup(PatientRoot patient, bool isFromNotebook, bool isCurrentlyWaiting)
        {
            dialogueText.text = patient.GetCaseDataSO.GetDialogueTxt(patient.GetPatientInfo.GetNameWithTitle());


            Option[] optionsToRandomize = patient.GetCaseDataSO.GetOptions.Take(patient.GetCaseDataSO.GetOptions.Length - 1).ToArray();

            // Shuffle using Unity's Random
            for (int i = optionsToRandomize.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1); // Random index between 0 and i (inclusive)
                                                // Swap elements at indices i and j
                var temp = optionsToRandomize[i];
                optionsToRandomize[i] = optionsToRandomize[j];
                optionsToRandomize[j] = temp;
            }

            for (int i = 0; i < patient.GetCaseDataSO.GetOptions.Length; i++)
            {
                ChoiceButton choiceBtn = Instantiate(choice_PREFAB, choicesParent, false)
                    .GetComponent<ChoiceButton>();

                if (i < optionsToRandomize.Length) 
                {
                    choiceBtn.Initialize(optionsToRandomize[i].GetText, patient, false,
                        optionsToRandomize[i].GetScoreType, isFromNotebook, isCurrentlyWaiting);
                }

                else 
                {
                    choiceBtn.Initialize(patient.GetCaseDataSO.GetOptions[i].GetText, patient, true, 0, isFromNotebook, isCurrentlyWaiting);
                }
            }

            UI_ANIM();
        }

        #endregion

        #region Intake
        private void CreateIntakePopup(PatientRoot patient, bool isFromNotebook, bool isCurrentlyWaiting) // Just have 1 option
        {
            PatientRoom assignedRoom = Utility_Functions.GetRandomAvailableRoom();

            dialogueText.text = patient.GetCaseDataSO.GetIntakeDialogue.GetDialogueTxt(
                patient.GetPatientInfo.GetNameWithTitle(), GetRoomName(assignedRoom));

            ChoiceButton choiceBtn = Instantiate(choice_PREFAB, choicesParent, false)
                .GetComponent<ChoiceButton>();

            choiceBtn.Initialize("Ok, is goed!", patient, false, 0, isFromNotebook, isCurrentlyWaiting); // REPLACE

            choiceBtn.gameObject.GetComponent<Button>().onClick.AddListener(() =>
            assignedRoom.AssignPatient(patient));

            UI_ANIM();

            string GetRoomName(PatientRoom room)
            {
                switch (room.GetUrgencyLevel)
                {
                    case URGENCY_TYPE.RED:
                        return "Een-persoonskamer.";
                    case URGENCY_TYPE.YELLOW:
                        return "Twee-persoonskamer.";
                    case URGENCY_TYPE.GREEN:
                        return "Vier-persoonskamer.";
                }

                return string.Empty;
            }
        }

        /* // Old version where you assign someone
        private void CreateIntakePopup(PatientRoot patient, bool isFromNotebook, bool isCurrentlyWaiting) // Just have 1 option
        {
            dialogueText.text = patient.GetCaseDataSO.GetIntakeDialogue.GetDialogueTxt(patient.GetPatientInfo.GetNameWithTitle());

            for (int i = 0; i < 3; i++)
            {
                ChoiceButton choiceBtn = Instantiate(choice_PREFAB, choicesParent, false)
                    .GetComponent<ChoiceButton>();

                if (i == 0)
                {
                    choiceBtn.Initialize("Eenpersoonskamer", patient, false, 0, isFromNotebook, isCurrentlyWaiting); // REPLACE
                    HandleButtonState(URGENCY_TYPE.RED, choiceBtn.GetComponent<Button>());
                    choiceBtn.gameObject.GetComponent<Button>().onClick.AddListener(() => 
                    AssignPatientToDesiredRoom(URGENCY_TYPE.RED, patient));
                }

                else if (i == 1)
                {
                    choiceBtn.Initialize("Tweepersoonskamer", patient, false, 0, isFromNotebook, isCurrentlyWaiting); // REPLACE
                    HandleButtonState(URGENCY_TYPE.YELLOW, choiceBtn.GetComponent<Button>());
                    choiceBtn.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                    AssignPatientToDesiredRoom(URGENCY_TYPE.YELLOW, patient));
                }

                else if (i == 2)
                {
                    choiceBtn.Initialize("Vierpersoonskamer", patient, false, 0, isFromNotebook, isCurrentlyWaiting); // REPLACE
                    HandleButtonState(URGENCY_TYPE.GREEN, choiceBtn.GetComponent<Button>());
                    choiceBtn.gameObject.GetComponent<Button>().onClick.AddListener(() => 
                    AssignPatientToDesiredRoom(URGENCY_TYPE.GREEN, patient));
                }
            }

            void AssignPatientToDesiredRoom(URGENCY_TYPE roomType, PatientRoot patient)
            {
                PatientRoom[] rooms = Utility_Functions.GetAllPatientRoomsWithUrgency(roomType);

                if (rooms.Length == 0) // Set button not-interactable?
                    return;

                PatientRoom roomWithSpace = rooms.FirstOrDefault(room => room.HasSpace());

                if (roomWithSpace == null)
                    return;

                roomWithSpace.AssignPatient(patient);
            }

            void HandleButtonState(URGENCY_TYPE roomType, Button btn)
            {
                if (Utility_Functions.GetAllPatientRoomsWithUrgency(roomType)
                    .Any(room => room.HasSpace()))
                    btn.interactable = true;

                else
                    btn.interactable = false;
            }

            UI_ANIM();
        }
        */
        #endregion

        #region Code
        private void CreateCodePopup(PatientRoot patient, bool isFromNotebook, bool isCurrentlyWaiting)
        {
            dialogueText.text = DialogueManager.Instance.GetGenericCodeSO.GetDialogueTxt(patient.GetPatientInfo.GetNameWithTitle());

            ChoiceButton continueButton = Instantiate(choice_PREFAB, choicesParent, false)
                .GetComponent<ChoiceButton>();

            continueButton.Initialize(DialogueManager.Instance.GetGenericCodeSO.GetOptions[0].GetText,
                patient, false, 0, isFromNotebook, isCurrentlyWaiting);

            UI_ANIM();
        }

        /* Old version with puzzle element
        private void CreateCodePopup(PatientRoot patient, bool isFromNotebook, bool isCurrentlyWaiting)
        {
            dialogueText.text = DialogueManager.Instance.GetGenericCodeSO.GetDialogueTxt(patient.GetPatientInfo.GetNameWithTitle());
            int correctAnswerIndex = UnityEngine.Random.Range(0, 2);

            for (int i = 0; i < 3; i++)
            {
                ChoiceButton choiceBtn = Instantiate(choice_PREFAB, choicesParent, false)
                    .GetComponent<ChoiceButton>();

                if (i == correctAnswerIndex)
                {
                    choiceBtn.Initialize(patient.GetPatientInfo.GetCode.ToString(),
                        patient, false, SCORE_TYPES.CODE_CORRECT, isFromNotebook, isCurrentlyWaiting);
                }

                else
                {
                    choiceBtn.Initialize(Utility_Functions.GenerateCodeExclude(
                        patient.GetPatientInfo.GetCode).ToString(),
                        patient, false, SCORE_TYPES.CODE_INCORRECT, isFromNotebook, isCurrentlyWaiting);
                }
            }

            ChoiceButton callbackButton = Instantiate(choice_PREFAB, choicesParent, false)
                .GetComponent<ChoiceButton>();

            callbackButton.Initialize(DialogueManager.Instance.GetGenericCodeSO.GetOptions[0].GetText, 
                patient, true, 0, isFromNotebook, isCurrentlyWaiting);
            UI_ANIM();
        }
        */
        #endregion

        #region Infuus
        private void CreateInfuusPopupWeight(PatientRoot patient, INFUUS_WEIGHT_POPUP_TYPE popupType, bool isFromNotebook
            , bool isCurrentlyWaiting)
        {
            dialogueText.text = DialogueManager.Instance.GetGenericInfuusWeightSO.GetDialogueTxt(patient.GetPatientInfo.GetNameWithTitle());

            CreateButtonsForWeight(patient, popupType);

            void CreateButtonsForWeight(PatientRoot patient, INFUUS_WEIGHT_POPUP_TYPE popupType)
            {
                int[] choices = new int[3];

                choices = choices.Select(i => Utility_Functions.GetRandomWeightExclude(patient.GetPatientInfo.GetWeight,
                    BalancingManager.Instance.GetData.IS_WEIGHT_CALCULATION_SIMPLIFIED)).ToArray(); // Get random weights

                choices[0] = patient.GetPatientInfo.GetWeight; // Set correct weight

                System.Random randNum = new System.Random();
                choices = choices.OrderBy(x => randNum.Next()).ToArray(); // Shuffle
                choices = choices.Select(x => x * 30).ToArray(); // Weight to ml ratio

                for (int i = 0; i < choices.Length; i++)
                {
                    ChoiceButton choiceBtn = Instantiate(choice_PREFAB, choicesParent, false)
                        .GetComponent<ChoiceButton>();

                    if (choices[i] == patient.GetPatientInfo.GetWeight)
                    {
                        choiceBtn.Initialize($"{choices[i]} mL.", patient, false, SCORE_TYPES.INFUUS_WEIGHT_CORRECT, isFromNotebook
                            , isCurrentlyWaiting);
                    }

                    else
                    {
                        choiceBtn.Initialize($"{choices[i]} mL.", patient, false, SCORE_TYPES.INFUUS_WEIGHT_INCORRECT, isFromNotebook
                            , isCurrentlyWaiting);
                    }
                }
            }


            ChoiceButton callbackButton = Instantiate(choice_PREFAB, choicesParent, false)
                .GetComponent<ChoiceButton>();

            callbackButton.Initialize(DialogueManager.Instance.GetGenericInfuusWeightSO.GetOptions[0].GetText, patient, true, 0, isFromNotebook
                , isCurrentlyWaiting);

            UI_ANIM();
        }
        #endregion
    }
}