using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TopDownViewCharacterController : MonoBehaviour
{
    private PlayerInputActions _playerControls; // New Input system

    private Animator _animator;

    private InputAction _move;

    private Vector2 _playerMovementInput;

    public Transform _lastcheckpoint;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    private void Awake()
    {
        _playerControls = new PlayerInputActions();
        _animator = GetComponent<Animator>();
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

    private void Start()
    {
        _lastcheckpoint = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _playerMovementInput = _move.ReadValue<Vector2>();

        if (_playerMovementInput.magnitude >= 0.1f)
        {
            _animator.SetBool("Run",true);
        }
        else
        {
            _animator.SetBool("Run", false);
        }
        
        var _movementTarget = new Vector3 (_playerMovementInput.x, 0 , _playerMovementInput.y);

        var _movementVector = MoveTowardsTarget(_movementTarget);

        RotateTowardsMovementVector(_movementVector);

        CheckIfPlayerFalled();
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


    private void CheckIfPlayerFalled()
    {
        if (transform.position.y <= -5f)
        {
            transform.position = _lastcheckpoint.position;
        }
    }

    public void SetCheckPoint(Transform _transform)
    {
        _lastcheckpoint = _transform;
    }

}
