using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float movementSpeed = 2f;
    public float detectionRange = 5f;
    public float knockbackForce = 5f;
    public float KnockbackDistance = 0.5f;

    private Transform player;
    private bool isPlayerDetected = false;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = true;
        }
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

        if (distanceToPlayer > KnockbackDistance)
        {
            directionToPlayer.Normalize();
            transform.position += directionToPlayer * movementSpeed * Time.deltaTime;
        }
        else
        {
            // Apply knockback to the player
            Vector3 knockbackDirection = directionToPlayer;
            knockbackDirection.y = 0f;
            player.GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
        }


}
