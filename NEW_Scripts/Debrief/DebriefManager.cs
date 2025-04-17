using System.Collections.Generic;
using UnityEngine;

namespace New
{
    public class DebriefManager : MonoBehaviour
    {
        public static DebriefManager Instance;

        [SerializeField] private bool timeStampShowTimeLeft_TYPE;

        private List<Timestamp> timestamps = new List<Timestamp>();
        public List<Timestamp> GetTimeStamps { get { return timestamps; } }

        public int InitialScore;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitialScore = BalancingManager.Instance.GetData.GET_STARTING_SCORE;
        }

        public void ResetStats()
        {
            timestamps.Clear();
        }

        public void RegisterTimestamp(SCORE_TYPES scoreType, int scoreAmount, PatientRoot patient)
        {
            int timeOfRecord = 0;

            if (timeStampShowTimeLeft_TYPE)
            {
                timeOfRecord = Mathf.CeilToInt(GameManager.Instance.GetTimeLeft);
            }

            else
            {
                timeOfRecord = Utility_Functions.GetTimePassedFromTimer(
                BalancingManager.Instance.GetData.GET_GLOBAL_TIMER,
                GameManager.Instance.GetTimeLeft);
            }

            timestamps.Add(
                new Timestamp
                (
                    timeOfRecord,
                    patient.GetPatientInfo.GetNameWithTitle(),
                    scoreAmount,
                    scoreType
                // patient.GetPatientInfo.GetSprite
                ));
        }
    }
}
