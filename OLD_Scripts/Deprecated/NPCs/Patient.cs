using System;
using Unity.VisualScripting;
using UnityEngine;

namespace OLD
{
    public class Patient : MonoBehaviour
    {
        [SerializeField] private NpcProfile profile;
        [SerializeField] private PatientState state;
        [SerializeField] private PatientTask currentTask;

        [SerializeField] private float currentTaskDuration;
        [SerializeField] private float currentTimeAfterTask;
        [SerializeField] private float taskInterval = 5;

        public PatientState GetState { get { return state; } }
        public NpcProfile GetProfile { get { return profile; } }

        private void Awake()
        {
            // state = PatientState.InDatabase;
        }

        public void Initialize()
        {
            profile.RandomizeNpcProfile();
            state = PatientState.Default;
        }

        public void StartTreatment(Helper helper)
        {
            state = PatientState.InTreatment;
            currentTaskDuration = 0;
            currentTask = new PatientTask(helper);
        }

        public void CompleteTreatment(Helper helper)
        {
            currentTask = null;
            state = PatientState.RecentlyTreated;
        }

        public void CompleteDataRequest()
        {
            state = PatientState.InTreatmentPhaseTwo;
        }

        private void Update()
        {
            if (state == PatientState.InTreatment &&
                currentTaskDuration < currentTask.totalDuration)
            {
                if (currentTaskDuration > currentTask.dataRequestMoment)
                {
                    state = PatientState.RequireAdditionalData;
                    return;
                }

                else
                {
                    currentTaskDuration += Time.deltaTime;
                }
            }

            else if (state == PatientState.InTreatmentPhaseTwo &&
                currentTaskDuration < currentTask.totalDuration)
            {
                currentTaskDuration += Time.deltaTime;
            }

            else if (state == PatientState.InTreatmentPhaseTwo &&
                currentTaskDuration >= currentTask.totalDuration)
            {
                CompleteTreatment(currentTask.connectedHelper);
                return;
            }

            else if (state == PatientState.RecentlyTreated &&
                currentTimeAfterTask < taskInterval)
            {
                currentTimeAfterTask += Time.deltaTime;
            }

            else if (state == PatientState.RecentlyTreated &&
                currentTimeAfterTask >= taskInterval)
            {
                currentTimeAfterTask = 0;
                state = PatientState.OpenForTreatment;
            }
        }
    }

    [Serializable]
    public class PatientTask
    {
        public float totalDuration;
        public float dataRequestMoment;
        public Helper connectedHelper;

        public PatientTask(Helper helper)
        {
            connectedHelper = helper;
            totalDuration = 3;
            dataRequestMoment = 1;
        }
    }
}