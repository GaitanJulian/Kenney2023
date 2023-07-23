using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float floatDistance = 0.5f; // The distance the object will float up and down from its initial position
    public float floatDuration = 2f; // The duration of each up and down float
    public Ease floatEase = Ease.InOutQuad; // The easing function for the float animation

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
        StartFloatingAnimation();
    }

    void StartFloatingAnimation()
    {
        transform.DOMoveY(originalPosition.y + floatDistance, floatDuration).SetEase(floatEase).OnComplete(ReverseFloatingAnimation);
    }

    void ReverseFloatingAnimation()
    {
        transform.DOMoveY(originalPosition.y, floatDuration).SetEase(floatEase).OnComplete(StartFloatingAnimation);
    }
}
