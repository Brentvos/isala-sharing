using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OLD
{
    public class NurseInfoElement : MonoBehaviour
    {
        [SerializeField] private GameObject draggableElement;
        [SerializeField] private GameObject draggableParentElement;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI nurseNameText;
        [SerializeField] private TextMeshProUGUI taskStateText;
        [SerializeField] private TextMeshProUGUI moraleStateText;
        [SerializeField] private TextMeshProUGUI dragText;
        [SerializeField] private TextMeshProUGUI degressionText;

        [Header("Images")]
        [SerializeField] private Image moraleDisplay;
        [SerializeField] private Image moraleEmoji;
        [SerializeField] private Image moraleEmojiTwo;


        [Header("Emojis")]
        [SerializeField] private Sprite emojiHappy;
        [SerializeField] private Sprite emojiNeutral;
        [SerializeField] private Sprite emojiSad;
        [SerializeField] private Sprite emojiAngry;

        private Color colourHappy = Color.green;
        private Color colourNeutral = Color.yellow;
        private Color colourSad = Color.magenta;
        private Color colourAngry = Color.red;

        private NurseData nurseData;
        public NurseData GetNurseData { get { return nurseData; } }

        public void Initialize(NurseData nurse)
        {
            nurseData = nurse;
            nurseNameText.text = "Nurse: " + nurseData.GetName;
            dragText.text = "DRAG";

            NurseData_OnMoraleStateChanged(nurseData);
            NurseData_OnNurseStateChanged(nurseData.GetNurseState);
            nurseData.GetMoraleData.ChangeMoraleDegressionState(nurseData.GetNurseState);

            nurseData.OnMoraleStateChanged += NurseData_OnMoraleStateChanged;
            nurseData.OnNurseStateChanged += NurseData_OnNurseStateChanged;

            GameObject draggableNurseElement = Instantiate(draggableElement, draggableParentElement.transform, false);
            draggableNurseElement.GetComponent<DraggableNurseElement>().Initialize(nurseData);
        }


        private void NurseData_OnNurseStateChanged(NurseState state)
        {
            taskStateText.text = "Task State: " + state.ToString();

            if (state == NurseState.Available)
                dragText.text = "DRAG";

            else if (state == NurseState.No_Morale)
                dragText.text = "OUT";

            else
                dragText.text = "BUSY";


            if (nurseData.GetMoraleData.GetMoraleDegressionState == MoraleDegressionRate.Low)
                degressionText.text = "<";

            else if (nurseData.GetMoraleData.GetMoraleDegressionState == MoraleDegressionRate.Medium)
                degressionText.text = "<<";

            else if (nurseData.GetMoraleData.GetMoraleDegressionState == MoraleDegressionRate.High)
                degressionText.text = "<<<";

            else if (nurseData.GetMoraleData.GetMoraleDegressionState == MoraleDegressionRate.None)
                degressionText.text = "X";

        }

        private void NurseData_OnMoraleStateChanged(NurseData nurseData)
        {
            moraleStateText.text = nurseData.GetMoraleData.GetMoraleState.ToString();

            switch (nurseData.GetMoraleData.GetMoraleState)
            {
                case MoraleState.High:
                    moraleDisplay.color = colourHappy;
                    moraleEmoji.sprite = emojiHappy;
                    moraleEmojiTwo.sprite = emojiHappy;
                    NurseData_OnNurseStateChanged(nurseData.GetNurseState); // This is debatable
                    break;

                case MoraleState.Medium:
                    moraleDisplay.color = colourNeutral;
                    moraleEmoji.sprite = emojiNeutral;
                    moraleEmojiTwo.sprite = emojiNeutral;
                    NurseData_OnNurseStateChanged(nurseData.GetNurseState); // This is debatable
                    break;

                case MoraleState.Low:
                    moraleDisplay.color = colourSad;
                    moraleEmoji.sprite = emojiSad;
                    moraleEmojiTwo.sprite = emojiSad;
                    NurseData_OnNurseStateChanged(nurseData.GetNurseState); // This is debatable
                    break;

                case MoraleState.Empty:
                    moraleDisplay.color = colourAngry;
                    moraleEmoji.sprite = emojiAngry;
                    moraleEmojiTwo.sprite = emojiAngry;
                    NurseData_OnNurseStateChanged(nurseData.GetNurseState); // This is debatable
                    break;
            }
        }
    }
}