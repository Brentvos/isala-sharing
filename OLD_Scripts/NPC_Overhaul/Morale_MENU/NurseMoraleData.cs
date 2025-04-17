using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLD
{


    [Serializable]
    public class NurseMoraleData
    {
        [SerializeField] private float lowDegresionRate = default;
        [SerializeField] private float mediumDegressionRate = default;
        [SerializeField] private float highDegressionRate = default;
        [Space(5)]
        [SerializeField] private float maxMorale = 100;
        [SerializeField] private float startingMorale = 75;
        [SerializeField] private float moraleOnClick = 10;

        [SerializeField] private MoraleDegressionRate moraleDegressionState = MoraleDegressionRate.Low;
        [SerializeField] private MoraleState moraleState = MoraleState.Medium;

        public float GetLowDegressionRate { get { return lowDegresionRate; } }
        public float GetMediumDegressionRate { get { return mediumDegressionRate; } }
        public float GetHighDegressionRate { get { return highDegressionRate; } }
        public float GetMaxMorale { get { return maxMorale; } }
        public float GetStartingMorale { get { return startingMorale; } }
        public float GetMoraleOnClick { get { return moraleOnClick; } }
        public MoraleDegressionRate GetMoraleDegressionState { get { return moraleDegressionState; } }
        public MoraleState GetMoraleState { get { return moraleState; } }

        public float CurrentMorale;

        public NurseMoraleData(float lowDegressionRate, float mediumDegressionRate, float highDegressionRate,
            float maxMorale, float startingMorale, float moraleOnClick, MoraleDegressionRate degressionRate)
        {
            this.lowDegresionRate = lowDegressionRate;
            this.mediumDegressionRate = mediumDegressionRate;
            this.highDegressionRate = highDegressionRate;
            this.maxMorale = maxMorale;
            this.startingMorale = startingMorale;
            this.moraleOnClick = moraleOnClick;

            moraleDegressionState = degressionRate;
            CurrentMorale = startingMorale;
        }

        public void ChangeMoraleState(MoraleState state)
        {
            moraleState = state;

            if (state == MoraleState.Empty)
                moraleDegressionState = MoraleDegressionRate.None;
        }

        //////////////// A: Always lose morale, have morale loss rate be relatively low \\\\\\\\\\\\\\\
        public void ChangeMoraleDegressionState(NurseState nurseState)
        {
            if (nurseState == NurseState.Available)
                moraleDegressionState = MoraleDegressionRate.Low;

            else if (nurseState == NurseState.In_Treatment)
                moraleDegressionState = MoraleDegressionRate.Medium;

            else if (nurseState == NurseState.Requiring_Data)
                moraleDegressionState = MoraleDegressionRate.High;
        }

        //////////////// B: Only lose morale on task, have morale loss rate be relatively high \\\\\\\\\\\\\\\

        /*
        public void ChangeMoraleDegressionRate(NurseState nurseState)
        {
            if (nurseState == NurseState.Available)
                moraleDegressionState = MoraleDegressionRate.None;

            else if (nurseState == NurseState.In_Treatment)
                moraleDegressionState = MoraleDegressionRate.Medium;

            else if (nurseState == NurseState.Requiring_Data)
                moraleDegressionState = MoraleDegressionRate.High;
        }
        */
    }
}