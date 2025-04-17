using UnityEngine;

namespace OLD
{


    public class Wall : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<BoxCollider2D>().size = GetComponent<RectTransform>().sizeDelta;
        }
    }
}