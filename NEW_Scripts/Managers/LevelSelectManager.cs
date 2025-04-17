using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace New
{
    public class LevelSelectManager : MonoBehaviour // We can make this work with levels later
    {
        [SerializeField] private Button startGame_BTN;
        [SerializeField] private Button goBack_BTN;
        [SerializeField] private List<LevelSelectElements> levelElements = new List<LevelSelectElements>();
        public GameObject lines_OBJ;
        public GameObject lock_OBJ;

        private void Awake()
        {
            foreach(LevelSelectElements element in levelElements)
            {
                element.level_BTN.onClick.AddListener(() => OnLevelSelect(element, levelElements.ToArray()));
            }

            goBack_BTN.onClick.AddListener(() => SceneManager.LoadScene(0));

            if (PlayerStatsManager.instance != null)
            {
                if (PlayerStatsManager.instance.GetPlayerStats.HasUnlockedLevel2)
                {
                    levelElements[1].level_BTN.enabled = true;
                    levelElements[1].level_BTN.interactable = true;
                    lines_OBJ.SetActive(true);
                    lock_OBJ.SetActive(false);
                }
            }
        }
        
        private void OnLevelSelect(LevelSelectElements selectedElem, LevelSelectElements[] allElements)
        {
            foreach(LevelSelectElements element in allElements)
            {
                element.level_Popup_OBJ.SetActive(false);
            }

            selectedElem.level_Popup_OBJ.SetActive(true);
            startGame_BTN.gameObject.SetActive(true);

            if (selectedElem == allElements[0])
                startGame_BTN.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));

            else
                startGame_BTN.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2));
        }
    }


    [Serializable]
    public class LevelSelectElements
    {
        public Button level_BTN;
        public GameObject level_Popup_OBJ;
    }
}
