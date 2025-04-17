using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace New
{
    public class DebriefMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI debriefScore_TXT;
        [SerializeField] private TextMeshProUGUI playerName_TXT;
        [SerializeField] private Image playerAvatar_IMG;
        [SerializeField] private GameObject timestampElement_PREFAB;
        [SerializeField] private GameObject timestampParent_OBJ;
        [SerializeField] private ContentSizeFitter contentSizeFitter;
        [SerializeField] private ScrollRect scrollRect;

        [SerializeField] private Button continueToMenu_BTN;

        [SerializeField] private Color redSoftColour;
        [SerializeField] private Color greenSoftColour;

        private int debriefScore;

        private void Awake()
        {
            continueToMenu_BTN.onClick.AddListener(() => FinishGame());
        }

        private void Start()
        {
            continueToMenu_BTN.enabled = false;

            if (PlayerStatsManager.instance != null)
            {
                playerName_TXT.text = "Dr. " + PlayerStatsManager.instance.GetPlayerStats.GetPlayerName;
                playerAvatar_IMG.sprite = PlayerStatsManager.instance.GetPlayerStats.GetUIAvatarZoomIn;
            }

            debriefScore = DebriefManager.Instance.InitialScore;
            StartCoroutine(PopulateDebriefCoroutine());
        }

        private IEnumerator PopulateDebriefCoroutine()
        {
            int curObj = 0;

            float originalSensitivity = scrollRect.scrollSensitivity;
            scrollRect.scrollSensitivity = 0;

            foreach (Timestamp timestamp in DebriefManager.Instance.GetTimeStamps)
            {
                curObj++;

                if (curObj == 11)
                    contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                TimestampElement elem = Instantiate(timestampElement_PREFAB, timestampParent_OBJ.transform, false)
                    .GetComponent<TimestampElement>();

                Color colourToAssign = Color.white;

                if (timestamp.IsScorePositive)
                    colourToAssign = greenSoftColour;

                else
                    colourToAssign = redSoftColour;

                elem.Initialize(timestamp.GetTimeOfRecord, timestamp.GetPatientNameTitle,
                    timestamp.GetScore, timestamp.IsScorePositive, timestamp.GetFeedbackToDisplay,
                    colourToAssign);

                elem.GetComponent<RectTransform>().DOScale(1,
                    BalancingManager.Instance.GetData.GET_ELEMENT_DEBRIEF_SPAWN_INTERVAL);

                UpdateTotalScore(timestamp.GetScore, timestamp.IsScorePositive);

                scrollRect.verticalNormalizedPosition = 0;

                debriefScore_TXT.transform.DOScale(1.5f, BalancingManager.Instance.GetData.GET_ELEMENT_DEBRIEF_SPAWN_INTERVAL * 0.4f);
                yield return new WaitForSeconds(BalancingManager.Instance.GetData.GET_ELEMENT_DEBRIEF_SPAWN_INTERVAL * 0.5f);
                debriefScore_TXT.transform.DOScale(1f, BalancingManager.Instance.GetData.GET_ELEMENT_DEBRIEF_SPAWN_INTERVAL * 0.4f);
                yield return new WaitForSeconds(BalancingManager.Instance.GetData.GET_ELEMENT_DEBRIEF_SPAWN_INTERVAL * 0.5f);
            }

            continueToMenu_BTN.enabled = true;
            scrollRect.scrollSensitivity = originalSensitivity;
            debriefScore_TXT.DOColor(new Color(0.349f, 0.298f, 0.239f), BalancingManager.Instance.GetData.GET_ELEMENT_DEBRIEF_SPAWN_INTERVAL * 0.8f);
        }

        private void UpdateTotalScore(int score, bool isPositive)
        {
            if (isPositive)
            {
                debriefScore += score;
                debriefScore_TXT.DOColor(greenSoftColour, BalancingManager.Instance.GetData.GET_ELEMENT_DEBRIEF_SPAWN_INTERVAL * 0.8f);
            }

            else
            {
                debriefScore_TXT.DOColor(redSoftColour, BalancingManager.Instance.GetData.GET_ELEMENT_DEBRIEF_SPAWN_INTERVAL * 0.8f);
                debriefScore += score;
            }

            debriefScore_TXT.text = debriefScore.ToString();
        }
        private void FinishGame()
        {
            if (PlayerStatsManager.instance != null)
            {
                PlayerStatsManager.instance.GetPlayerStats.SetFinalScore(
                debriefScore);

                HighscoreManager.SavePlayerStats(PlayerStatsManager.instance.GetPlayerStats);
                DebriefManager.Instance.ResetStats();
            }

            SceneManager.LoadSceneAsync(0);
        }
    }
}
