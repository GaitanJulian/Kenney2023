using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int keys;
    public GameObject spotlight;
    public GameObject completedZone;
    public GameObject gate;

    private bool completed = false;
    
    public void KeyFound()
    {
        keys--;
        if (keys <= 0 && !completed)
        {
            LevelCompleted();
            completed = true;
        }
    }

    public void LevelCompleted()
    {
        spotlight.SetActive(true);
        completedZone.SetActive(true);
        gate.SetActive(false);
    }
}
