using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TopDownViewCharacterController : MonoBehaviour
{
    private PlayerInputActions _playerControls; // New Input system

    private AnimationClip standingUpAnimationClip; // Reference to the "standing_up" animation clip



    private Animator _animator;

    private InputAction _move;
    private InputAction _attack;

    private Vector2 _playerMovementInput;

    public Transform _lastcheckpoint;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    private void Awake()
    {
        _playerControls = new PlayerInputActions();
        _animator = GetComponent<Animator>();
        // Get the "Attack" action from the PlayerInputActions asset
        _attack = _playerControls.Player.Fire;

        // Get the "standing_up" animation clip from the Animator
        standingUpAnimationClip = _animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == "stand up");
    }


    private void OnEnable()
    {
        _move = _playerControls.Player.Move;
        _move.Enable();
        _attack = _playerControls.Player.Fire;
        _attack.Enable();
    }

 

    private void OnDisable()
    {
        _move.Disable();

        // Unbind the "Fire" action
        _attack.Disable();
    }

    private void Start()
    {
        _lastcheckpoint = transform;
        _animator.SetBool("started_playing", false);
        _animator.SetBool("finishStandup", false);
    }

    // Update is called once per frame
    void Update()
    {
        _playerMovementInput = _move.ReadValue<Vector2>();

        if (!_animator.GetBool("started_playing") && _playerMovementInput.magnitude >= 0.1f)
        {
            // If the player pressed any movement key and "started_playing" is false, trigger the standing up animation
            _animator.SetBool("started_playing", true);
            Invoke("FinishStandupAnimation", standingUpAnimationClip.length);
        }

        if (_playerMovementInput.magnitude >= 0.1f)
        {
            _animator.SetBool("Run",true);
        }
        else
        {
            _animator.SetBool("Run", false);
        }
        
        if (_animator.GetBool("finishStandup"))
        {
            var _movementTarget = new Vector3(_playerMovementInput.x, 0, _playerMovementInput.y);

            var _movementVector = MoveTowardsTarget(_movementTarget);

            RotateTowardsMovementVector(_movementVector);

            CheckIfPlayerFalled();

            if (_attack.WasPressedThisFrame())
            {
                _animator.SetTrigger("heavy");
            }

        }


 
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


    public void FinishStandupAnimation()
    {
        // This function will be called when the standing up animation finishes.
        // Set the "FinishStandup" parameter to true to signal that the animation is complete.
        _animator.SetBool("finishStandup", true);
    }
      
}
