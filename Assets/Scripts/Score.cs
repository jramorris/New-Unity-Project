using System;
using UnityEngine;

public static class Score
{
    static int _score = 0;
    static int _highScore = 0;
    public static event Action<int> OnScoreChange;

    public static void IncrementScore(int pointsToIncrement)
    {
        _score += pointsToIncrement;
        OnScoreChange(_score);
        SetHighScore();
    }

    public static void ResetScore()
    {
        _score = 0;
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
