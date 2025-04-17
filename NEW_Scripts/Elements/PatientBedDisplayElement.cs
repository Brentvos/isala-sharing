using DG.Tweening;
using TMPro;
using UnityEngine;

namespace New
{
    public class PatientBedDisplayElement : MonoBehaviour
    {
        [SerializeField] private Transform healthParent;
        [SerializeField] private TextMeshProUGUI nameText_TXT;
        [SerializeField] private Meter clinicalStatusMeter;
        [SerializeField] private SpriteRenderer bedSleepRenderer;

        [Header("Status Effects")]
        [SerializeField] protected GameObject clinicalStatusDownVFX;
        [SerializeField] protected GameObject clinicalStatusUpVFX;
        [SerializeField] protected GameObject requiringHelpLightVFX;
        [SerializeField] private GameObject patientHeart;

        private ClinicalStatusInfo clinicalStatusInfo;
        private bool resetAnim = true;

        public Meter GetClinicalStatusMeter { get { return clinicalStatusMeter; } }


        public void LightActivation(bool enable)
        {
            requiringHelpLightVFX.SetActive(enable);
        }

        public void Initialize(string name, ClinicalStatusInfo statusInfo, bool flipped)
        {
            nameText_TXT.text = name;
            clinicalStatusInfo = statusInfo;
            clinicalStatusMeter.Initialize(statusInfo);
            bedSleepRenderer.sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;

            clinicalStatusInfo.OnChangeValue += ClinicalStatusInfo_OnChangeValue;

            if (flipped)
            {
                bedSleepRenderer.flipX = flipped;
                healthParent.gameObject.GetComponent<RectTransform>().localPosition = 
                    new Vector3(-92, healthParent.gameObject.GetComponent<RectTransform>().localPosition.y);
            }
        }

        private void ClinicalStatusInfo_OnChangeValue(float amount)
        {
            if (resetAnim)
            {
                Sequence mySequence = DOTween.Sequence();

                if (amount > 75)
                {
                    mySequence.Append(patientHeart.transform.DOScale(1.25f, 1f))
                        .Append(patientHeart.transform.DOScale(1.25f, 1f))
                        .OnComplete(() => resetAnim = true);
                }

                else if (amount > 33 && amount <= 75)
                {
                    mySequence.Append(patientHeart.transform.DOScale(1.25f, 0.5f))
                        .Append(patientHeart.transform.DOScale(1f, 0.5f))
                        .OnComplete(() => resetAnim = true);
                }

                else if (amount >= 10 && amount <= 33)
                {
                    mySequence.Append(patientHeart.transform.DOScale(1.35f, 0.33f))
                        .Append(patientHeart.transform.DOScale(1f, 0.33f))
                        .OnComplete(() => resetAnim = true);
                }

                else if (amount < 10)
                {
                    mySequence.Append(patientHeart.transform.DOScale(1.5f, 0.2f))
                        .Append(patientHeart.transform.DOScale(1f, 0.2f))
                        .OnComplete(() => resetAnim = true);
                }


                resetAnim = false;
            }
        }

        private void Update()
        {
            if (clinicalStatusInfo != null)
            {
                if (clinicalStatusInfo.IsDegressionOverridden)
                {
                    if (!clinicalStatusUpVFX.activeSelf &&
                        clinicalStatusUpVFX.GetComponentInChildren<ParticleSystem>() != null)
                        clinicalStatusUpVFX.GetComponentInChildren<ParticleSystem>().Clear();

                    clinicalStatusUpVFX.SetActive(true);
                    clinicalStatusDownVFX.SetActive(false);
                }

                else
                {
                    if (!clinicalStatusDownVFX.activeSelf &&
                        clinicalStatusDownVFX.GetComponentInChildren<ParticleSystem>() != null)
                        clinicalStatusDownVFX.GetComponentInChildren<ParticleSystem>().Clear();

                    clinicalStatusDownVFX.SetActive(true);
                    clinicalStatusUpVFX.SetActive(false);
                }
            }
        }
    }
}