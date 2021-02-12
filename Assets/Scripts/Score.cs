using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    static int _score = 0;
    static int _highScore = 0;
    public static event Action<int> OnScoreChange;
    public static Score Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    
    public void ZeroScore()
    {
        Debug.Log(_score);
        _score = 0;
    }
    public static void IncrementScore(int pointsToIncrement)
    {
        _score += pointsToIncrement;
        OnScoreChange(_score);
        SetHighScore();
    }

    static void SetHighScore()
    {
        if (_score > _highScore)
            PlayerPrefs.SetInt("HighScore", _score);
    }

    public static int CurrentScore()
    {
        return _score;
    }

    public static int CurrentHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", _score);
    }
}
