using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLD_2
{
    [Serializable]
    public class PatientState
    {
        [SerializeField] private StandardPatientStates patientState;
        [SerializeField] private InsulinPatientStates insulinPatientState;


    }

    public enum StandardPatientStates
    {
        Default,
        OpenForTreatment,
        InTreatment,
        RequireAdditionalData,
        InTreatmentPhaseTwo,
        RecentlyTreated
    }

    public enum InsulinPatientStates
    {
        Default,
        OpenForInsulin,
        IncorrectInsulinGiven,
        CorrectInsulinGiven
    }

    public enum PatientType 
    {
        Standard,
        Insulin
    }
}
