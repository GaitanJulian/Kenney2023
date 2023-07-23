using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleted : MonoBehaviour
{
    public string nextSceneName; // The name of the next scene to load after completion

    private TransitionScript transitionScript;

    void Start()
    {
        // Get the TransitionScript component attached to the GameObject with the TransitionScript
        transitionScript = FindObjectOfType<TransitionScript>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the fade animation when the player enters the trigger collider
            transitionScript.gameObject.SetActive(true);

            // Start the fadeInWhite animation
            transitionScript.FadeInWhite();
        }
    }
}