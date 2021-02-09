using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] string _sceneName;

    public void GoToLevel()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
