using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public Enemies _enemy;

    private void Start()
    {
        _enemy = GetComponentInParent<Enemies>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _enemy.PlayerDetected();
        }
    }
}
