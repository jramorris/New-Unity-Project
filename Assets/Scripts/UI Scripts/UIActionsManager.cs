using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIActionsManager : MonoBehaviour
{
    private PlayerController _playerController;
    private float _maxCharge;

    private int _requiredNovaCharge;
    private GameObject _novaObj;
    private Button _novaButton;
    private Image _novaImage;
    Coroutine novaCoroutine;

    private int _requiredShieldCharge;
    private GameObject _shieldObj;
    private Button _shieldButton;
    private Image _shieldImage;
    Coroutine shieldCoroutine;

    private int _requiredSpeedCharge;
    private GameObject _speedObj;
    private Button _speedButton;
    private Image _speedImage;
    private TextMeshProUGUI _speedTextObj;
    Coroutine speedCoroutine;

    void Awake()
    {

        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _maxCharge = _playerController._maxCharge;

        // nova
        _requiredNovaCharge = _playerController._requiredNovaCharge;
        _novaObj = transform.GetChild(0).GetChild(1).gameObject;
        _novaButton = _novaObj.GetComponent<Button>();
        _novaImage = _novaObj.GetComponent<Image>();

        // shield
        _requiredShieldCharge = _playerController._powerToChargeShield;
        _shieldObj = transform.GetChild(1).gameObject;
        _shieldButton = _shieldObj.GetComponent<Button>();
        _shieldImage = _shieldObj.GetComponent<Image>();

        // speed
        _requiredSpeedCharge = _playerController._requiredSpeedUpCharge;
        _speedObj = transform.GetChild(2).gameObject;
        _speedButton = _speedObj.GetComponent<Button>();
        _speedImage = _speedObj.GetComponent<Image>();
        _speedTextObj = _speedObj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        PlayerController.OnChargeChange += UpdateButtons;
    }

    private void OnDisable()
    {
        PlayerController.OnChargeChange -= UpdateButtons;
    }

    void UpdateButtons(float currentCharge)
    {
        UpdateNovaButton(currentCharge);
        UpdateShieldButton(currentCharge);
        UpdateSpeedButton(currentCharge);
    }

    private void UpdateNovaButton(float currentCharge)
    {
        if (novaCoroutine != null)
            StopCoroutine(novaCoroutine);
        novaCoroutine = StartCoroutine(FillIndicator(currentCharge / _requiredNovaCharge, _novaImage, 4f));
    }

    private void UpdateShieldButton(float currentCharge)
    {
        if (shieldCoroutine != null)
            StopCoroutine(shieldCoroutine);
        shieldCoroutine = StartCoroutine(FillIndicator(currentCharge / _requiredShieldCharge, _shieldImage, 5.5f));
    }

    private void UpdateSpeedButton(float currentCharge)
    {
        if (speedCoroutine != null)
            StopCoroutine(speedCoroutine);
        speedCoroutine = StartCoroutine(FillIndicator(currentCharge / _requiredSpeedCharge, _speedImage, 7f));
    }

    public void UpdateSpeedText()
    {
        _speedTextObj.text = $"{_playerController._movementModifier}x";
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
}
