using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace New
{
    public class TimestampElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeOfRecord_TXT;
        [SerializeField] private TextMeshProUGUI score_TXT;
        [SerializeField] private TextMeshProUGUI name_TXT;
        [SerializeField] private TextMeshProUGUI reason_TXT;

        [SerializeField] private Image background_IMG;

        //[SerializeField] private Image avatar;

        public void Initialize(int timeOfRecord, string name, int score, bool isPositive, string reason, Color backgroundColour)
        {
            name_TXT.text = name;
            background_IMG.color = backgroundColour;
            reason_TXT.text = reason;
            timeOfRecord_TXT.text = Utility_Functions.SecondsToPrintableFormat(timeOfRecord);

            if (isPositive)
                score_TXT.text = "+" + score.ToString();

            else
                score_TXT.text = score.ToString();
        }
    }
}