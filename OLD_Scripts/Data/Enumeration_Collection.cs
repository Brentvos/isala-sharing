using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLD
{


    public class Enumeration_Collection
    {
    }

    public enum First_Names
    {
        James, Michael, Robert, John, David, William, Richard, Joseph, Thomas, Christopher,
        Mary, Patricia, Jennifer, Linda, Elizabeth, Barbara, Susan, Jessica, Karen, Sarah
    }

    public enum DragCancelReason { WallCollision, MouseUp, PatientCollision };

    public enum NurseState
    {
        Available,
        In_Treatment,
        Requiring_Data,
        No_Morale
    }

    public enum PatientState
    {
        InDatabase, // Used to catch them and put them on playing field
        Default, // When nothing is happening
        OpenForTreatment,
        InTreatment,
        RequireAdditionalData,
        InTreatmentPhaseTwo,
        RecentlyTreated
    }

    public enum MoraleDegressionRate
    {
        None,
        Low,
        Medium,
        High
    }

    public enum HealthManageState
    {
        None,
        HealthyGain,
        TreatmentGain,
        WaitingForTreatmentLoss,
        WaitingForDataLoss,
        Dead
    }

    public enum MoraleState
    {
        High,
        Medium,
        Low,
        Empty
    }

    public enum InfoType
    {
        Weight,
        SugarLevels
    }
}

