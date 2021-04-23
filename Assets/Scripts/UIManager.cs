using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void LaunchDeathInterstitial()
    {
        transform.Find("DeathInterstitial").gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
