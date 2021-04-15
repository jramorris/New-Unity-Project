using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    TextMeshProUGUI _tmp;

    void Start()
    {
        Time.timeScale = 0;
        _tmp = GetComponent<TextMeshProUGUI>();
        StartCoroutine(StartGame(3));
    }

    IEnumerator StartGame(int seconds)
    {
        int count = seconds;

        while (count > 0)
        {

            _tmp.SetText(count.ToString());
            Debug.Log(_tmp);
            yield return new WaitForSecondsRealtime(1);
            count--;
        }

        // count down is finished...
        _tmp.enabled = false;
        Time.timeScale = 1;
    }
}
