using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownViewCharacterController : MonoBehaviour
{
    private PlayerInputActions _playerControls; // New Input system

    private InputAction _move;

    private Vector2 _playerMovementInput;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
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

        var _movementVector = MoveTowardsTarget(_movementTarget);

        RotateTowardsMovementVector(_movementVector);
    }

    private void RotateTowardsMovementVector(Vector3 _movementVector)
    {
        if (_movementVector.magnitude == 0) return;
        var _rotation = Quaternion.LookRotation(_movementVector);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotation, _rotateSpeed);
    }

    private Vector3 MoveTowardsTarget(Vector3 _target)
    {
        var _speed = _moveSpeed * Time.deltaTime;

        _target = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * _target;

        var _targetPoistion = transform.position + _target * _speed;
        
        transform.position = _targetPoistion;

        return _target;
    }

}
