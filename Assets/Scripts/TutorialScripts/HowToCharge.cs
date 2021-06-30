using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToCharge : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _centralText;
    [SerializeField] GameObject _collectibleArrow;
    [SerializeField] GameObject _novaArrow;
    [SerializeField] GameObject _nextButton;
    private PlayerController _controller;
    private bool _novaUsed;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_controller._currentCharge < 9f && !_novaUsed)
        {
            _controller._currentCharge = 9f;
            _controller.InvokeChargeChangeEvent();
        }
    }

    public void CollectStep()
    {
        _centralText.text = "Now try the supernova.";
        _collectibleArrow.SetActive(false);
        _novaArrow.SetActive(true);
        _novaUsed = true;
    }

    public void NovaStep()
    {
        _novaArrow.SetActive(false);
        StartCoroutine(UpdateTextAfterSeconds());
        StartCoroutine(LoadAfterSeconds(5));
    }

    public void NextButton()
    {
        return;
    }

    IEnumerator UpdateTextAfterSeconds()
    {
        yield return new WaitForSeconds(1);
        _centralText.text = "Pretty cool.";

        yield return new WaitForSeconds(2);
        _centralText.text =  "You'll need to collect more powercells to recharge your ship.";

        yield return new WaitForSeconds(2);
        _nextButton.SetActive(true);
    }

    IEnumerator LoadAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        //SceneManager.LoadScene("HowToAction");
    }
}
