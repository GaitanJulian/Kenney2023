using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform _checkPoint;
    private bool _checked = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_checked)
        {
            other.GetComponent<TopDownViewCharacterController>().SetCheckPoint(_checkPoint);
            _checked = true;
        }
    }
}
