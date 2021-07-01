using System.Collections;
using TMPro;
using UnityEngine;

public class HowToShield : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _centralText;
    [SerializeField] UIActionsManager _manager;
    [SerializeField] GameObject _nextButton;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerController.InvokeChargeChangeEvent();
        //_manager.UpdateButtons(_playerController._currentCharge);
    }

    private void Update()
    {
        if (_playerController._currentCharge < 9f)
        {
            _playerController._currentCharge = 9f;
            _playerController.InvokeChargeChangeEvent();
        }
    }

    void Start()
    {
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        StartCoroutine(Next());
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(1.5f);
        _centralText.text = "good.";
        yield return new WaitForSeconds(2f);
        _nextButton.SetActive(true);
    }
}
