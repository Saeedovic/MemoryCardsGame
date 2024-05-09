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
        return flippedCardsCount >= 2; 
    }

    public void AddVisibleFace(int index)
    {
        if (flippedCardsCount < 2)
        { 
            if (visibleFaces[0] == -1)
            {
                visibleFaces[0] = index;
            }
            else if (visibleFaces[1] == -2)
            {
                visibleFaces[1] = index;
            }
            flippedCardsCount++;
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
        flippedCardsCount--; 
    }

    public bool AllCardsMatched()
    {
        foreach (int index in faceIndexes)
        {
            if (index >= 0)
            {
                return false; 
            }
        }
        return true; 
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
            matchCount++; 

            
            flippedCardsCount -= 2;

            if (matchCount == 4) 
            {
                Debug.Log("All cards matched! Game finished!");
                gameFinished = true;
                DestroyAllTokens(); 
                inputFieldGameObject.SetActive(true);
                submitButtonGameObject.SetActive(true);

                
                visibleFaces[0] = -1;
                visibleFaces[1] = -2;

                
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
            Destroy(gameObject); 
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
            Destroy(token); 
        }
    }
    public void OnSubmitPlayerName()
    {
        
        TMP_InputField inputField = inputFieldGameObject.GetComponent<TMP_InputField>();
        if (inputField != null)
        {
            
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

