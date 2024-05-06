using System;
using System.IO;
using UnityEngine;

public class TestSaveLoad : MonoBehaviour
{
    public string playerName = "Saeed";

    public void SaveName()
    {
        string json = JsonUtility.ToJson(playerName);
        File.WriteAllText(Application.persistentDataPath + "/testName.json", json);
        Debug.Log("Saved playerName: " + playerName);
    }

    public void LoadName()
    {
        if (File.Exists(Application.persistentDataPath + "/testName.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/testName.json");
            playerName = JsonUtility.FromJson<string>(json);
            Debug.Log("Loaded playerName: " + playerName);
        }
        else
        {
            Debug.LogError("No saved playerName found.");
        }
    }
}
