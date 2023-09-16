using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : MonoBehaviour
{
    private PlayerInput _playerInput;

    public ReactiveCommand<Vector2> OnMouseMoved = new();
    public ReactiveCommand<Vector2> OnMouseClicked = new();
    public ReactiveCommand OnSpaceClicked = new();

    private void Awake()
    {
        _playerInput = new();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        
        _playerInput.Mouse.MouseMovement.performed += MouseMove;
        _playerInput.Mouse.MouseMovement.canceled += MouseMove;

        _playerInput.Mouse.MouseClick.performed += MouseClick;

        _playerInput.KeyBoard.SkipTurn.performed += SpaceClick;
    }

    private void MouseMove(InputAction.CallbackContext callback)
    {
        var value = callback.ReadValue<Vector2>();

        OnMouseMoved?.Execute(value);
    } 
    
    private void MouseClick(InputAction.CallbackContext callback)
    {
        var value = _playerInput.Mouse.MouseMovement.ReadValue<Vector2>();

        OnMouseClicked?.Execute(value);
    }

    private void SpaceClick(InputAction.CallbackContext callback)
    {
        OnSpaceClicked?.Execute();
    }

    private void OnDisable()
    {
        _playerInput.Mouse.MouseMovement.performed -= MouseMove;
        _playerInput.Mouse.MouseMovement.canceled -= MouseMove;

        _playerInput.Mouse.MouseClick.performed -= MouseClick;
        
        _playerInput.KeyBoard.SkipTurn.performed -= SpaceClick;

        _playerInput.Disable();
    }
}