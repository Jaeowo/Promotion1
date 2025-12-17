using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

using static PlayerInputAction;

[CreateAssetMenu(fileName = "InputReader", menuName = "Topdown/Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event UnityAction<Vector2> Move = delegate { };
    public event UnityAction<bool> Jump = delegate { };

    PlayerInputAction inputAction;

    public Vector2 Direction => inputAction.Player.Move.ReadValue<Vector2>();

    void OnEnable()
    {
        if (inputAction == null)
        {
            inputAction = new PlayerInputAction();
            inputAction.Player.SetCallbacks(this);
        }
        inputAction.Enable();
    }

    void OnDisable()
    {
        if (inputAction != null)
        {
            inputAction.Player.Disable();
            inputAction.Disable();
        }
    }

    public void EnablePlayerActions()
    {
        inputAction.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Move.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Jump.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                Jump.Invoke(false);
                break;
            default:
                break;
        }
    }


}
