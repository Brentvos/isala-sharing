using System;
using UnityEngine;

namespace New
{
    [Serializable]
    public class Treatment
    {
        [SerializeField] private TREATMENT_TYPE type;
        public TREATMENT_TYPE GetTreatmentType { get { return type; } }

        // Moved to BALANCING, can use ENUM here like with the scores

        //[Header("WAITING_TYPE")] 
        //[SerializeField] private int timeToWait;
       // public int GetTimeToWait { get { return timeToWait; } }

        // [Header("CASUS_TYPE")]
        // SO for casus

        // [Header("INTAKE_TYPE")]
        // SO for intake, might need SO like with casus - These need to be related tightly

        // [Header("CODE_TYPE")]
    }
}
