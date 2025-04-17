using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace OLD
{
    [Serializable]
    public class Meter
    {
        [SerializeField] private Image progressionImage = default;
        // [SerializeField] private Image backgroundImage = default;

        [SerializeField] private float fillDuration = default;
        // [SerializeField] private Color progressionColour = default;
        // [SerializeField] private Color backgroundColour = default;
        // [SerializeField] private Color targetProgressionColour = default;

        [SerializeField] private bool hasGradient = false;
        [SerializeField] private Gradient progressionGradient;

        [SerializeField] private bool hasProgressionPrediction = false;
        [SerializeField] private Image targetProgressionImageUp = default;
        // [SerializeField] private Image targetProgressionImageDown = default;
        // [SerializeField] private Image targetProgressionImage = default;

        private float maxValue;
        private float actualValue;
        private float targetValue;

        public void Initialize(float maxValue, float currentValue)
        {
            this.maxValue = maxValue;
            this.actualValue = currentValue;

            UpdateMeterVisuals(currentValue, false);
        }

        private void Awake()
        {
            if (maxValue == 0)
                maxValue = 100;

            actualValue = maxValue;
            progressionImage.fillAmount = 1;

            //backgroundImage.color = backgroundColour;
            //progressionImage.color = progressionColour;

            // Optional
            // targetProgressionImage.color = targetProgressionColour;
        }

        public IEnumerator UpdateProgressionPrediction()
        {
            if (!hasProgressionPrediction)
                yield break;

            while (true)
            {
                if (progressionImage.fillAmount != targetValue)
                {
                    if (progressionImage.fillAmount < targetValue)
                    {
                        targetProgressionImageUp.fillAmount = targetValue;
                        targetProgressionImageUp.gameObject.SetActive(true);

                        // targetProgressionImageDown.gameObject.SetActive(false);
                    }

                    else
                    {
                        // targetProgressionImageDown.fillAmount = targetValue;
                        // targetProgressionImageDown.gameObject.SetActive(true);

                        targetProgressionImageUp.gameObject.SetActive(false);
                    }
                }

                else
                {
                    targetProgressionImageUp.gameObject.SetActive(false);
                }

                yield return new WaitForSeconds(.1f);
            }
        }

        public void UpdateMeterVisualsSimple(float value)
        {
            progressionImage.fillAmount = Mathf.Clamp01(value / maxValue);
        }

        public void UpdateMeterVisuals(float value, bool isGradual)
        {
            float targValue = Mathf.Clamp01(value / maxValue);

            if (isGradual)
                progressionImage.DOFillAmount(targValue, fillDuration);

            else
                progressionImage.DOFillAmount(targValue, 1);

            //else
            // progressionImage.fillAmount = targValue;

            if (hasGradient)
                progressionImage.color = progressionGradient.Evaluate(targValue);

            targetValue = targValue;
        }
    }
}