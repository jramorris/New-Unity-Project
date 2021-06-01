using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIBarManager : MonoBehaviour
{
    [SerializeField] float _chargeFillDuration = 1f;
    private PlayerController _playerController;
    private float _maxCharge;
    private float _maxPower;
    private Image _fuelBarImage;
    private Material _fuelBarMaterial;
    private Image _chargeBarImage;
    private Material _chargeBarMaterial;
    private float _startUpdateChargeTime;
    private float _chargeFillAmount;
    private float _decimalTimePassed;
    private float _fuelFillAmount;

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
        _fuelFillAmount = currentPower / _maxPower;

        // set color via shader
        _fuelBarMaterial.SetFloat("_fuelFillAmount", _fuelFillAmount);

        if (_fuelBarImage.fillAmount < _fuelFillAmount)
            _fuelBarImage.fillAmount += (Time.deltaTime * .1f * 3f);
        else
            _fuelBarImage.fillAmount = _fuelFillAmount;
    }

    void UpdateChargeBar(float currentCharge)
    {
        _chargeFillAmount = currentCharge / _maxCharge;

        // set color via shader
        _chargeBarMaterial.SetFloat("_fuelFillAmount", _chargeFillAmount);

        if (_chargeBarImage.fillAmount < _chargeFillAmount)
        {
            _startUpdateChargeTime = Time.time;
            StartCoroutine(FillChargeBar());
        }
        else
            _chargeBarImage.fillAmount = _chargeFillAmount;
    }

    IEnumerator FillChargeBar()
    {
        _decimalTimePassed = 0f;
        while (_decimalTimePassed < _chargeFillDuration)
        {
            _decimalTimePassed = (Time.time - _startUpdateChargeTime) / _chargeFillDuration;
            _chargeBarImage.fillAmount = Mathf.SmoothStep(_chargeBarImage.fillAmount, _chargeFillAmount, _decimalTimePassed);
            yield return null;
        }
        Debug.Log("exited");
        _chargeBarImage.fillAmount = _chargeFillAmount;
    }
}
