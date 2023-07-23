using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float movementSpeed = 2f;
    public float detectionRange = 5f;
    public float knockbackForce = 5f;
    public float KnockbackDistance = 0.5f;
    public int hitcount = 3;

    private Transform player;
    private bool isPlayerDetected = false;

    // Add a Coroutine reference to handle knockback
    private Coroutine knockbackCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isPlayerDetected)
        {
            RotateTowardsPlayer();
            MoveTowardsPlayer();
        }

        if ( hitcount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void PlayerDetected()
    {
        isPlayerDetected = true;
    }

    
    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f; // Keep the enemy facing horizontally
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

        private void MoveTowardsPlayer()
        {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f; // Only move in the horizontal plane

        float distanceToPlayer = directionToPlayer.magnitude;

            directionToPlayer.Normalize();
            transform.position += directionToPlayer * movementSpeed * Time.deltaTime;
        }



    // Method to initiate knockback coroutine
    public void StartKnockback(Vector3 knockbackDirection)
    {
        if (knockbackCoroutine == null)
        {
            knockbackCoroutine = StartCoroutine(KnockbackCoroutine(knockbackDirection));
            hitcount--;
        }
    }

    // Coroutine to handle knockback
    private IEnumerator KnockbackCoroutine(Vector3 knockbackDirection)
    {
        // Stop enemy movement during knockback
        isPlayerDetected = false;

        // Apply knockback to the enemy
        Vector3 knockbackVelocity = knockbackDirection.normalized * knockbackForce;
        GetComponent<Rigidbody>().velocity = knockbackVelocity;

        // Wait for a short duration to apply the knockback effect
        yield return new WaitForSeconds(0.5f);

        // Reset enemy velocity and resume movement
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        isPlayerDetected = true;

        // Reset the knockback coroutine reference
        knockbackCoroutine = null;
    }
}
