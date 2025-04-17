using New;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private TextMeshProUGUI tasksCorrect_TXT;
    [SerializeField] private Image tasksCorrect_IMG;

    [SerializeField] private TextMeshProUGUI tasksInCorrect_TXT;
    [SerializeField] private Image tasksIncorrect_IMG;

    [SerializeField] private Image background_IMG;

    private int tasksCorrect = 0;
    private int tasksIncorrect = 0;

    private void Awake()
    {
        if (instance != this)
            instance = this;
    }

    public void UpdateScore(SCORE_TYPES scoreType, PatientRoot patient)
    {
        int amount = 0;

        if (scoreType == SCORE_TYPES.NONE) // Might put this later, because of the feedback system
            return;

        switch (scoreType)
        {
            case SCORE_TYPES.CASUS_BEST:
                amount += BalancingManager.Instance.GetData.GET_SCORE_CASUS_BEST;
                UpdateCorrectCounter();
                break;
            case SCORE_TYPES.CODE_CORRECT:
                amount += BalancingManager.Instance.GetData.GET_SCORE_CODE_CORRECT;
                UpdateCorrectCounter();
                break;
            case SCORE_TYPES.INFUUS_LIQUID_CORRECT:
                amount += BalancingManager.Instance.GetData.GET_SCORE_INFUUS_LIQUID_CORRECT;
                UpdateCorrectCounter();
                break;
            case SCORE_TYPES.INFUUS_WEIGHT_CORRECT:
                amount += BalancingManager.Instance.GetData.GET_SCORE_INFUUS_WEIGHT_CORRECT;
                UpdateCorrectCounter();
                break;
            case SCORE_TYPES.BEEPER_PICKUP_SUCCESS:
                amount += BalancingManager.Instance.GetData.GET_SCORE_BEEPER_PICKUP_SUCCESS;
                UpdateCorrectCounter();
                break;

            case SCORE_TYPES.CASUS_MEDIUM:
                amount += BalancingManager.Instance.GetData.GET_SCORE_CASUS_MEDIUM;
                UpdateIncorrectCounter();
                break;
            case SCORE_TYPES.CASUS_WORST:
                amount += BalancingManager.Instance.GetData.GET_SCORE_CASUS_WORST;
                UpdateIncorrectCounter();
                break;
            case SCORE_TYPES.CODE_INCORRECT:
                amount += BalancingManager.Instance.GetData.GET_SCORE_CODE_INCORRECT;
                UpdateIncorrectCounter();
                break;
            case SCORE_TYPES.INFUUS_LIQUID_INCORRECT:
                amount += BalancingManager.Instance.GetData.GET_SCORE_INFUUS_LIQUID_INCORRECT;
                UpdateIncorrectCounter();
                break;
            case SCORE_TYPES.BEEPER_PICKUP_FAILED:
                amount += BalancingManager.Instance.GetData.GET_SCORE_BEEPER_PICKUP_FAILED;
                UpdateIncorrectCounter();
                break;
            case SCORE_TYPES.INFUUS_WEIGHT_INCORRECT:
                amount += BalancingManager.Instance.GetData.GET_SCORE_INFUUS_WEIGHT_INCORRECT;
                UpdateIncorrectCounter();
                break;
            case SCORE_TYPES.PATIENT_LOST_ON_HEALTH:
                amount += BalancingManager.Instance.GetData.GET_SCORE_PATIENT_LOST_ON_HEALTH;
                UpdateIncorrectCounter();
                break;
        }

        DebriefManager.Instance.RegisterTimestamp(scoreType, amount, patient);
    }

    private void UpdateIncorrectCounter() 
    {
        tasksIncorrect++;
        tasksInCorrect_TXT.text = tasksIncorrect.ToString();

        Sequence scaleColourSequence = DOTween.Sequence();

        scaleColourSequence.Append(background_IMG.DOColor(GameManager.Instance.GetRedSoftColour, 0.2f))
            .Append(background_IMG.DOColor(GameManager.Instance.GetRedSoftColour, 1))
            .Append(background_IMG.DOColor(Color.white, .2f));
    }

    private void UpdateCorrectCounter()
    {
        tasksCorrect++;
        tasksCorrect_TXT.text = tasksCorrect.ToString();

        Sequence scaleColourSequence = DOTween.Sequence();

        scaleColourSequence.Append(background_IMG.DOColor(GameManager.Instance.GetGreenSoftColour, 0.2f))
            .Append(background_IMG.DOColor(GameManager.Instance.GetGreenSoftColour, 1f))
            .Append(background_IMG.DOColor(Color.white, .2f));
    }
}
