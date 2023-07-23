using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SceneLoader : MonoBehaviour
{
    public string _nextSceneName;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !SceneManager.GetSceneByName(_nextSceneName).isLoaded)
        {
            SceneManager.LoadScene(_nextSceneName, LoadSceneMode.Additive);
        }
    }
}
