using DG.Tweening;
using UnityEngine;

namespace New
{
    public abstract class RoomRoot : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform playerIdleLocation_OBJ;

        [Header("Variables")]
        [SerializeField] private ROOM_TYPE roomType; // Unnecessary because we're using derived classes?

        private bool isPlayerInside = false;
        public bool IsPlayerInside { get { return isPlayerInside; } }

        public Transform GetPlayerIdleLocation { get { return playerIdleLocation_OBJ; } }
        public ROOM_TYPE GetRoomType { get { return roomType; } }

        public virtual void OnPlayerEnter()
        {
            GetComponent<SpriteRenderer>().DOColor(GameManager.Instance.GetGreenSoftColour, 1f);
            isPlayerInside = true;
            Debug.Log("Player entered room");
        }

        public virtual void OnPlayerExit()
        {
            GetComponent<SpriteRenderer>().DOColor(Color.white, 1f);
            isPlayerInside = false;
            Debug.Log("Player exited room");
        }

    }
}