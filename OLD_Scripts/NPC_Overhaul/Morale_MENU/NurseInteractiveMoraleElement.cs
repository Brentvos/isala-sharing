using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace OLD
{


    public class NurseInteractiveMoraleElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nurseNameText = default;
        [SerializeField] private Button moraleBoostButton;

        [SerializeField] private Image moraleDisplay;
        [SerializeField] private Image moraleEmoji;

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

        [Space(5)]
        [SerializeField] private Meter meter = default;

        private MoraleState oldMoraleState;

        public void Initialize(NurseData nurse)
        {
            nurseData = nurse;
            nurseNameText.text = nurse.GetName.ToString();

            meter.Initialize(nurseData.GetMoraleData.GetMaxMorale, nurseData.GetMoraleData.CurrentMorale);

            moraleBoostButton.onClick.AddListener(() => OnClickMoraleBoost(nurseData.GetMoraleData.GetMoraleOnClick));

            ManageMoraleState();

            nurseData.OnMoraleStateChanged += NurseData_OnMoraleStateChanged;
        }

        private void Awake()
        {
            StartCoroutine(meter.UpdateProgressionPrediction());
        }

        private void Update()
        {
            if (nurseData.GetMoraleData.GetMoraleDegressionState == MoraleDegressionRate.None)
                return;

            ManageDegressionRate();
        }

        private void ManageDegressionRate()
        {
            switch (nurseData.GetMoraleData.GetMoraleDegressionState)
            {
                case MoraleDegressionRate.None:
                    break;
                case MoraleDegressionRate.Low:
                    ChangeValue(-nurseData.GetMoraleData.GetLowDegressionRate * Time.deltaTime, false);
                    break;
                case MoraleDegressionRate.Medium:
                    ChangeValue(-nurseData.GetMoraleData.GetMediumDegressionRate * Time.deltaTime, false);
                    break;
                case MoraleDegressionRate.High:
                    ChangeValue(-nurseData.GetMoraleData.GetHighDegressionRate * Time.deltaTime, false);
                    break;
            }
        }

        public void ChangeValue(float value, bool isGradual = true) // Don't use isGradual = false. It doesn't work rn
        {
            nurseData.GetMoraleData.CurrentMorale = Mathf.Clamp(
                nurseData.GetMoraleData.CurrentMorale + value, 0, nurseData.GetMoraleData.GetMaxMorale);

            ManageMoraleState();
            meter.UpdateMeterVisuals(nurseData.GetMoraleData.CurrentMorale, isGradual); // Explicitly called because it is a child                
        }

        private void ManageMoraleState()
        {
            if (nurseData.GetMoraleData.CurrentMorale >= 75)
                oldMoraleState = MoraleState.High;

            else if (nurseData.GetMoraleData.CurrentMorale < 75 && nurseData.GetMoraleData.CurrentMorale >= 25)
                oldMoraleState = MoraleState.Medium;

            else if (nurseData.GetMoraleData.CurrentMorale > 0 && nurseData.GetMoraleData.CurrentMorale < 25)
                oldMoraleState = MoraleState.Low;

            else
                oldMoraleState = MoraleState.Empty;

            if (nurseData.GetMoraleData.GetMoraleState != oldMoraleState)
            {
                nurseData.MoraleStateChanged(oldMoraleState);
            }
        }
        public void OnClickMoraleBoost(float amount)
        {
            ChangeValue(amount);
        }

        private void NurseData_OnMoraleStateChanged(NurseData nurseData)
        {
            switch (nurseData.GetMoraleData.GetMoraleState)
            {
                case MoraleState.High:
                    moraleDisplay.color = colourHappy;
                    moraleEmoji.sprite = emojiHappy;
                    break;

                case MoraleState.Medium:
                    moraleDisplay.color = colourNeutral;
                    moraleEmoji.sprite = emojiNeutral;
                    break;

                case MoraleState.Low:
                    moraleDisplay.color = colourSad;
                    moraleEmoji.sprite = emojiSad;
                    break;

                case MoraleState.Empty:
                    moraleDisplay.color = colourAngry;
                    moraleEmoji.sprite = emojiAngry;
                    break;
            }
        }
    }
}