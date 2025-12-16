using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

using static PlayerInputAction;

[CreateAssetMenu(fileName = "InputReader", menuName = "Topdown/Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event UnityAction<Vector2> Move = delegate { };

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

    public void OnMove(InputAction.CallbackContext context)
    {
        Move.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
      
    }


}
