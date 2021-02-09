using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    TMP_Text _tmp;

    void Start()
    {
        _tmp = GetComponent<TMP_Text>();
        Score.OnScoreChange += DisplayScore;
    }
    public void DisplayScore(int score)
    {
        _tmp.SetText(score.ToString());
    }

    void OnDestroy()
    {
        Score.OnScoreChange -= DisplayScore;
    }
}
