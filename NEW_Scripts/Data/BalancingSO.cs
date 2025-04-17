
using System.Collections.Generic;
using UnityEngine;

namespace New
{
    [CreateAssetMenu(fileName = "New Balancing Object", menuName = "Balancing")]
    public class BalancingSO : ScriptableObject
    {
        #region Tasks
        [Header("Tasks")]
        [SerializeField] private int TIME_TO_PICK_UP_BEEPER_BASE = 5;
        [SerializeField] private int TIME_TO_PICK_UP_BEEPER_DEVIATION = 5; 
        [Space(5)]
        [SerializeField] private int TIME_TO_WAIT_FOR_TASK_BASE = 15; 
        [SerializeField] private int TIME_TO_WAIT_FOR_TASK_DEVIATION = 15;
        [Space(5)]
        [SerializeField] private int TIME_TO_WAIT_AFTER_BEEPER_FAIL = 2;
        [Space(5)]
        [SerializeField] private int MIN_TASK_INTERVAL = 7;
        [SerializeField] private int MAX_TASK_INTERVAL = 12;

        public int GET_TIME_TO_PICK_UP_BEEPER_BASE { get { return TIME_TO_PICK_UP_BEEPER_BASE; } }
        public int GET_TIME_TO_PICK_UP_BEEPER_DEVIATION { get { return TIME_TO_PICK_UP_BEEPER_DEVIATION; } }

        public int GET_TIME_TO_WAIT_FOR_TASK_BASE { get { return TIME_TO_WAIT_FOR_TASK_BASE; } }
        public int GET_TIME_TO_WAIT_FOR_TASK_DEVIATION { get { return TIME_TO_WAIT_FOR_TASK_DEVIATION; } }

        public int GET_TIME_TO_WAIT_AFTER_BEEPER_FAIL { get { return TIME_TO_WAIT_AFTER_BEEPER_FAIL; } }
        
        public int GET_MIN_TASK_INTERVAL {  get { return MIN_TASK_INTERVAL; } }
        public int GET_MAX_TASK_INTERVAL {  get { return MAX_TASK_INTERVAL; } }
        #endregion

        #region Scores
        [Header("Score")]
        [SerializeField] private int STARTING_SCORE = 1000; // 1000
        [Space(5)]
        [SerializeField] private int SCORE_CODE_CORRECT = 75; // 75
        [SerializeField] private int SCORE_CODE_INCORRECT = -75; // -75
        [Space(5)]
        [SerializeField] private int SCORE_INFUUS_LIQUID_CORRECT = 150; // 150
        [SerializeField] private int SCORE_INFUUS_LIQUID_INCORRECT = -150; // -150
        [SerializeField] private int SCORE_INFUUS_WEIGHT_CORRECT = 150;
        [SerializeField] private int SCORE_INFUUS_WEIGHT_INCORRECT = -150;
        [Space(5)]
        [SerializeField] private int SCORE_CASUS_BEST = 150; // 150
        [SerializeField] private int SCORE_CASUS_MEDIUM = 50; // 50
        [SerializeField] private int SCORE_CASUS_WORST = 150; // -150
        [Space(5)]
        [SerializeField] private int SCORE_BEEPER_PICKUP_FAILED = -50;
        [SerializeField] private int SCORE_BEEPER_PICKUP_SUCCESS = 50;
        [Space(5)]
        [SerializeField] private int SCORE_PATIENT_LOST_ON_HEALTH = -200;

        public int GET_STARTING_SCORE { get { return STARTING_SCORE; } }

        public int GET_SCORE_CODE_CORRECT { get { return SCORE_CODE_CORRECT; } }
        public int GET_SCORE_CODE_INCORRECT { get { return SCORE_CODE_INCORRECT; } }

        public int GET_SCORE_INFUUS_LIQUID_CORRECT { get { return SCORE_INFUUS_LIQUID_CORRECT; } }
        public int GET_SCORE_INFUUS_LIQUID_INCORRECT { get { return SCORE_INFUUS_LIQUID_INCORRECT; } }        
        public int GET_SCORE_INFUUS_WEIGHT_CORRECT { get { return SCORE_INFUUS_WEIGHT_CORRECT; } }
        public int GET_SCORE_INFUUS_WEIGHT_INCORRECT{ get { return SCORE_INFUUS_WEIGHT_INCORRECT; } }

        public int GET_SCORE_CASUS_BEST { get { return SCORE_CASUS_BEST; } }
        public int GET_SCORE_CASUS_MEDIUM { get { return SCORE_CASUS_MEDIUM; } }
        public int GET_SCORE_CASUS_WORST { get { return SCORE_CASUS_WORST; } }

        public int GET_SCORE_BEEPER_PICKUP_FAILED { get { return SCORE_BEEPER_PICKUP_FAILED; } }
        public int GET_SCORE_BEEPER_PICKUP_SUCCESS { get { return SCORE_BEEPER_PICKUP_SUCCESS; } }

        public int GET_SCORE_PATIENT_LOST_ON_HEALTH { get { return SCORE_PATIENT_LOST_ON_HEALTH; } }
        #endregion

        #region Patients
        [Header("Patients")]
        [SerializeField] private int PATIENT_BUFF_WHILE_PLAYER_IN_ROOM = 2; // 2
        [SerializeField] private int PATIENT_BASE_DIGRESSION = 3; // 3
        [Space(5)]
        [SerializeField] private int TIME_BETWEEN_PATIENTS_SPAWN_BASE = 15;
        [SerializeField] private int TIME_BETWEEN_PATIENTS_SPAWN_DEVIATION = 15;
        [Space(5)]
        [SerializeField] private int TIME_BETWEEN_PATIENTS_SPAWN_FIRST_BASE = 15;
        [SerializeField] private int TIME_BETWEEN_PATIENTS_SPAWN_FIRST_DEVIATION = 15;

        [SerializeField] private List<PATIENT_TYPE> PATIENT_TYPES_TO_EXIST = new List<PATIENT_TYPE>();// 3-7

        public int GET_PATIENT_BUFF_WHILE_PLAYER_IN_ROOM { get { return PATIENT_BUFF_WHILE_PLAYER_IN_ROOM; } }
        public int GET_PATIENT_BASE_DIGRESSION { get { return PATIENT_BASE_DIGRESSION; } }

        public int GET_TIME_BETWEEN_PATIENTS_SPAWN_BASE { get { return TIME_BETWEEN_PATIENTS_SPAWN_BASE; } }
        public int GET_TIME_BETWEEN_PATIENTS_SPAWN_DEVIATION { get { return TIME_BETWEEN_PATIENTS_SPAWN_DEVIATION; } }

        public int GET_TIME_BETWEEN_PATIENTS_SPAWN_FIRST { get { return TIME_BETWEEN_PATIENTS_SPAWN_FIRST_BASE; } }
        public int GET_TIME_BETWEEN_PATIENTS_SPAWN_FIRST_DEVIATION { get { return TIME_BETWEEN_PATIENTS_SPAWN_DEVIATION; } }
        public List<PATIENT_TYPE> GET_PATIENT_TYPES_TO_EXIST { get { return PATIENT_TYPES_TO_EXIST; } }
        #endregion

        #region Other
        [Header("Other")]
        [SerializeField] private bool WEIGHT_CALCULATION_SIMPLIFIED = false;
        [SerializeField] private int GLOBAL_TIMER = 600; // 600s
        [SerializeField] private float ELEMENT_DEBRIEF_SPAWN_INTERVAL = 0.1f;

        public bool IS_WEIGHT_CALCULATION_SIMPLIFIED { get { return WEIGHT_CALCULATION_SIMPLIFIED; } }
        public int GET_GLOBAL_TIMER { get {  return GLOBAL_TIMER; } }
        public float GET_ELEMENT_DEBRIEF_SPAWN_INTERVAL { get { return ELEMENT_DEBRIEF_SPAWN_INTERVAL; } }
        #endregion

        #region Player
        [Header("Player")]
        [SerializeField] private float PLAYER_SELFCARE_LOSS_RATE = 2.5f;
        [SerializeField] private float PLAYER_SELFCARE_GAIN_RATE = 10f;
        [SerializeField] private float PLAYER_SPEED = 5f;

        public float GET_PLAYER_SELFCARE_LOSS_RATE { get { return PLAYER_SELFCARE_LOSS_RATE; } }
        public float GET_PLAYER_SELFCARE_GAIN_RATE { get { return PLAYER_SELFCARE_GAIN_RATE; } }
        public float GET_PLAYER_SPEED { get { return PLAYER_SPEED; } }
        #endregion
    }
}
