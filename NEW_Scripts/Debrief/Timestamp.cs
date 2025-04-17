
using UnityEngine;

namespace New
{
    public class Timestamp
    {
        private int timeOfRecord;
       // private Sprite patientSprite;
        private string patientNameTitle;
        private int score;
        private bool isScorePositive;
        private SCORE_TYPES scoreType;

        public Timestamp(int timeOfRecord, string patientNameTitle,
            /*Sprite patientSprite*/ int score, SCORE_TYPES scoreType)
        {
            this.timeOfRecord = timeOfRecord;
            this.patientNameTitle = patientNameTitle;
            this.score = score;
            this.scoreType = scoreType;

            if (score > 0)
                isScorePositive = true;
            else
                isScorePositive = false;
            //this.patientSprite = patientSprite;
        }

        public int GetTimeOfRecord { get { return timeOfRecord; } }
        public string GetPatientNameTitle { get { return patientNameTitle; } }
        public int GetScore { get { return score; } }
        public bool IsScorePositive { get { return isScorePositive; } }

        //public Sprite GetPatientSprite { get { return patientSprite; } }

        public string GetFeedbackToDisplay
        {
            get

            {
                string feedback = string.Empty;

                switch (scoreType)
                {
                    case SCORE_TYPES.CASUS_BEST:
                        feedback = "Casus goed beantwoord.";
                        break;
                    case SCORE_TYPES.CASUS_MEDIUM:
                        feedback = "Casus matig beantwoord.";
                        break;
                    case SCORE_TYPES.CASUS_WORST:
                        feedback = "Casus verkeerd beantwoord.";
                        break;
                    case SCORE_TYPES.CODE_INCORRECT:
                        feedback = "Patienten-nummer incorrect.";
                        break;
                    case SCORE_TYPES.CODE_CORRECT:
                        feedback = "Patienten-nummer correct.";
                        break;
                    case SCORE_TYPES.INFUUS_LIQUID_CORRECT:
                        feedback = "Infuus correct.";
                        break;
                    case SCORE_TYPES.INFUUS_LIQUID_INCORRECT:
                        feedback = "Infuus incorrect.";
                        break;
                    case SCORE_TYPES.BEEPER_PICKUP_FAILED:
                        feedback = "Pieper niet opgenomen.";
                        break;
                    case SCORE_TYPES.BEEPER_PICKUP_SUCCESS:
                        feedback = "Pieper op tijd opgenomen.";
                        break;
                    case SCORE_TYPES.INFUUS_WEIGHT_CORRECT:
                        feedback = "Gewicht correct.";
                        break;
                    case SCORE_TYPES.INFUUS_WEIGHT_INCORRECT:
                        feedback = "Gewicht incorrect.";
                        break;
                    case SCORE_TYPES.PATIENT_LOST_ON_HEALTH:
                        feedback = "Patient is overgenomen.";
                        break;
                }

                return feedback;
            }
        }
    }
}