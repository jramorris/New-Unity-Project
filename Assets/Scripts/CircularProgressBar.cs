using UnityEngine;
using UnityEngine.UI;

public class CircularProgressBar : MonoBehaviour
{
    [SerializeField] int _maxPower = 10;

    Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    public void IncrementPowerBar()
    {
        _image.fillAmount += .1f;
    }
}
