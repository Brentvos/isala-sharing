using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{

    public abstract class MovementElementRoot : MonoBehaviour
    {
        [SerializeField] protected MOVEMENT_TYPE movementType = MOVEMENT_TYPE.DRAGGING;

        protected Rigidbody2D rb;

        protected bool isWalking = false;
        private bool isDragging = false;
        private Vector3 offset;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

        }

        #region WASD
        protected virtual void Update()
        {
        }

        #endregion

        #region Dragging
        private void OnMouseDown()
        {
            if (movementType != MOVEMENT_TYPE.DRAGGING)
                return;

            isDragging = true;
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Cursor.visible = false;
        }
        protected virtual void OnMouseUp()
        {
            if (movementType != MOVEMENT_TYPE.DRAGGING)
                return;

            isDragging = false;
            Cursor.visible = true;
        }

        private void OnMouseDrag()
        {
            if (movementType != MOVEMENT_TYPE.DRAGGING)
                return;

            if (isDragging)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
                rb.MovePosition(new Vector2(mousePosition.x, mousePosition.y));
            }
        }
        #endregion
    }
}