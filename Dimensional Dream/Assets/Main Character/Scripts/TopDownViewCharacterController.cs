using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownViewCharacterController : MonoBehaviour
{
    private PlayerInputActions _playerControls; // New Input system

    private InputAction _move;

    private Vector2 _playerMovementInput;

    private void Awake()
    {
        _playerControls = new PlayerInputActions();
    }


    private void OnEnable()
    {
        _move = _playerControls.Player.Move;
        _move.Enable();
    }

    private void OnDisable()
    {
        _move.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        _playerMovementInput = _move.ReadValue<Vector2>();
        
        var _movementTarget = new Vector3 (_playerMovementInput.x, 0 , _playerMovementInput.y);
    }
}
