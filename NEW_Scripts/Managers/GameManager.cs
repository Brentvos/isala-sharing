using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace New

{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timer_TXT;

        [SerializeField] private Button notebookOpener;
        [SerializeField] private Button notebookCloser;
        [SerializeField] private GameObject notebook_OBJ;


        [SerializeField] private Sprite[] patientCloseUpSprites;
        [SerializeField] private Sprite[] patientZoomOutSprites;

        [SerializeField] private Color redSoftColour;
        [SerializeField] private Color yellowSoftColour;
        [SerializeField] private Color greenSoftColour;

        [Header("Settings")]
        [SerializeField] private Button settingsMenu_BTN;
        [SerializeField] private Button backgroundSFX_BTN;
        [SerializeField] private Button SFX_BTN;
        [SerializeField] private Image playerAvatar_IMG;
        [SerializeField] private TextMeshProUGUI playerName_TXT;

        public event Action SoundButtonPressed;

        public bool backgroundSFXEnabled = true;
        public bool SFXEnabled = true;
        
        public Color GetRedSoftColour { get { return redSoftColour; } }
        public Color GetYellowSoftColour { get { return yellowSoftColour; } }
        public Color GetGreenSoftColour { get { return greenSoftColour; } }

        private bool isTimerDone = false;
        private bool timerDoneMessageSent = false;
        private float timeLeft;       

        public static GameManager Instance;
        public float GetTimeLeft { get { return timeLeft; } }
        public Sprite[] GetPatientCloseUpSprites { get { return patientCloseUpSprites; } }
        public Sprite[] GetPatientZoomOutSprites { get { return patientZoomOutSprites; } }

        private void Awake()
        {
            if (Instance != this)
                Instance = this;

            notebookOpener.onClick.AddListener(() => SetNotebookState(true));
            notebookCloser.onClick.AddListener(() => SetNotebookState(false));

            backgroundSFX_BTN.onClick.AddListener(() => SFXStateChange(true));
            SFX_BTN.onClick.AddListener(() => SFXStateChange(false));
            settingsMenu_BTN.onClick.AddListener(() => SceneManager.LoadSceneAsync(0));

            backgroundSFXEnabled = true;
            SFXEnabled = true;

            foreach (Image img in notebook_OBJ.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0f);
            }

            foreach (TextMeshProUGUI txt in notebook_OBJ.GetComponentsInChildren<TextMeshProUGUI>())
            {
                txt.DOFade(0, 0f);
            }

            notebook_OBJ.transform.DOScale(0.1f, 0f);
        }

        private void SFXStateChange(bool onlyBackground)
        {
            if (onlyBackground)
            {
                backgroundSFXEnabled = !backgroundSFXEnabled;

                if (!backgroundSFXEnabled)
                    backgroundSFX_BTN.GetComponentInChildren<TextMeshProUGUI>().text = "UIT";

                else
                    backgroundSFX_BTN.GetComponentInChildren<TextMeshProUGUI>().text = "AAN";
            }

            else
            {
                SFXEnabled = !SFXEnabled;

                if (!SFXEnabled)
                    SFX_BTN.GetComponentInChildren<TextMeshProUGUI>().text = "UIT";

                else
                    SFX_BTN.GetComponentInChildren<TextMeshProUGUI>().text = "AAN";
            }

            SoundButtonPressed?.Invoke();
        }

        private void Start()
        {
            timeLeft = BalancingManager.Instance.GetData.GET_GLOBAL_TIMER;

            if (PlayerStatsManager.instance == null)
                return;

            playerAvatar_IMG.sprite = PlayerStatsManager.instance.GetPlayerStats.GetUIAvatarZoomOut;
            playerName_TXT.text = "Dr. " + PlayerStatsManager.instance.GetPlayerStats.GetPlayerName;
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Alpha1)) // For prototyping
            //{
            //    timeLeft = 5;
            //}

            if (timeLeft <= 0)
                isTimerDone = true;

            if (!isTimerDone)
            {
                timeLeft -= Time.deltaTime;
                timer_TXT.text = Utility_Functions.SecondsToPrintableFormat((int)timeLeft);
            }

            else
            {
                if (timerDoneMessageSent)
                    return;

                timer_TXT.text = "00:00";
                SceneManager.LoadSceneAsync(SceneManager.sceneCountInBuildSettings - 1);
                timerDoneMessageSent = true;
                
            }
        }

        private void SetNotebookState(bool enable)
        {
            notebookOpener.interactable = !enable;
            Player.Instance.isMovementEnabled = !enable;

            if (enable)
            {
                notebook_OBJ.SetActive(enable);

                foreach (Image img in notebook_OBJ.GetComponentsInChildren<Image>())
                {
                    Sequence seq = DOTween.Sequence();

                    seq.Append(img.DOFade(1, 0.25f));
                }

                foreach (TextMeshProUGUI txt in notebook_OBJ.GetComponentsInChildren<TextMeshProUGUI>())
                {
                    Sequence seq = DOTween.Sequence();

                    seq.Append(txt.DOFade(1, 0.25f));
                }

                notebook_OBJ.transform.DOScale(1, 0.25f);
            }

            else
            {
                CloseNotebook();
                //foreach (Image img in notebook_OBJ.GetComponentsInChildren<Image>())
                //{
                //    img.DOFade(0f, 0.25f);
                //}

                //foreach (TextMeshProUGUI txt in notebook_OBJ.GetComponentsInChildren<TextMeshProUGUI>())
                //{
                //    txt.DOFade(0, 0.25f);
                //}

                //notebook_OBJ.transform.DOScale(0.1f, 0.25f).OnComplete(CloseNotebook); 
            }
        }

        private void CloseNotebook()
        {
            notebook_OBJ.SetActive(false);
        }
    }
}