using System;
using UnityEngine;
using UnityEngine.UI;

public class CircularProgressBar : MonoBehaviour
{
    Image _image;
    int _maxCharge;

    void Start()
    {
        _image = GetComponent<Image>();
        _maxCharge = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>()._maxCharge;
    }

    private void OnEnable()
    {
        PlayerController.OnChargeChange += UpdatePower;
    }

    private void OnDisable()
    {
        PlayerController.OnChargeChange -= UpdatePower;
    }

    public void UpdatePower(int newPowerLevel)
    {
        decimal fillDecimal = decimal.Divide(newPowerLevel, _maxCharge);
        float fillAmount = Convert.ToSingle(fillDecimal);
        _image.fillAmount = fillAmount;
    }
}
