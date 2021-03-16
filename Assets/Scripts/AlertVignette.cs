using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//using UnityEngine.Rendering;
//using UnityEngine.Rendering.PostProcessing;

public class AlertVignette : MonoBehaviour
{

    [SerializeField] float _vignetteShrinkRate = .07f;

    public VolumeProfile _volumeProfile;
    UnityEngine.Rendering.ClampedFloatParameter _vignetteIntensity;
    bool _vignetteIsGrowing;
    AudioSource _alertAudio;

    private void Start()
    {
        if (_volumeProfile.TryGet<Vignette>(out Vignette vignette))
        {
            _vignetteIntensity = vignette.intensity;
            _vignetteIsGrowing = _vignetteIntensity.value < .5f;
            _alertAudio = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        // TODO move from update to event & coroutine
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
        _vignetteIntensity.overrideState = false;
    }

    void Alert()
    {
        if (_vignetteIntensity.overrideState == false)
            InitializeVignette();
        UpdateVignette();
    }

    private void UpdateVignette()
    {
        if (_vignetteIsGrowing == true)
        {
            if (_vignetteIntensity.value < .5f)
                _vignetteIntensity.value += Time.deltaTime;
            else
                _vignetteIsGrowing = false;
        }
        else
        {
            if (_vignetteIntensity.value > .4f)
                _vignetteIntensity.value = Mathf.Lerp(_vignetteIntensity.value, .35f, _vignetteShrinkRate);
            else
                _vignetteIsGrowing = true;
        }
    }

    private void InitializeVignette()
    {
        _vignetteIntensity.overrideState = true;
        _vignetteIntensity.value = .4f;
        _vignetteIsGrowing = true;
        _alertAudio.Play();
    }
}
