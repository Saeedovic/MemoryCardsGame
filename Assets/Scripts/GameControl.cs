using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;

    public LeaderboardManager leaderboardManager;
    public GameObject tokenPrefab;
    List<int> faceIndexes = new List<int> { 0, 1, 2, 3, 0, 1, 2, 3 };
    public static System.Random rnd = new System.Random();
    public int shuffleNum = 0;
    int[] visibleFaces = { -1, -2 };
    public Vector3[] instantiatePositions;
    private int flippedCardsCount = 0;

    [SerializeField]
    private float startTime;
    public bool gameFinished = false;
    public GameObject inputFieldGameObject;
    public GameObject submitButtonGameObject;
    private int matchCount = 0;


    void Start()
    {
        startTime = Time.time;
        InitializeTokens();
    }

    private void InitializeTokens()
    {
        for (int i = 0; i < instantiatePositions.Length; i++)
        {
            var temp = Instantiate(tokenPrefab, instantiatePositions[i], Quaternion.identity);
            shuffleNum = rnd.Next(0, faceIndexes.Count);
            temp.GetComponent<MainToken>().faceIndex = faceIndexes[shuffleNum];
            faceIndexes.Remove(faceIndexes[shuffleNum]);
            temp.tag = "Token";
        }
            tokenPrefab.SetActive(false);
    }

    public bool TwoCardsUp()
    {
        return flippedCardsCount >= 2; // Allow flipping only if less than two cards are flipped
    }

    public void AddVisibleFace(int index)
    {
        if (flippedCardsCount < 2)
        { // Allow adding only if less than two cards are flipped
            if (visibleFaces[0] == -1)
            {
                visibleFaces[0] = index;
            }
            else if (visibleFaces[1] == -2)
            {
                visibleFaces[1] = index;
            }
            flippedCardsCount++; // Increment flipped cards count
        }
    }

    public void RemoveVisibleFace(int index)
    {
        if (visibleFaces[0] == index)
        {
            visibleFaces[0] = -1;
        }
        else if (visibleFaces[1] == index)
        {
            visibleFaces[1] = -2;
        }
        flippedCardsCount--; // Decrement flipped cards count
    }

    public bool AllCardsMatched()
    {
        foreach (int index in faceIndexes)
        {
            if (index >= 0)
            {
                return false; // If any card is not matched, return false
            }
        }
        return true; // If all cards are matched, return true
    }

    public bool CheckMatch()
    {
        Debug.Log("Checking match...");
        bool success = false;
        if (visibleFaces[0] == visibleFaces[1])
        {
            visibleFaces[0] = -1;
            visibleFaces[1] = -2;
            success = true;
            Debug.Log("Matched!");
            matchCount++; // Increment match count

            // Decrement flipped cards count by 2
            flippedCardsCount -= 2;

            if (matchCount == 4) // Check if all cards are matched
            {
                Debug.Log("All cards matched! Game finished!");
                gameFinished = true;
                DestroyAllTokens(); // Destroy all tokens here
                inputFieldGameObject.SetActive(true);
                submitButtonGameObject.SetActive(true);

                // Reset visibleFaces array
                visibleFaces[0] = -1;
                visibleFaces[1] = -2;

                // Reset flipped cards count
                flippedCardsCount = 0;
            }
        }
        return success;
    }
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy the duplicate instance
        }

        tokenPrefab = GameObject.Find("Token");

        inputFieldGameObject.SetActive(false);
        submitButtonGameObject.SetActive(false);
    }
    public void GameFinished(string playerName)
    {
        Debug.Log("Game finished. Player: " + playerName + ", Time: " + (Time.time - startTime));

        if (gameFinished)
        {
            float timeTaken = Time.time - startTime;
            leaderboardManager.AddEntry(playerName, timeTaken);

            // Add the following line to check if the leaderboardEntries list is not empty
            Debug.Log("Leaderboard entries count: " + leaderboardManager.leaderboardEntries.Count);

            leaderboardManager.SaveLeaderboard();
            gameFinished = true;
            DestroyAllTokens();
            inputFieldGameObject.SetActive(true);
            submitButtonGameObject.SetActive(true);
        }
    }

    public void DestroyAllTokens()
    {
        GameObject[] tokens = GameObject.FindGameObjectsWithTag("Token");
        foreach (GameObject token in tokens)
        {
            Destroy(token); // Destroy each token
        }
    }
    public void OnSubmitPlayerName()
    {
        // Attempt to get the TMP_InputField component
        TMP_InputField inputField = inputFieldGameObject.GetComponent<TMP_InputField>();
        if (inputField != null)
        {
            // Only proceed if the component is successfully retrieved
            string playerName = inputField.text;
            GameFinished(playerName);

            inputFieldGameObject.SetActive(false);
            submitButtonGameObject.SetActive(false);
            leaderboardManager.SaveLeaderboard();
        }
        else
        {
            Debug.LogError("TMP_InputField component not found on inputFieldGameObject.");
        }
    }



}

