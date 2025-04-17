using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace New
{

    public class PlayerStatsManager : MonoBehaviour
    {
        public static PlayerStatsManager instance;
        
        private PlayerStats playerStats;
        public PlayerStats GetPlayerStats { get { return playerStats; } }

        //private Sprite[] avatarOptions;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); // Keep this object across scenes
            }

            else
            {
                Destroy(gameObject);
            }


        }
        private void Start()
        {
            //avatarOptions = CharacterCreator.instance.avatars;
        }

        public void InitializePlayer(string name, int index, Sprite uiAvatarZoomIn, Sprite uiAvatarZoomOut, Sprite fieldAvatar)
        {
            playerStats = new PlayerStats(name, index, uiAvatarZoomIn, uiAvatarZoomOut, fieldAvatar);
        }

        public void ResetStats()
        {
            playerStats = null;
        }
    }
}