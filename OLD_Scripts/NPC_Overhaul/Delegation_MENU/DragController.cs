using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OLD
{


    public class DragController : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        private bool isDragging = false;
        private GameObject nurseObjectBeingDragged;

        private Vector2 startingPos;

        public static DragController Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Update()
        {
            if (isDragging)
            {
                MoveElementToMouse();
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

                if (selectedObject != null)
                {
                    DraggableNurseElement draggableNurse = selectedObject.GetComponent<DraggableNurseElement>();

                    if (draggableNurse != null && draggableNurse.IsAvailable)
                    {
                        isDragging = true;
                        nurseObjectBeingDragged = draggableNurse.gameObject;

                        startingPos = nurseObjectBeingDragged.transform.position;
                        Debug.Log("Draggable nurse found: " + draggableNurse.name);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                EndDrag(DragCancelReason.MouseUp);
            }
        }

        public void EndDrag(DragCancelReason cancelReason, int nurseCode = 0,
            PatientInteractiveElement patientElement = null)
        {
            if (cancelReason == DragCancelReason.PatientCollision && nurseCode != 0)
            {
                if (patientElement.GetPatientData.GetState == PatientState.OpenForTreatment)
                {
                    patientElement.StartTreatment(nurseCode);
                }
                // Object being dragged might be put next to patient? 
                // objectBeingDragged.transform.position = patient.helperSlot?
            }

            else
            {
            }

            nurseObjectBeingDragged.transform.position = startingPos;

            isDragging = false;
            nurseObjectBeingDragged = null;
        }

        private void MoveElementToMouse()
        {
            // Get the RectTransform of the draggable element
            RectTransform parentRectTransform = nurseObjectBeingDragged.transform.
                parent.GetComponent<RectTransform>();

            RectTransform rectTransform = nurseObjectBeingDragged.GetComponent<RectTransform>();

            // Convert the mouse position to the canvas's local position
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out localPoint
            );

            // Set the position of the draggable element

            rectTransform.localPosition = localPoint;
        }
    }
}