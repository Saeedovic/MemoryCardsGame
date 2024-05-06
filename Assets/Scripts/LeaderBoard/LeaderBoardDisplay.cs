/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LeaderboardDisplay : MonoBehaviour
{
    public Text[] leaderboardTexts;
    public static LeaderboardDisplay Instance;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("Start() called in LeaderboardDisplay");
        LeaderBoardManager.Instance.OnNewEntryAdded.AddListener(UpdateLeaderboard);
        UpdateLeaderboard();
    }

    public void UpdateLeaderboard()
    {
        var leaderboardEntries = LeaderBoardManager.Instance.GetLeaderboard();
        for (int i = 0; i < leaderboardTexts.Length; i++)
        {
            if (i < leaderboardEntries.Count)
            {
                leaderboardTexts[i].text = $"{i + 1}. {leaderboardEntries[i].Item1} - {leaderboardEntries[i].Item2:F2}s";
            }
            else
            {
                leaderboardTexts[i].text = "hiiii";
            }
        }
        Debug.Log("Leaderboard updated.");
    }
    public void RefreshLeaderboard()
    {
        UpdateLeaderboard();
    }
}*/