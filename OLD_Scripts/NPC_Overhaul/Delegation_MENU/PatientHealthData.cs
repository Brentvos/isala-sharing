using System;
using UnityEngine;

namespace OLD
{


    [Serializable]
    public class PatientHealthData
    {
        [SerializeField] private float defaultHealthGainRate = default;
        [SerializeField] private float defaultHealthLossRate = default;
        [SerializeField] private float heightenedHealthLossRate = default;
        [Space(5)]
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float startingHealth = 75;
        [SerializeField] private float healthOnTaskComplete = 50;
        private float currentChangeRate;

        [SerializeField] private HealthManageState healthManageState = HealthManageState.None;
        [SerializeField] private Sprite deathSprite;

        public float GetDefaultHealthGainRate { get { return defaultHealthGainRate; } }
        public float GetDefaultHealthLossRate { get { return defaultHealthLossRate; } }
        public float GetHeightenedHealthLossRate { get { return heightenedHealthLossRate; } }
        public float GetMaxHealth { get { return maxHealth; } }
        public float GetStartingHealth { get { return startingHealth; } }
        public float GetHealthOnTaskComplete { get { return healthOnTaskComplete; } }
        public float GetChangeRate { get { return currentChangeRate; } }
        public HealthManageState GetHealthManageState { get { return healthManageState; } }

        public Sprite GetDeathSprite { get { return deathSprite; } }

        public float CurrentHealth;

        public PatientHealthData(float defaultHealthGainRate, float defaultHealthLossRate, float heightenedHealthLossRate,
            float maxHealth, float startingHealth, float healthOnTaskComplete, HealthManageState healthManageState)
        {
            this.defaultHealthGainRate = defaultHealthGainRate;
            this.defaultHealthLossRate = defaultHealthLossRate;
            this.heightenedHealthLossRate = heightenedHealthLossRate;
            this.maxHealth = maxHealth;
            this.startingHealth = startingHealth;
            this.healthOnTaskComplete = healthOnTaskComplete;

            this.healthManageState = healthManageState;
            CurrentHealth = startingHealth;
        }

        public void ChangeValue(float value, PatientData patient, bool isGradual = true) // Don't use isGradual = false. It doesn't work rn
        {
            CurrentHealth = Mathf.Clamp(
                CurrentHealth + value, 0, GetMaxHealth);

            if (CurrentHealth <= 0)
            {
                patient.PatientZeroHealthSignal();
            }
        }

        public void ChangeHealthState(HealthManageState state)
        {
            healthManageState = state;

            switch (healthManageState)
            {
                case HealthManageState.None:
                    currentChangeRate = 0;
                    break;
                case HealthManageState.HealthyGain:
                    currentChangeRate = defaultHealthGainRate;
                    break;
                case HealthManageState.TreatmentGain:
                    currentChangeRate = defaultHealthGainRate;
                    break;
                case HealthManageState.WaitingForTreatmentLoss:
                    currentChangeRate = -defaultHealthLossRate;
                    break;
                case HealthManageState.WaitingForDataLoss:
                    currentChangeRate = -heightenedHealthLossRate;
                    break;
                case HealthManageState.Dead:
                    currentChangeRate = 0;
                    break;
            }
        }
    }
}