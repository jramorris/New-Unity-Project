using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HowToSteer : MonoBehaviour
{
    // The how to scripts are all Mega-hacks
    // to get the tutorial to work.

    [SerializeField] InputAction moveAction;
    private float horizontal;
    private float vertical;
    [SerializeField] TextMeshProUGUI _centralText;

    private void OnDisable()
    {
        moveAction.Disable();
    }

    void ReadInput()
    {
        var moveDirection = moveAction.ReadValue<Vector2>();
        horizontal = moveDirection.x;
        vertical = moveDirection.y;
    }

    void Update()
    {
        if (!moveAction.enabled)
        {
            // input system doesnt allow enabling in Awake/Start, etc.
            moveAction.Enable();
        }

        ReadInput();

        if (horizontal != 0 || vertical != 0)
            Step();
    }

    void Step()
    {
        _centralText.text = "Nice.";
    }
}
