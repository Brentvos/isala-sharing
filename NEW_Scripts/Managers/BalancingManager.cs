using UnityEngine;

namespace New
{
    public class BalancingManager : MonoBehaviour
    {
        public static BalancingManager Instance;

        [SerializeField] private BalancingSO data;
        public BalancingSO GetData { get { return data; } }

        private void Awake()
        {
            if (Instance != this)
                Instance = this;
        }
    }
}