using UnityEngine;
using UnityEngine.UI;

public class UIBarManager : MonoBehaviour
{
    private PlayerController _playerController;
    private float _maxCharge;
    private float _maxPower;
    private Image _fuelBarImage;
    private Material _fuelBarMaterial;
    private Image _chargeBarImage;
    private Material _chargeBarMaterial;
    private float _fillAmount;

    void Awake()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _maxCharge = _playerController._maxCharge;
        _maxPower = _playerController._maxPower;

        _fuelBarImage = transform.GetChild(0).gameObject.GetComponent<Image>();
        _fuelBarMaterial = _fuelBarImage.material;

        _chargeBarImage = transform.GetChild(1).gameObject.GetComponent<Image>();
        _chargeBarMaterial = _chargeBarImage.material;
    }

    private void OnEnable()
    {
        PlayerController.OnChargeChange += UpdateChargeBar;
    }

    private void OnDisable()
    {
        PlayerController.OnChargeChange -= UpdateChargeBar;
    }

    private void Update()
    {
        UpdateFuelBar(_playerController._currentPower);
    }

    void UpdateFuelBar(float currentPower)
    {
        _fillAmount = currentPower / _maxPower;

        _fuelBarMaterial.SetFloat("_fuelFillAmount", _fillAmount);
        _fuelBarImage.fillAmount = _fillAmount;

        if (_fuelBarImage.fillAmount < _fillAmount)
            _fuelBarImage.fillAmount += (Time.deltaTime * .1f * 3f);
        else
            _fuelBarImage.fillAmount = _fillAmount;
    }

    void UpdateChargeBar(float currentCharge)
    {
        _fillAmount = currentCharge / _maxCharge;

        _chargeBarMaterial.SetFloat("_fuelFillAmount", _fillAmount);
        _chargeBarImage.fillAmount = _fillAmount;

        if (_chargeBarImage.fillAmount < _fillAmount)
            _chargeBarImage.fillAmount += (Time.deltaTime * .1f * 3f);
        else
            _chargeBarImage.fillAmount = _fillAmount;
    }
}
