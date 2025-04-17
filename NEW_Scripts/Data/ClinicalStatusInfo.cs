using System;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace New
{
    /// <summary>
    /// This is the most basic patient info
    /// </summary>

    [Serializable]
    public class ClinicalStatusInfo
    {
        private float curValue;
        private float maxValue;
        private float baseDegression;
        private float totalDegression;
        private float roomProgressionBuff;
        private bool isDegressionOverridden = false;

        public float GetMaxValue { get { return maxValue; } }
        public float GetRoomProgressionBuff { get {  return roomProgressionBuff; } }
        public float GetTotalDegression { get { return totalDegression; } }
        public bool IsDegressionOverridden { get { return  isDegressionOverridden; } }

        public event Action<float> OnChangeValue;

        public ClinicalStatusInfo()
        {
            maxValue = 100f;
            curValue = maxValue;
        }

        public void SetBalancingStats()
        {
            baseDegression = BalancingManager.Instance.GetData.GET_PATIENT_BASE_DIGRESSION;
            roomProgressionBuff = BalancingManager.Instance.GetData.GET_PATIENT_BUFF_WHILE_PLAYER_IN_ROOM;
            totalDegression = baseDegression;
        }

        public void OverrideDegression(bool overrideDegression)
        {
            isDegressionOverridden = overrideDegression;
        }

        public void ChangeDegressionRate(float amount)
        {
            totalDegression += amount;
        }

        public void ChangeValue(float amount)
        {
            curValue = Mathf.Clamp(curValue += amount, 0, maxValue);
            OnChangeValue?.Invoke(curValue);
        }
    }
}