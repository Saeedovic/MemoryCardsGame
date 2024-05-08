using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class LeaderboardManager : MonoBehaviour
{
    [Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public float completionTime;
    }

    public List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    public int maxEntries = 10;
   public TestSaveLoad testSaveLoad;

    public Text[] leaderboardTexts;

    public static List<T> FromJsonList<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }

     public void Awake()
    {
        LoadLeaderboard(); // Load the leaderboard data when the script is initialized
    }

    public void Start()
    {
        Debug.Log("Loaded");
       
        LoadLeaderboard(); // Load the leaderboard data afterwards
       

        Debug.Log("Leaderboard entries count: " + leaderboardEntries.Count);
        Debug.Log("Leaderboard texts count: " + leaderboardTexts.Length);
        UpdateLeaderboardUI();
    }

    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboardEntries);
        Debug.Log("Saved leaderboard data: " + json);
        File.WriteAllText(Application.persistentDataPath + "/leaderboard.json", json);
    }

    public void LoadLeaderboard()
    {
        if (File.Exists(Application.persistentDataPath + "/leaderboard.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/leaderboard.json");
            Debug.Log("Loaded leaderboard data: " + json);

            try
            {
                leaderboardEntries = FromJsonList<LeaderboardEntry>(json);
                Debug.Log("Deserialized leaderboard data: " + JsonUtility.ToJson(leaderboardEntries));
            }
            catch (Exception e)
            {
                Debug.LogError("Error deserializing leaderboard data: " + e.Message);
            }
        }
    }
    public void AddEntry(string playerName, float completionTime)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry
        {
            playerName = playerName,
            completionTime = completionTime
        };

        leaderboardEntries.Add(newEntry);

        Debug.Log("Added entry: " + playerName + " with time: " + completionTime);

        SortLeaderboard();
        TrimLeaderboard();
        SaveLeaderboard();
        CheckSavedLeaderboardData();

        
        Debug.Log("Calling UpdateLeaderboardUI");

        UpdateLeaderboardUI(); // Update UI with new entry
    }


   
    public void SortLeaderboard()
    {
        leaderboardEntries.Sort((x, y) => x.completionTime.CompareTo(y.completionTime));
    }

   
    public void TrimLeaderboard()
    {
        while (leaderboardEntries.Count > maxEntries)
        {
            leaderboardEntries.RemoveAt(maxEntries);
        }
    }

  
    public void UpdateLeaderboardUI()
    {
        for (int i = 0; i < Mathf.Min(leaderboardEntries.Count, 10); i++)
        {
            leaderboardTexts[i].text = (i + 1) + ". " + leaderboardEntries[i].playerName + ": " + leaderboardEntries[i].completionTime.ToString("F2") + " seconds";

            Debug.Log("Leaderboard text: " + leaderboardTexts[i].text);
        }

        for (int i = leaderboardEntries.Count; i < 10; i++)
        {
            leaderboardTexts[i].text = "";
        }
        SaveLeaderboard();
    }
    public void DisplayLeaderboard()
    {
        SaveLeaderboard();
        UpdateLeaderboardUI();
    }

    public void CheckSavedLeaderboardData()
    {
        if (File.Exists(Application.persistentDataPath + "/leaderboard.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/leaderboard.json");
            Debug.Log("Saved leaderboard data: " + json);

            // Parse the JSON data and check if it matches the expected data
            List<LeaderboardEntry> savedLeaderboardEntries = JsonUtility.FromJson<List<LeaderboardEntry>>(json);
            bool dataMatches = true;
            for (int i = 0; i < savedLeaderboardEntries.Count && i < leaderboardEntries.Count; i++)
            {
                if (savedLeaderboardEntries[i].playerName != leaderboardEntries[i].playerName ||
                    savedLeaderboardEntries[i].completionTime != leaderboardEntries[i].completionTime)
                {
                    dataMatches = false;
                    break;
                }
            }

            if (dataMatches)
            {
                Debug.Log("Saved leaderboard data matches expected data.");
            }
            else
            {
                Debug.LogError("Saved leaderboard data does not match expected data.");
            }
        }
        else
        {
            Debug.LogError("Leaderboard.json file does not exist.");
        }
    }
}