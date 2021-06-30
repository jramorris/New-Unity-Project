using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToCollect : MonoBehaviour
{
    // The how to scripts are all Mega-hacks
    // to get the tutorial to work.

    [SerializeField] TextMeshProUGUI _centralText;
    [SerializeField] TextMeshProUGUI _powerCellText;
    [SerializeField] GameObject _arrow;

    public void NextStep()
    {
        _centralText.text = "You got more power.  You don't want that to run out.";
        _powerCellText.enabled = false;
        _arrow.SetActive(false);
        StartCoroutine(LoadAfterSeconds());
    }

    IEnumerator LoadAfterSeconds()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("HowToAction");
    }
}
