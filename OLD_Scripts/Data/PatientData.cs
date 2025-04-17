using System;
using System.Collections.Generic;
using UnityEngine;

namespace OLD
{


    [Serializable]
    public class InsulinPatientData
    {
        private int weight;
        private int initialSugarLevels;
        private bool isWeightHidden;
        private bool isSugarLevelsHidden;

        public int GetWeight { get { return weight; } }
        public int GetInitialSugarLevels { get { return initialSugarLevels; } }
        public bool IsWeightHidden { get { return isWeightHidden; } }
        public bool IsSugarLevelsHidden { get { return isSugarLevelsHidden; } }

        public InsulinPatientData(int weight, int initialSugarLevels, bool isWeightHidden, bool isSugarLevelsHidden)
        {
            this.weight = weight;
            this.initialSugarLevels = initialSugarLevels;
            this.isWeightHidden = isWeightHidden;
            this.isSugarLevelsHidden = isSugarLevelsHidden;
        }

        public void SetHiddenState(bool revealInfo, InfoType type)
        {
            switch (type)
            {
                case InfoType.Weight:
                    isWeightHidden = !revealInfo;
                    break;
                case InfoType.SugarLevels:
                    isSugarLevelsHidden = !revealInfo;
                    break;
            }
        }
    }

    [Serializable]
    public class PatientData
    {
        [SerializeField] private First_Names name; // Temporary to TRACK state
        [SerializeField] private int code, age; // Temporary to TRACK state
        [SerializeField] private PatientState state = PatientState.InDatabase; // Temporary to TRACK state
        [Space(10)]
        [SerializeField] private TreatmentData treatmentData;
        [SerializeField] private PatientHealthData healthData;

        [Space(10)]
        [Header("Insulin")]
        [SerializeField] private bool isInsulinPatient;
        [SerializeField] private InsulinPatientData insulinData;

        public NurseInfoElement CurrentAttachedNurseElementReference;

        public bool IsInsulinPatient { get { return isInsulinPatient; } }
        public InsulinPatientData GetInsulinData { get { return insulinData; } }

        private float currentSugarLevels;

        public event Action<PatientState> OnStateChange;
        public event Action<PatientData> OnStateChangeDetailed; // Might replace the OnStateChange with this?
        public event Action<InfoType> OnInsulinInfoUpdated;
        public event Action OnDataIncorrectlyGiven;
        public event Action OnPatientHelped;
        public event Action OnPatientDeath;

        public int GetAge { get { return age; } }
        public int GetCode { get { return code; } }
        public First_Names GetName { get { return name; } }
        public PatientState GetState { get { return state; } }
        public TreatmentData GetTreatmentData { get { return treatmentData; } }
        public PatientHealthData GetHealthData { get { return healthData; } }

        public PatientData(First_Names name, int age, int code)
        {
            this.name = name;
            this.age = age;
            this.code = code;

            state = PatientState.Default;
            treatmentData = new TreatmentData(UnityEngine.Random.Range(30, 45), UnityEngine.Random.Range(15, 25),
                UnityEngine.Random.Range(20, 40), UnityEngine.Random.Range(10f, 60f));

            healthData = new PatientHealthData(5, 2, 5, 100, 100, 50, HealthManageState.None);
            isInsulinPatient = false;
        }

        public PatientData(First_Names name, int age, int code, int sugarLevels, int weight, bool hasHiddenSugarLevels, bool hasHiddenWeight)
        {
            this.name = name;
            this.age = age;
            this.code = code;
            this.insulinData = new InsulinPatientData(weight, sugarLevels, hasHiddenWeight, hasHiddenSugarLevels);

            state = PatientState.Default;
            treatmentData = new TreatmentData(UnityEngine.Random.Range(30, 45), UnityEngine.Random.Range(15, 25),
                UnityEngine.Random.Range(20, 40), UnityEngine.Random.Range(10f, 60f));

            healthData = new PatientHealthData(5, 2, 5, 100, 100, 50, HealthManageState.None);
            isInsulinPatient = true;
        }

        public void GenerateNewTreatmentData()
        {
            treatmentData = new TreatmentData(UnityEngine.Random.Range(30, 45), UnityEngine.Random.Range(15, 25),
                UnityEngine.Random.Range(20, 40), UnityEngine.Random.Range(10f, 60f));
        }

        public void UpdateInfo(bool revealInfo, InfoType type)
        {
            insulinData.SetHiddenState(revealInfo, type);
            OnInsulinInfoUpdated?.Invoke(type);
        }

        public void ChangeState(PatientState state)
        {
            this.state = state;
            OnStateChange?.Invoke(state);
            OnStateChangeDetailed?.Invoke(this);

            switch (state)
            {
                case PatientState.InDatabase:
                    break;
                case PatientState.Default:
                    break;
                case PatientState.OpenForTreatment:
                    break;
                case PatientState.InTreatment:
                    CurrentAttachedNurseElementReference.GetNurseData.NurseStateChanged(NurseState.In_Treatment);
                    break;
                case PatientState.RequireAdditionalData:
                    CurrentAttachedNurseElementReference.GetNurseData.NurseStateChanged(NurseState.Requiring_Data);
                    break;
                case PatientState.InTreatmentPhaseTwo:
                    CurrentAttachedNurseElementReference.GetNurseData.NurseStateChanged(NurseState.In_Treatment);
                    break;
                case PatientState.RecentlyTreated:
                    CurrentAttachedNurseElementReference.GetNurseData.NurseStateChanged(NurseState.Available);
                    OnPatientHelped?.Invoke();
                    break;
            }
        }

        public void DataIncorrectlyGiven()
        {
            OnDataIncorrectlyGiven?.Invoke();
        }
        public void PatientZeroHealthSignal()
        {
            OnPatientDeath?.Invoke();
        }
    }
}