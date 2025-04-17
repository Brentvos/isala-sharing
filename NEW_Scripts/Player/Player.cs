using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace New
{
    public class Player : MonoBehaviour
    {        
        public static Player Instance;
        public bool IsInCanteenRoom = false;
        public bool isMovementEnabled = true;

        [SerializeField] private int maxValue = 100;


        [SerializeField] private TextMeshProUGUI currentPercentage_TXT;
        [SerializeField] private Image background_IMG;
        [SerializeField] private Image emotion_IMG;
        [SerializeField] private Sprite[] emotionSprites;
        // [SerializeField] private Meter energyMeter;
       
        //[SerializeField] private TextMeshProUGUI playerName_TXT;
        //[SerializeField] private Image playerAvatar_IMG;
        [SerializeField] private SpriteRenderer playerMovementAvatar_RENDERER;
        [SerializeField] private Volume volume;
        private Vignette vignette;
        private FilmGrain filmGrain;
        private ColorAdjustments colorAdjustments;
        private ChromaticAberration chromaticAberration;


        [Header("Particles")]
        [SerializeField] private GameObject healthUpParticles;
        [SerializeField] private GameObject healthDownParticles;

        private float curValue;
        public float GetCurHealthValue { get { return curValue; } }

        public int GetMaxValue { get { return maxValue; } }
        public event Action<float> OnChangeValue;

        private void Awake()
        {
            curValue = maxValue;

            if (Instance != this)
                Instance = this;

            // energyMeter.Initialize(this);

            if (volume.profile.TryGet<Vignette>(out vignette))
            {
                vignette.intensity.overrideState = true;
            }

            if (volume.profile.TryGet<FilmGrain>(out filmGrain))
            {
                filmGrain.intensity.overrideState = true;
            }

            if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
            {
                chromaticAberration.intensity.overrideState = true;
            }

            if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            {
                colorAdjustments.saturation.overrideState = true;
            }


            if (PlayerStatsManager.instance != null)
            {
                //playerName_TXT.text = PlayerStatsManager.instance.GetPlayerStats.GetPlayerName;
               // playerAvatar_IMG.sprite = PlayerStatsManager.instance.GetPlayerStats.GetUIAvatar;
                playerMovementAvatar_RENDERER.sprite = PlayerStatsManager.instance.GetPlayerStats.GetFieldAvatar;
            }
        }

        private void Update()
        {
            if (BalancingManager.Instance == null)
                return;

            if (IsInCanteenRoom)
            {
                ChangeValue(BalancingManager.Instance.GetData.GET_PLAYER_SELFCARE_GAIN_RATE * Time.deltaTime);
                //healthUpParticles.SetActive(true);
                //healthDownParticles.SetActive(false);
            }

            else
            {
                ChangeValue(-BalancingManager.Instance.GetData.GET_PLAYER_SELFCARE_LOSS_RATE * Time.deltaTime);
                //healthUpParticles.SetActive(false);
                //healthDownParticles.SetActive(true);
            }

            vignette.intensity.value = Mathf.Clamp01(((maxValue / 2) - curValue) / (maxValue / 2));
            filmGrain.intensity.value = Mathf.Clamp01(((maxValue / 2) - curValue) / (maxValue / 2) * 0.5f);
            chromaticAberration.intensity.value = Mathf.Clamp01(((maxValue / 2) - curValue) / (maxValue / 2) * 0.15f);

            colorAdjustments.saturation.value = Mathf.Lerp(0f, -50f, 1 - (curValue / (maxValue * 0.5f)));
        }

        private void ChangeValue(float amount)
        {
            curValue = Mathf.Clamp(curValue += amount, 0, maxValue);
            currentPercentage_TXT.text = Mathf.CeilToInt(curValue).ToString() + "%";

            OnChangeValue?.Invoke(curValue);

            Sprite oldEmotion = emotion_IMG.sprite;
            Color colorToApply = Color.white;

            if (curValue > 66)
            {
                emotion_IMG.sprite = emotionSprites[0];
                colorToApply = GameManager.Instance.GetGreenSoftColour;
            }

            else if (curValue > 33 && curValue <= 66)
            {
                emotion_IMG.sprite = emotionSprites[1];
                colorToApply = GameManager.Instance.GetYellowSoftColour;
            }

            else if (curValue <= 33)
            {
                emotion_IMG.sprite = emotionSprites[2];
                colorToApply = GameManager.Instance.GetRedSoftColour;
            }

            if (oldEmotion != emotion_IMG.sprite)
            {
                Sequence colourSeq = DOTween.Sequence();

                if (!isColourAnimPlaying)
                {
                    colourSeq.Append(background_IMG.DOColor(colorToApply, .2f))
                        .Append(background_IMG.DOColor(colorToApply, 1f))
                        .Append(background_IMG.DOColor(Color.white, .2f))
                        .OnComplete(() => isColourAnimPlaying = false);

                    isColourAnimPlaying = true;
                }
            }
        }

          private bool isColourAnimPlaying = false;
    }
}
