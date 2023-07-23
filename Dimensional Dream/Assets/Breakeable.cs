using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakeable : MonoBehaviour
{
    public GameObject padlock;
    public GameObject spotlight;
    public GameObject keyPrefab;
    public LevelManager levelManager;


    public void BreakObject()
    {
        Destroy(padlock);
        Destroy(spotlight);

        // Instantiate the key object at the current position of the broken object
        GameObject keyObject = Instantiate(keyPrefab, transform.position, Quaternion.identity);

        // Use DoTween to animate the key object's position upwards
        Vector3 targetPosition = transform.position + Vector3.up * 1.5f; // Adjust the 2f value as per your desired height
        float animationDuration = 1f; // Adjust the duration as per your desired speed

        keyObject.transform.DOMove(targetPosition, animationDuration)
            .OnComplete(() => DestroyKey(keyObject));

        levelManager.KeyFound();
        Destroy(gameObject);
    }

    private void DestroyKey(GameObject keyObject)
    {
        // Destroy the key object after the animation is complete
        Destroy(keyObject);
    }
}
