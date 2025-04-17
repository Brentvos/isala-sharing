using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace New
{
    public class ChoiceButton : MonoBehaviour // Add FEEDBACK to generate after
    {
        [SerializeField] private TextMeshProUGUI text;

        public void Initialize(string text, PatientRoot patient, bool isDelayed, SCORE_TYPES scoreType, 
            bool isFromNotebook, bool isCurrentlyWaiting)
        {
            this.text.text = text;
            GetComponent<Button>().onClick.AddListener(() => OnClick(patient, isDelayed, scoreType, isFromNotebook
                , isCurrentlyWaiting)); // REPLACE
        }


        private void OnClick(PatientRoot patient, bool isDelayed, SCORE_TYPES scoreType,
            bool isFromNotebook, bool isCurrentlyWaiting) // REPLACE
        {
            ScoreManager.instance.UpdateScore(scoreType, patient);
            BeeperManager.Instance.ProcessPatient(patient, isDelayed, isFromNotebook, isCurrentlyWaiting); // REPLACE
        }
    }
}


        /* This is for INTAKE popups
        [Header("Option Buttons")]
        [SerializeField] private Button redRoom_BTN;
        [SerializeField] private Button yellowRoom_BTN;
        [SerializeField] private Button greenRoom_BTN;

        private PatientRoot patient;

        public void Initialize(PatientRoot patient)
        {
            this.patient = patient;

            PatientSpawningManager.Instance.OnFieldPatientSpawned += OnFieldPatientSpawned;
            PatientSpawningManager.Instance.OnFieldPatientRemoved += OnFieldPatientRemoved; // Implement this
            HandleButtonState();


            redRoom_BTN.onClick.AddListener(() => AssignPatientToDesiredRoom(URGENCY_TYPE.RED));
            yellowRoom_BTN.onClick.AddListener(() => AssignPatientToDesiredRoom(URGENCY_TYPE.YELLOW));
            greenRoom_BTN.onClick.AddListener(() => AssignPatientToDesiredRoom(URGENCY_TYPE.GREEN));
        }

        private void OnDestroy()
        {
            PatientSpawningManager.Instance.OnFieldPatientSpawned -= OnFieldPatientSpawned;
            PatientSpawningManager.Instance.OnFieldPatientRemoved -= OnFieldPatientRemoved;
        }

        private void OnFieldPatientRemoved(PatientRoot patient)
        {
            HandleButtonState();
        }

        private void OnFieldPatientSpawned(PatientRoot patient)
        {
            HandleButtonState();
        }

        private void AssignPatientToDesiredRoom(URGENCY_TYPE roomType)
        {
            PatientRoom[] rooms = Utility_Functions.GetAllPatientRoomsWithUrgency(roomType);

            if (rooms.Length == 0) // Set button not-interactable?
                return;

            PatientRoom roomWithSpace = rooms.FirstOrDefault(room => room.HasSpace());

            if (roomWithSpace == null)
                return;

            roomWithSpace.AssignPatient(patient);
            Destroy(gameObject); // Wacht misschien voor later
        }

        private void HandleButtonState()
        {
            if (Utility_Functions.GetAllPatientRoomsWithUrgency(URGENCY_TYPE.RED)
                .Any(room => room.HasSpace()))
                redRoom_BTN.interactable = true;

            else
                redRoom_BTN.interactable = false;

            if (Utility_Functions.GetAllPatientRoomsWithUrgency(URGENCY_TYPE.YELLOW)
                .Any(room => room.HasSpace()))
                yellowRoom_BTN.interactable = true;

            else
                yellowRoom_BTN.interactable = false;

            if (Utility_Functions.GetAllPatientRoomsWithUrgency(URGENCY_TYPE.GREEN)
                .Any(room => room.HasSpace()))
                greenRoom_BTN.interactable = true;

            else
                greenRoom_BTN.interactable = false;
        }
        */