using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OLD
{
    public class HelperProfileDisplayComponent : MonoBehaviour
    {
        [SerializeField] private Helper connectedHelper;

        [SerializeField] private TextMeshProUGUI nameDisplay;
        [SerializeField] private Image moraleStateDisplay;
        [SerializeField] private Image moraleStateEmptyDisplay;
        [SerializeField] private Image taskStateDisplay;
        [SerializeField] private Meter taskProgression;

        public void Initialize(Helper helper)
        {
            connectedHelper = helper;

            nameDisplay.text = helper.GetProfile.GetName.ToString();

            // Initialize Task stats?
        }

        private void OnMoraleStateChanged(MoraleState currentMoraleState)
        {
            switch (currentMoraleState)
            {
                case MoraleState.High:
                    moraleStateDisplay.color = Color.green;
                    moraleStateEmptyDisplay.gameObject.SetActive(false);
                    break;
                case MoraleState.Medium:
                    moraleStateDisplay.color = Color.yellow;
                    moraleStateEmptyDisplay.gameObject.SetActive(false);
                    break;
                case MoraleState.Low:
                    moraleStateDisplay.color = Color.red;
                    moraleStateEmptyDisplay.gameObject.SetActive(false);
                    break;
                case MoraleState.Empty:
                    moraleStateDisplay.color = Color.red;
                    moraleStateEmptyDisplay.gameObject.SetActive(true);
                    break;
            }
        }
    }
}