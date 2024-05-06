using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileIOManager
{
    private static readonly string filePath = Application.persistentDataPath + "/leaderboard.json";

    public static void SaveData(List<LeaderboardManager.LeaderboardEntry> data)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }

    public static List<LeaderboardManager.LeaderboardEntry> LoadData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<List<LeaderboardManager.LeaderboardEntry>>(jsonData);
        }
        else
        {
            Debug.LogWarning("Leaderboard file not found.");
            return new List<LeaderboardManager.LeaderboardEntry>();
        }
    }
}