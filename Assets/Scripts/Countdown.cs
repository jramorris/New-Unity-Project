using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    TextMeshProUGUI _tmp;
    [SerializeField] int _countdownSeconds = 3;

    void Start()
    {
        Time.timeScale = 0;
        _tmp = GetComponent<TextMeshProUGUI>();
        StartCoroutine(StartGame(_countdownSeconds));
    }

    IEnumerator StartGame(int seconds)
    {
        int count = seconds;

        while (count > 0)
        {

            _tmp.SetText(count.ToString());
            yield return new WaitForSecondsRealtime(1);
            count--;
        }

        // count down is finished...
        _tmp.enabled = false;
        Time.timeScale = 1;
    }
}
