using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

using static PlayerInputAction;

[CreateAssetMenu(fileName = "InputReader", menuName = "Topdown/Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event UnityAction<Vector2> Move = delegate { };
    public event UnityAction Jump = delegate { };
    public event UnityAction Dash = delegate { };
    public event UnityAction Slash = delegate { };
    public event UnityAction Skill = delegate { };

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
        if (context.phase == InputActionPhase.Started)
        {
            Jump.Invoke();

        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Dash.Invoke();

        }
    }

    public void OnSlash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Slash.Invoke();
        }
    }

    public void OnSkill(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Skill.Invoke();

        }
    }

}
