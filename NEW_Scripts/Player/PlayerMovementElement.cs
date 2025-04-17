using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace New
{

    public class PlayerMovementElement : MovementElementRoot
    {
        [SerializeField] private Transform playerDefaultIdleLocation_OBJ;
        [SerializeField] private GameObject hitEffect;

        private float speed;
        private RoomRoot currentRoom;

        private void Start()
        {
            speed = BalancingManager.Instance.GetData.GET_PLAYER_SPEED;
        }

        protected override void Update()
        {
            if (!Player.Instance.isMovementEnabled)
                return;

            base.Update();

            if (movementType == MOVEMENT_TYPE.DRAGGING)
                return;

            float moveX = 0;
            float moveY = 0;

            if (movementType == MOVEMENT_TYPE.WASDSNAPPY)
            {
                moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
                moveY = Input.GetAxisRaw("Vertical");   // W/S or Up/Down
            }

            else
            {
                moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
                moveY = Input.GetAxis("Vertical");   // W/S or Up/Down
            }

            Vector3 movement = new Vector3(moveX, moveY, 0f);

            if (movement.magnitude > 1)
                movement.Normalize();

            movement *= speed * Time.deltaTime;
            transform.position += movement;

            if (movement == Vector3.zero)
            {
                if (currentRoom != null)
                    transform.position = currentRoom.GetPlayerIdleLocation.position;
            }

            //if (Player.Instance.GetCurHealthValue <= 0)
            //    speed = BalancingManager.Instance.GetData.GET_PLAYER_SPEED - 2;

            //else
            //    speed = BalancingManager.Instance.GetData.GET_PLAYER_SPEED;

            return;
        }
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out RoomRoot room))
            {
                currentRoom = room;
                room.OnPlayerEnter();
            }

            if (other.GetComponent<Wall>() != null)
            {
                transform.position = playerDefaultIdleLocation_OBJ.position;
                StartCoroutine(HitEffect());
                OnMouseUp();
            }
        }

        private IEnumerator HitEffect()
        {
            hitEffect.SetActive(true);
            hitEffect.GetComponent<Image>().DOFade(0.2f, 0.15f);
            yield return new WaitForSeconds(0.15f);
            hitEffect.GetComponent<Image>().DOFade(0, 0f);
            hitEffect.SetActive(false);
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out RoomRoot room))
            {
                room.OnPlayerExit();
                currentRoom = null;
            }
        }

        protected override void OnMouseUp()
        {
            if (currentRoom != null)
                transform.position = currentRoom.GetPlayerIdleLocation.position;

            else
                transform.position = playerDefaultIdleLocation_OBJ.position;

            base.OnMouseUp();
        }
    }
}