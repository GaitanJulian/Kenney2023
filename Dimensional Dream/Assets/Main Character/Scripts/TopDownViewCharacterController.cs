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

    public bool _canHit = false;

    private Animator _animator;

    private InputAction _move;
    private InputAction _attack;

    public float attackRange = 1.5f;
    public LayerMask enemyLayer;
    public LayerMask breakableLayer;

    private Vector2 _playerMovementInput;

    public Transform _lastcheckpoint;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    private bool _canAttack = true;
    private float _attackCooldown = 1.0f; // The time in seconds between attacks
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

            if (_attack.WasPressedThisFrame() && _canHit && _canAttack)
            {
                // Check for enemy within the attack range
                Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, attackRange, enemyLayer);
                Collider[] hitBreakeable = Physics.OverlapSphere(transform.position + transform.forward, attackRange, breakableLayer);
                _animator.SetTrigger("heavy");
                _canAttack = false; // Disable attack until the cooldown is over
                StartCoroutine(AttackCooldown());

                // Call StartKnockback on each hit enemy with the knockback direction
                foreach (Collider enemyCollider in hitEnemies)
                {
                    Enemies enemy = enemyCollider.GetComponent<Enemies>();
                    if (enemy != null)
                    {
                        Vector3 knockbackDirection = enemy.transform.position - transform.position;
                        knockbackDirection.y = 0f;
                        enemy.StartKnockback(knockbackDirection);
                    }
                }

                foreach (Collider breakeableCollider in hitBreakeable)
                {
                    Breakeable breakeable = breakeableCollider.GetComponent<Breakeable>();
                    if (breakeable != null)
                    {
                        breakeable.BreakObject();
                    }
                }

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
    
    public void ToggleAttack()
    {
        _canHit = true;
    }

    // Coroutine to reset the attack cooldown
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true; // Enable attack again after the cooldown
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack range region in the Unity Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, attackRange);
    }
}
