using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsViewer : MonoBehaviour
{
    public void Start()
    {
        // Specify the maximum number of PlayerPrefs you expect to have
        const int maxPrefs = 2;

        // Iterate through possible keys and print their values if they exist
        for (int i = 0; i < maxPrefs; i++)
        {
            string key = "PlayerPref_" + i;
            if (PlayerPrefs.HasKey(key))
            {
                Debug.Log(key + ": " + PlayerPrefs.GetString(key));
            }
        }
    }
}
