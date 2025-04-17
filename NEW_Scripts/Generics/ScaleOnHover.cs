using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace New
{
    public class ScaleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float scaleIncrease = 1.1f;
        [SerializeField] private float scaleDuration = 0.15f;

        private Vector3 initialScale;

        private void Awake()
        {
            initialScale = transform.localScale;
        }
        public void IncreaseScale(bool status)
        {
            Vector3 finalScale = initialScale;

            if (status)
                finalScale = initialScale * scaleIncrease;

            transform.DOScale(finalScale, scaleDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IncreaseScale(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (GetComponent<Button>() != null)
            {
                if (GetComponent<Button>().interactable != false && GetComponent<Button>().enabled != false)
                    IncreaseScale(true);

                return;
            }

            IncreaseScale(true);
        }
    }
}
