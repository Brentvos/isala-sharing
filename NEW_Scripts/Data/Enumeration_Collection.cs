namespace New
{
    public enum LAST_NAME
    {
        Bakker, Janssen, Meijer, Boer, Dijkstra,
        Vermeer, Molenaar, Smit, Maas, Koster,
        Oost, Kamphuis, Haan, Baten, Veen,
        Luong, Kelder, Knaapen, Postma, Blom,
        Vos, Vlegels, Yin, Abbema, Nauta,
    }

    public enum GENDER
    {
        FEMALE,
        MALE
    }

    public enum PATIENT_TYPE
    { 
        DEFAULT,
        SYMPTOM,
        INFUUS
    }

    public enum URGENCY_TYPE
    {
        RED,
        YELLOW,
        GREEN
    }

    public enum ROOM_TYPE
    {
        PATIENT,
        DOCTOR,
        CANTEEN

        /*
        HALLWAY,
        NURSE,
        PLAYER
        */
    }

    public enum BED_ROTATION
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    }

    public enum MOVEMENT_TYPE
    {
        WASDSNAPPY,
        WASDLOOSE,
        DRAGGING
    }

    public enum ADMISSION_REASON
    {
        DEFAULT
    }

    public enum TREATMENT_TYPE
    {
        WAITING,
        INTAKE,
        CODE,
        CASUS_GENERIC,
        INFUUS_LIQUID,
        INFUUS_WEIGHT,
        INSULIN,
        DONE
    }

    public enum INFUUS_WEIGHT_POPUP_TYPE
    {
        FREE_ANSWER,
        MULTIPLE_CHOICE,
        DROPDOWN
    }

    public enum INFUUS_TYPE
    {
        VOCHT,
        ZOUT,
        GLUCOSE
    }

    public enum TREATMENT_STATE
    {
        TO_DO,
        PENDING,
        DONE
    }

    public enum SCORE_TYPES
    {
        NONE,

        CASUS_BEST,
        CASUS_MEDIUM,
        CASUS_WORST,

        CODE_INCORRECT,
        CODE_CORRECT,

        INFUUS_LIQUID_CORRECT,
        INFUUS_LIQUID_INCORRECT,

        INFUUS_WEIGHT_CORRECT,
        INFUUS_WEIGHT_INCORRECT,

        BEEPER_PICKUP_FAILED,
        BEEPER_PICKUP_SUCCESS,

        PATIENT_LOST_ON_HEALTH
    }
}
