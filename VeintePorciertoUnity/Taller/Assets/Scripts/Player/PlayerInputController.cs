using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private InputAction _moveAction;
    private InputAction _jumpAction;

    public Vector2 MoveInput { get; private set; }
    public bool JumpInput { get; private set; }

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");

        if (_moveAction != null)
        {
            _moveAction.Enable();
            Debug.Log("[PlayerInputController] Acción 'Move' encontrada y habilitada.");
        }
        else
        {
            Debug.LogError("[PlayerInputController] No se encontró la acción 'Move'.");
        }

        if (_jumpAction != null)
        {
            _jumpAction.Enable();
            Debug.Log("[PlayerInputController] Acción 'Jump' encontrada y habilitada.");
        }
        else
        {
            Debug.LogError("[PlayerInputController] No se encontró la acción 'Jump'.");
        }
    }

    private void Update()
    {
        if (_moveAction == null) return;

        MoveInput = _moveAction.ReadValue<Vector2>();

        if (MoveInput != Vector2.zero)
        {
            Debug.Log($"[PlayerInputController] Input detectado: {MoveInput}");
        }

        if (_jumpAction != null && _jumpAction.WasPressedThisFrame())
        {
            JumpInput = true;
            Debug.Log("[PlayerInputController] Jump presionado.");
        }
    }

    // PlayerMovementModel llama esto después de procesar el salto.
    public void ConsumeJump()
    {
        JumpInput = false;
    }

    private void OnDisable()
    {
        if (_moveAction != null)
        {
            _moveAction.Disable();
            Debug.Log("[PlayerInputController] Acción 'Move' deshabilitada.");
        }

        if (_jumpAction != null)
        {
            _jumpAction.Disable();
            Debug.Log("[PlayerInputController] Acción 'Jump' deshabilitada.");
        }
    }
}