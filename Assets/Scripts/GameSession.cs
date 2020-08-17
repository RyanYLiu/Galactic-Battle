using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int score = 0;

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int numGameSessions = FindObjectsOfType(GetType()).Length;
        if (numGameSessions > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddToScore(int pointValue)
    {
        score += pointValue;
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        Destroy(gameObject);
    }
}
