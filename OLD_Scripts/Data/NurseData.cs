using System;
using UnityEngine;

namespace OLD
{


    [Serializable]
    public class NurseData
    {
        private int code, age;
        private First_Names name;
        private NurseState nurseState;

        [SerializeField] private NurseMoraleData moraleData;

        public int GetAge { get { return age; } }
        public int GetCode { get { return code; } }
        public First_Names GetName { get { return name; } }
        public NurseState GetNurseState { get { return nurseState; } }
        public NurseMoraleData GetMoraleData { get { return moraleData; } }
        public event Action<NurseData> OnMoraleStateChanged;
        public event Action<NurseState> OnNurseStateChanged;
        public event Action OnMoraleEmpty;


        private NurseState stateWhenMoraleReachesZero; // Used to remember the state?


        public NurseData(First_Names name, int age, int code)
        {
            this.name = name;
            this.age = age;
            this.code = code;

            nurseState = NurseState.Available;

            //////////////// A: Low but constant morale loss rate \\\\\\\\\\\\\\\
            moraleData = new NurseMoraleData(UnityEngine.Random.Range(1f, 2f),
                UnityEngine.Random.Range(3f, 4f), UnityEngine.Random.Range(5f, 6f), 100,
                UnityEngine.Random.Range(50, 100), UnityEngine.Random.Range(8f, 12f), MoraleDegressionRate.Low);

            //////////////// B: High but not constant morale loss rate \\\\\\\\\\\\\\\
            //moraleData = new NurseMoraleData(3,
            //    7, 12, 100,
            //    UnityEngine.Random.Range(50, 100), UnityEngine.Random.Range(8f, 12f), MoraleDegressionRate.Low);
        }

        public void MoraleStateChanged(MoraleState state)
        {
            if (state == MoraleState.Empty)
            {
                //stateWhenMoraleReachesZero = nurseState;
                //NurseStateChanged(NurseState.No_Morale);
                OnMoraleEmpty?.Invoke();
            }

            else
            {
                GetMoraleData.ChangeMoraleDegressionState(nurseState);
            }

            GetMoraleData.ChangeMoraleState(state);
            OnMoraleStateChanged?.Invoke(this);

        }

        public void NurseStateChanged(NurseState state)
        {
            this.nurseState = state;
            GetMoraleData.ChangeMoraleDegressionState(state);
            OnNurseStateChanged?.Invoke(state);
        }
    }
}