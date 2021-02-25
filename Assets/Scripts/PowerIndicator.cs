using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerIndicator : MonoBehaviour
{
    [SerializeField] Image _chargeProgressIndicator;
    [SerializeField] Image _shieldIndicator;

    int _maxPower;
    int _maxShieldCharges;
    Coroutine shieldCoroutine;
    Coroutine powerCoroutine;


    void Awake()
    {
        _maxPower = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>()._maxCharge;
        _maxShieldCharges = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>()._maxShieldCharges;
    }

    private void OnEnable()
    {
        PlayerController.OnChargeChange += UpdatePowerIndicator;
        PlayerController.OnShieldChargeChange += UpdateShieldIndicator;
    }

    private void OnDisable()
    {
        PlayerController.OnChargeChange -= UpdatePowerIndicator;
        PlayerController.OnShieldChargeChange -= UpdateShieldIndicator;
    }

    private void UpdateShieldIndicator(int currentCharges)
    {
        float fillAmount = getCurrentDecimal(currentCharges, _maxShieldCharges);
        if (shieldCoroutine != null)
            StopCoroutine(shieldCoroutine);
        shieldCoroutine = StartCoroutine(FillIndicator(fillAmount, _shieldIndicator, 5.5f));
    }

    public void UpdatePowerIndicator(int newPowerLevel)
    {
        float fillAmount = getCurrentDecimal(newPowerLevel, _maxPower);
        if (powerCoroutine != null)
            StopCoroutine(powerCoroutine);
        powerCoroutine = StartCoroutine(FillIndicator(fillAmount, _chargeProgressIndicator, 1.1f));
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

    private float getCurrentDecimal(int currentInt, int maxInt)
    {
        decimal fillDecimal = decimal.Divide(currentInt, maxInt);
        float fillAmount = Convert.ToSingle(fillDecimal);
        return fillAmount;
    }
}
