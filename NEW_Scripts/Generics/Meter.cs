using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace New
{
    [Serializable]
    public class Meter
    {
        [SerializeField] private Image progressionImage = default;
        [SerializeField] private bool hasGradient = false;
        [SerializeField] private Gradient progressionGradient;

        private float maxValue;

        public void Initialize(Player player)
        {
            this.maxValue = player.GetMaxValue;
            UpdateMeter(maxValue, true); // Can be changed later

            player.OnChangeValue += Player_OnChangeValue;
        }

        public void Initialize(ClinicalStatusInfo status)
        {
            this.maxValue = status.GetMaxValue;
            UpdateMeter(maxValue, true); // Can be changed later

            status.OnChangeValue += Status_OnChangeValue;
        }

        public void Initialize(PatientRoot patient) // For tasks, always dependant on GetTimeToWait
        {
            this.maxValue = BalancingManager.Instance.GetData.GET_TIME_TO_WAIT_FOR_TASK_BASE;
            UpdateMeter(0, true); // Can be changed later

            patient.OnTaskTimeChanged += Task_OnChangeValue;
        }

        public void ResetBar()
        {
            progressionImage.fillAmount = 0;
        }

        private void Status_OnChangeValue(float newAmount)
        {
            UpdateMeter(newAmount, false, 1f);
        }

        private void Task_OnChangeValue(float newAmount)
        {
            UpdateMeter(newAmount, true);
        }
        private void Player_OnChangeValue(float newAmount)
        {
            UpdateMeter(newAmount, false, 3f);
        }

        private void UpdateMeter(float currentValue, bool doFast, float speed = 1f)
        {
            float targetValue = Mathf.Clamp01(currentValue / maxValue);

            if (doFast)
                progressionImage.fillAmount = targetValue;

            else
                progressionImage.DOFillAmount(targetValue, speed);

            if (hasGradient)
                progressionImage.color = progressionGradient.Evaluate(targetValue);
        }
    }
}