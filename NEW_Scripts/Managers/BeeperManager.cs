using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace New
{
    public class BeeperManager : MonoBehaviour
    {
        public static BeeperManager Instance;

        [Header("Components")]
        [SerializeField] private Button beeperMainGame_BTN;
        [SerializeField] private Image beeperMainGame_IMG;
        [SerializeField] private TextMeshProUGUI beeperCounter_TXT;
        [SerializeField] private Image beeperTimer;
        [SerializeField] private Image beeperTimerImg2;
        [SerializeField] private ScaleOnHover scaler;

        [Header("Variables")]
        // [SerializeField] private float timeToPickup; // Moved to BALANCING, can add a randomizer afterewards though
        [SerializeField] private Sprite beeperOff_SPR;
        [SerializeField] private Sprite beeperOn_SPR;
        [SerializeField] private Sprite beeperCalling_SPR;

        private float queueInterval;
        private float currentQueueIntervalTimer = 0;

        [Header("Dialogue")]
        [SerializeField] private GameObject dialogueParent_OBJ;
        [SerializeField] private GameObject dialoguePopup_PREFAB;

        private GameObject dialoguePopup_OBJ;

        private bool isPatientOnBeeper;
        private bool isPatientOnCall;
        private bool isWaitingAfterFail = false;
        private float timeSinceBeeperStarted;
        private List<PatientRoot> patientsQueued = new List<PatientRoot>();
        private PatientRoot currentPatient;
        public PatientRoot GetCurrentPatient { get { return currentPatient; } }
        public bool IsPatientOnCall { get { return isPatientOnCall; } }

        private void Awake()
        {
            if (Instance != this)
                Instance = this;
        }

        private void Start()
        {
            PatientSpawningManager.Instance.OnPatientDataComponentCreated += OnAddTaskToBeeper;
            currentQueueIntervalTimer = 0;
        }

        private void Update()
        {
            BeeperTimer();
            UpdateCounterUI();

            if (!isPatientOnCall)
                currentQueueIntervalTimer -= Time.deltaTime;
        }

        private void BeeperTimer()
        {
            if (!isPatientOnCall && !isPatientOnBeeper) // Disable sprite when no patients in call or on beeper
            {
                beeperMainGame_IMG.sprite = beeperOff_SPR;

                //if (patientsQueued.Count > 0 && currentPatient == null)
            }

            else if (timeSinceBeeperStarted < BalancingManager.Instance.GetData.GET_TIME_TO_PICK_UP_BEEPER_BASE 
                && isPatientOnBeeper && !isPatientOnCall) // Update stuff while patient on beeper and time left
            {
                beeperMainGame_IMG.sprite = beeperCalling_SPR;
                beeperTimer.gameObject.SetActive(true);
                beeperTimerImg2.gameObject.SetActive(true);
                beeperTimer.fillAmount = timeSinceBeeperStarted / BalancingManager.Instance.GetData.GET_TIME_TO_PICK_UP_BEEPER_BASE;
                timeSinceBeeperStarted += Time.deltaTime;
            }

            else if (isPatientOnCall || timeSinceBeeperStarted >= BalancingManager.Instance.GetData.GET_TIME_TO_PICK_UP_BEEPER_BASE)
            {
                beeperTimer.gameObject.SetActive(false);
                beeperTimerImg2.gameObject.SetActive(false);
                beeperMainGame_IMG.sprite = beeperOn_SPR;

                if (timeSinceBeeperStarted >= BalancingManager.Instance.GetData.GET_TIME_TO_PICK_UP_BEEPER_BASE)
                    OnBeeperPickupFailed();
            }

            beeperMainGame_IMG.SetNativeSize();
            beeperMainGame_IMG.transform.localScale = new Vector3(0.38f, 0.38f, 1f);
        }

        public void OnAddTaskToBeeper(PatientRoot patient)
        {
            patientsQueued.Add(patient);
            StartCoroutine(WaitForBeeperInterval(false));
        }
        
        private void ManageNextBeeperInteraction(bool isFromNotebook, bool isCurrentlyWaiting = false)
        {
            if (patientsQueued.Count > 0 && currentPatient == null && !isPatientOnCall)
            {
                currentPatient = patientsQueued[0];

                if (isFromNotebook && !isCurrentlyWaiting)
                    return;

                beeperMainGame_BTN.onClick.AddListener(() => OnTriggerTreatment(currentPatient));
                isPatientOnBeeper = true;

                AnimationANDSoundManager.instance.Animate(
                    AnimationANDSoundManager.instance.GetBeeperActivate_ANIM, true, 2f);

                AnimationANDSoundManager.instance.Sound(
                    AnimationANDSoundManager.instance.GetBeeperActivate_AUDIOSOURCE, true);
            }
        }
        
        private IEnumerator WaitForBeeperInterval(bool isFromNotebook, bool isCurrentlyWaiting = false)
        {
            yield return new WaitUntil(() => currentQueueIntervalTimer <= 0);
            ManageNextBeeperInteraction(isFromNotebook, isCurrentlyWaiting);
        }

        /*
        private IEnumerator ManageNextBeeperInteraction(bool isFromNotebook, bool isCurrentlyWaiting = false)
        {
            if (patientsQueued.Count > 0 && currentPatient == null && !isPatientOnCall)
            {
                currentPatient = patientsQueued[0];

                if (isFromNotebook && !isCurrentlyWaiting)
                    return;

                beeperMainGame_BTN.onClick.AddListener(() => OnTriggerTreatment(currentPatient));
                isPatientOnBeeper = true;

                AnimationANDSoundManager.instance.Animate(
                    AnimationANDSoundManager.instance.GetBeeperActivate_ANIM, true, 2f);

                AnimationANDSoundManager.instance.Sound(
                    AnimationANDSoundManager.instance.GetBeeperActivate_AUDIOSOURCE, true);
            }
        }
        */

        private void OnBeeperPickupFailed()
        {
            StartCoroutine(WaitForBeeperAfterFail());
            // OnTriggerTreatment(currentPatient);
        }

        private IEnumerator WaitForBeeperAfterFail()
        {
            isWaitingAfterFail = true;
            beeperMainGame_IMG.sprite = beeperOff_SPR;
            ScoreManager.instance.UpdateScore(SCORE_TYPES.BEEPER_PICKUP_FAILED, currentPatient);

            beeperMainGame_BTN.onClick.RemoveAllListeners();
            isPatientOnBeeper = false;
            currentPatient = null;
            timeSinceBeeperStarted = 0;


            AnimationANDSoundManager.instance.Animate(
                AnimationANDSoundManager.instance.GetBeeperActivate_ANIM, false, 2f);

            AnimationANDSoundManager.instance.ResetRect(
                beeperMainGame_IMG.rectTransform, true);

            AnimationANDSoundManager.instance.Sound(
                AnimationANDSoundManager.instance.GetBeeperActivate_AUDIOSOURCE, false);

            yield return new WaitForSeconds(BalancingManager.Instance.GetData.GET_TIME_TO_WAIT_AFTER_BEEPER_FAIL);
            isWaitingAfterFail = false;
            StartCoroutine(WaitForBeeperInterval(false));
        }

        private void OnTriggerTreatment(PatientRoot patient, bool isFromNotebook = false)
        {
            // Reset stuff
            if (!isFromNotebook)
                beeperMainGame_BTN.onClick.RemoveAllListeners();

            currentQueueIntervalTimer = UnityEngine.Random.Range(BalancingManager.Instance.GetData.GET_MIN_TASK_INTERVAL,
                BalancingManager.Instance.GetData.GET_MAX_TASK_INTERVAL);

            isPatientOnBeeper = false;
            timeSinceBeeperStarted = 0;

            AnimationANDSoundManager.instance.Animate(
                AnimationANDSoundManager.instance.GetBeeperActivate_ANIM, false, 2f);

            AnimationANDSoundManager.instance.Sound(
                AnimationANDSoundManager.instance.GetBeeperActivate_AUDIOSOURCE, false);

            AnimationANDSoundManager.instance.ResetRect(
                beeperMainGame_IMG.rectTransform, true);

            ScoreManager.instance.UpdateScore(SCORE_TYPES.BEEPER_PICKUP_SUCCESS, patient);
            isPatientOnCall = true;

            TreatmentPopupRoot treatmentPopup = Instantiate(dialoguePopup_PREFAB, dialogueParent_OBJ.transform, false)
                .GetComponent<TreatmentPopupRoot>();

            dialoguePopup_OBJ = treatmentPopup.gameObject;
            treatmentPopup.Initialize(patient, false, isWaitingAfterFail);
            Player.Instance.isMovementEnabled = false;
        }

        public void PatientRemovedFromGame(PatientRoot patient)
        {
            patientsQueued.Remove(patient);

            if (patient == currentPatient)
            {
                currentPatient = null;

                beeperMainGame_BTN.onClick.RemoveAllListeners();
                isPatientOnBeeper = false;
                timeSinceBeeperStarted = 0;

                AnimationANDSoundManager.instance.Animate(
                    AnimationANDSoundManager.instance.GetBeeperActivate_ANIM, false, 2f);

                AnimationANDSoundManager.instance.Sound(
                    AnimationANDSoundManager.instance.GetBeeperActivate_AUDIOSOURCE, false);

                AnimationANDSoundManager.instance.ResetRect(
                    beeperMainGame_IMG.rectTransform, true);
            }
        }

        public void OnClickPatientTaskInNotebook(PatientRoot patient, Button taskButton) // WILL become problematic with other tasks
        {

            taskButton.onClick.RemoveAllListeners();

            
            isPatientOnCall = true;
            taskButton.interactable = false;
            taskButton.onClick.RemoveAllListeners();

            TreatmentPopupRoot treatmentPopup = Instantiate(dialoguePopup_PREFAB, dialogueParent_OBJ.transform, false)
                .GetComponent<TreatmentPopupRoot>();

            dialoguePopup_OBJ = treatmentPopup.gameObject;
            treatmentPopup.Initialize(patient, true, isWaitingAfterFail);
          
        }

        public void ProcessPatient(PatientRoot patient, bool isTaskDelayed, bool isFromNotebook, bool isCurrentlyWaiting)  // Add score (maybe somewhere else)
        {
            dialoguePopup_OBJ.GetComponent<TreatmentPopupRoot>().Destroy();
            dialoguePopup_OBJ = null;
            Player.Instance.isMovementEnabled = true;
            isPatientOnCall = false;
            patientsQueued.Remove(patient);
            currentPatient = null;
            patient.ProceedTreatment(isTaskDelayed);
            StartCoroutine(WaitForBeeperInterval(isFromNotebook, isCurrentlyWaiting));
        }

        private void UpdateCounterUI()
        {
            beeperCounter_TXT.text = patientsQueued.Count.ToString();

            if (patientsQueued.Count <= 0 || isPatientOnCall || isWaitingAfterFail)
            {
                beeperMainGame_BTN.interactable = false;
                scaler.enabled = false;
                scaler.IncreaseScale(false);
            }

            else
            {
                beeperMainGame_BTN.interactable = true;
                scaler.enabled = true;
            }
        }
    }
}
