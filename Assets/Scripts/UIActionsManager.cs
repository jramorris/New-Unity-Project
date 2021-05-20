using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActionsManager : MonoBehaviour
{
    private PlayerController _playerController;
    private float _maxCharge;

    void Awake()
    {

        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _maxCharge = _playerController._maxCharge;
        //_pulseButton = GetComponentInChildren<Button>();
        //_pulseButton.interactable = false;
    }

    private void Update()
    {
        //UpdatePowerIndicator(_playerController._currentPower);
    }

    private void OnEnable()
    {
        //PlayerController.OnCollectPower += UpdatePowerIndicator;
        //PlayerController.OnFullCharge += UpdatePulseButton;
        //PlayerController.OnChargeChange += UpdateChargeIndicator;
    }
}
