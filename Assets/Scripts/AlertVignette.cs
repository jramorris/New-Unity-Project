using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AlertVignette : MonoBehaviour
{

    public PostProcessProfile _profile;

    Vignette _vignetteOverride;
    FloatParameter _vignetteIntensity;
    bool _vignetteIsGrowing;
    AudioSource _alertAudio;

    private void Start()
    {
        _vignetteOverride = _profile.GetSetting<Vignette>();
        _vignetteIntensity = _vignetteOverride.intensity;
        _vignetteIsGrowing = _vignetteIntensity < .6f;
        _alertAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (PlayerController.offMap == true)
        {
            Alert();
        } else
        {
            StopAlert();
        }
    }

    private void StopAlert()
    {
        _vignetteOverride.enabled.value = false;
    }

    void Alert()
    {
        if (_vignetteOverride.enabled == false)
            InitializeVignette();
        UpdateVignette();
    }

    private void UpdateVignette()
    {
        if (_vignetteIsGrowing == true)
        {
            if (_vignetteIntensity.value < .6f)
                _vignetteIntensity.value += Time.deltaTime;
            else
                _vignetteIsGrowing = false;
        }
        else
        {
            if (_vignetteIntensity.value > .4f)
                _vignetteIntensity.value = Mathf.Lerp(_vignetteIntensity.value, .35f, .07f);
            else
                _vignetteIsGrowing = true;
        }
    }

    private void InitializeVignette()
    {
        _vignetteIntensity.value = .4f;
        _vignetteOverride.enabled.value = true;
        _vignetteIsGrowing = true;
        _alertAudio.Play();
    }
}
