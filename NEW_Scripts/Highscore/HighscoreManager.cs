using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace New
{
    public static class HighscoreManager
    {
        private static string filePath = Application.persistentDataPath + "/highscores.json";

        public static void SavePlayerStats(PlayerStats stats)
        {
            List<PlayerStats> allStats = LoadAllPlayerStats();

            PlayerStats existingPlayer = allStats.Find(p => p.GetPlayerName == stats.GetPlayerName);
            allStats.Add(stats);

            // Save to file
            File.WriteAllText(filePath, JsonUtility.ToJson(new PlayerStatsWrapper(allStats), true));
        }

        public static List<PlayerStats> LoadAllPlayerStats()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<PlayerStatsWrapper>(json)?.playerStats ?? new List<PlayerStats>();
            }

           return new List<PlayerStats>();
        }

        // Wrapper class for JSON serialization
        [System.Serializable]
        private class PlayerStatsWrapper
        {
            public List<PlayerStats> playerStats;

            public PlayerStatsWrapper(List<PlayerStats> stats)
            {
                playerStats = stats;
            }
        }
    }
}