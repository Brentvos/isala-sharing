using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace New
{

    public class MainMenuBehaviour : MonoBehaviour
    {
        [SerializeField] private Button highscore_BTN;
        [SerializeField] private Button game_BTN;
        [SerializeField] private Button quit_BTN;

        private void Awake()
        {
            highscore_BTN.onClick.AddListener(() => SceneManager.LoadSceneAsync(1));
            game_BTN.onClick.AddListener(() => StartGame());
            quit_BTN.onClick.AddListener(() => Application.Quit());
        }

        private void StartGame()
        {
            if (PlayerStatsManager.instance == null)
            {
                SceneManager.LoadSceneAsync(2);
            }

            else
            {
                SceneManager.LoadSceneAsync(3);
            }
        }
    }
}