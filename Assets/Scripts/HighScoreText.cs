using TMPro;
using UnityEngine;

public class HighScoreText : MonoBehaviour
{
    void Start()
    {
        string scoreText = Score.CurrentHighScore().ToString();
        GetComponent<TMP_Text>().SetText($"HighScore: {scoreText}");
    }
}
