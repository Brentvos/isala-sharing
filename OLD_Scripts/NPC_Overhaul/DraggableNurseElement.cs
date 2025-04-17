using UnityEngine;
using UnityEngine.UI;

namespace OLD
{
    public class DraggableNurseElement : MonoBehaviour
    {
        [SerializeField] private int nurseCode;
        public int GetNurseCode { get { return nurseCode; } }
        public bool IsAvailable = true;

        public void Initialize(NurseData nurseData)
        {
            this.nurseCode = nurseData.GetCode;
            nurseData.OnNurseStateChanged += NurseData_OnNurseStateChanged;
        }

        private void NurseData_OnNurseStateChanged(NurseState state)
        {
            if (state == NurseState.Available)
            {
                IsAvailable = true;
                GetComponent<Selectable>().interactable = true;
            }

            else
            {
                IsAvailable = false;
                GetComponent<Selectable>().interactable = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Wall>() != null)
                DragController.Instance.EndDrag(DragCancelReason.WallCollision);

            else if (collision.GetComponent<PatientInteractiveElement>() != null)
            {
                if (collision.GetComponent<PatientInteractiveElement>()
                    .GetPatientData.GetState == PatientState.OpenForTreatment)
                {
                    DragController.Instance.EndDrag(DragCancelReason.PatientCollision, nurseCode,
                        collision.GetComponent<PatientInteractiveElement>());
                }
            }
        }
    }
}