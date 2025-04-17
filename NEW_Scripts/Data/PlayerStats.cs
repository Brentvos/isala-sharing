using System;
using UnityEngine;

namespace New
{

    [Serializable]
    public class PlayerStats
    {
        [SerializeField] private string playerName;
        [SerializeField] private int avatarIndex;
        [SerializeField] private int finalScore;
        [SerializeField] private bool hasUnlockedLevel2;

        private Sprite uiAvatarZoomIn;
        private Sprite uiAvatarZoomOut;
        private Sprite fieldAvatar;

        public string GetPlayerName { get { return playerName; } }
        public int GetAvatarIndex { get { return avatarIndex; } }
        public Sprite GetUIAvatarZoomIn { get { return uiAvatarZoomIn; } }
        public Sprite GetUIAvatarZoomOut { get { return uiAvatarZoomOut; } }
        public Sprite GetFieldAvatar { get { return fieldAvatar; } }
        public int GetFinalScore { get { return finalScore; } }
        public bool HasUnlockedLevel2 { get { return hasUnlockedLevel2; } }


        public PlayerStats(string name, int avatarIndex, Sprite uiAvatarZoomIn, Sprite uiAvatarZoomOut, Sprite fieldAvatar, int score = 0)
        {
            playerName = name;
            this.uiAvatarZoomIn = uiAvatarZoomIn;
            this.uiAvatarZoomOut = uiAvatarZoomOut;
            this.fieldAvatar = fieldAvatar;
            this.avatarIndex = avatarIndex;
            finalScore = score;
        }

        public void SetFinalScore(int score)
        {
            finalScore = score;
            hasUnlockedLevel2 = true;
        }
    }
}