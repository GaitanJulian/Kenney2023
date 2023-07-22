using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWeapon : MonoBehaviour
{
    public GameObject _playerWeapon;
    public GameObject _door;

    private void OnCollisionEnter(Collision collision)
    {
        _playerWeapon.SetActive(true);
        _door.SetActive(false);

        Destroy(gameObject);
    }

}
