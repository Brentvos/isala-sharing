using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace New
{
    public class HighscoreMenu : MonoBehaviour
    {
        public static HighscoreMenu instance;
        public Button backToMenu_BTN;

        [SerializeField] private GameObject highscoreElem_PARENT;
        [SerializeField] private GameObject highscoreElem_OBJ;

        [SerializeField] Sprite[] avatars;
        public Sprite[] GetAvatars { get { return  avatars; } }

        void Start()
        {
            if (instance != this)
                instance = this;

            backToMenu_BTN.onClick.AddListener(() => SceneManager.LoadSceneAsync(0));
            DisplayHighscores();
        }

        void DisplayHighscores()
        {
            List<PlayerStats> allStats = HighscoreManager.LoadAllPlayerStats();
            allStats.Sort((a, b) => b.GetFinalScore.CompareTo(a.GetFinalScore)); // Sort by high score descending

            foreach (var stats in allStats)
            {
                HighscoreElement elem = Instantiate(highscoreElem_OBJ, highscoreElem_PARENT.transform, false)
                    .GetComponent<HighscoreElement>();

                elem.Initialize(avatars[stats.GetAvatarIndex], "Dr. " + stats.GetPlayerName, stats.GetFinalScore);
            }
        }
    }
}