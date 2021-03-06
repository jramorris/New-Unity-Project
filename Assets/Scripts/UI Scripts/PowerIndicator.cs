﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerIndicator : MonoBehaviour
{
    // unused script!
    [SerializeField] Image _powerIndicator;
    [SerializeField] Image _chargeIndicator;

    PlayerController _playerController;
    Button _pulseButton;
    float _maxPower;
    float _maxCharge;
    Coroutine shieldCoroutine;
    Coroutine powerCoroutine;


    void Awake()
    {

        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _maxPower = _playerController._maxPower;
        _maxCharge = _playerController._maxCharge;
        _pulseButton = GetComponentInChildren<Button>();
        //_pulseButton.interactable = false;
    }

    private void Update()
    {
        UpdatePowerIndicator(_playerController._currentPower);
    }

    private void OnEnable()
    {
        //PlayerController.OnCollectPower += UpdatePowerIndicator;
        //PlayerController.OnFullCharge += UpdatePulseButton;
        //PlayerController.OnChargeChange += UpdateChargeIndicator;
    }

    private void OnDisable()
    {
        PlayerController.OnCollectPower -= UpdatePowerIndicator;
        PlayerController.OnFullCharge -= UpdatePulseButton;
        PlayerController.OnChargeChange -= UpdateChargeIndicator;
    }

    private void UpdateChargeIndicator(float currentCharge)
    {
        if (shieldCoroutine != null)
            StopCoroutine(shieldCoroutine);
        shieldCoroutine = StartCoroutine(FillIndicator(currentCharge / _maxCharge, _chargeIndicator, 5.5f));
    }

    public void UpdatePowerIndicator(float newPowerLevel)
    {
        float fillAmount = getCurrentDecimal(newPowerLevel, _maxPower);
        //if (powerCoroutine != null)
        //    StopCoroutine(powerCoroutine);
        //powerCoroutine = StartCoroutine(FillIndicator(fillAmount, _powerIndicator, 3f));
        if (_powerIndicator.fillAmount < fillAmount)
            _powerIndicator.fillAmount += (Time.deltaTime * .1f * 3f);
        else
            _powerIndicator.fillAmount = fillAmount;
    }

    IEnumerator FillIndicator(float fillAmount, Image indicatorImage, float rateMultiplier)
    {
        while (indicatorImage.fillAmount < fillAmount)
        {
            indicatorImage.fillAmount += (Time.deltaTime * .1f * rateMultiplier);
            yield return null;
        }
        if (indicatorImage.fillAmount > fillAmount)
            indicatorImage.fillAmount = fillAmount;
    }

    private float getCurrentDecimal(float currentFloat, float maxFloat)
    {
        return currentFloat / maxFloat;
    }

    void UpdatePulseButton()
    {
        _pulseButton.interactable = true;
    }
}
