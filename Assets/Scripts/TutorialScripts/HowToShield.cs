using UnityEngine;

public class HowToShield : MonoBehaviour
{
    [SerializeField] UIActionsManager _manager;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerController.InvokeChargeChangeEvent();
        //_manager.UpdateButtons(_playerController._currentCharge);
    }

    private void Update()
    {
        if (_playerController._currentCharge < 9f)
        {
            _playerController._currentCharge = 9f;
            _playerController.InvokeChargeChangeEvent();
        }
    }

    void Start()
    {
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
    }
}
