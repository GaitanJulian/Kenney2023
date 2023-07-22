using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    public float minSpeed = 30f; // Minimum rotation speed for each axis (degrees per second)
    public float maxSpeed = 50f; // Maximum rotation speed for each axis (degrees per second)

    private Vector3 rotationSpeeds;

    private void OnEnable()
    {
        GenerateRandomRotationSpeeds();
    }

    private void OnDisable()
    {
        // Kill any active tweens when the object is disabled
        transform.DOKill();
    }

    private void GenerateRandomRotationSpeeds()
    {
        float randomSpeedX = Random.Range(minSpeed, maxSpeed);
        float randomSpeedY = Random.Range(minSpeed, maxSpeed);
        float randomSpeedZ = Random.Range(minSpeed, maxSpeed);

        Vector3 rotationSpeeds = new Vector3(randomSpeedX, randomSpeedY, randomSpeedZ);

        RotateIndefinitely(rotationSpeeds);
    }

    private void RotateIndefinitely(Vector3 rotationSpeeds)
    {
        transform.Rotate(rotationSpeeds * Time.deltaTime);
        DOTween.Sequence().AppendCallback(() => RotateIndefinitely(rotationSpeeds));
    }
}
