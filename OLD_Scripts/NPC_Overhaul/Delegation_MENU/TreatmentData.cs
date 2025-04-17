using System;
using UnityEngine;

namespace OLD
{

    [Serializable]
    public class TreatmentData
    {
        [SerializeField] private float duration, dataRequestPoint, interval, startInterval;

        [HideInInspector] public float TimeSinceTreatmentStarted, TimeSinceTreatmentCompleted;
        public float GetDuration { get { return duration; } }
        public float GetDataRequestPoint { get { return dataRequestPoint; } }
        public float GetInterval { get { return interval; } }
        public float GetStartInterval { get { return startInterval; } }

        public TreatmentData(float duration, float dataRequestPoint, float interval, float startInterval)
        {
            this.duration = duration;
            this.dataRequestPoint = dataRequestPoint;
            this.interval = interval;
            this.startInterval = startInterval;
        }
    }
}