using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    public float fadeDuration = 3.0f; // The duration of the fade animation in seconds
    public float fadeInDuration = 5.0f;

    private Image overlayImage;

    void Start()
    {
        overlayImage = GetComponentInChildren<Image>();

        // Start the fade animation
        FadeOutScreen();
    }

    private void FadeOutScreen()
    {
        // Set the overlay color to full white
        overlayImage.color = Color.white;

        // Use DoTween to fade out the overlay's alpha value to 0 over the specified duration
        overlayImage.DOFade(0.0f, fadeDuration).OnComplete(DisableOverlay);
    }

    private void DisableOverlay()
    {
        // After the fade animation is complete, disable the overlay canvas to interact with the scene
        overlayImage.gameObject.SetActive(false);
    }

    public void FadeInWhite()
    {
        overlayImage.gameObject.SetActive(true);

        // Set the overlay color to fully transparent (clear)
        overlayImage.color = new Color(1f, 1f, 1f, 0f);

        // Use DoTween to fade in the overlay's alpha value to 1 over the specified duration
        overlayImage.DOFade(1.0f, fadeInDuration).OnComplete(OnFadeInWhiteComplete);
    }

    private void OnFadeInWhiteComplete()
    {
        // After the fadeInWhite animation is complete, initiate the scene change
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        // Load the next scene
        SceneManager.LoadScene("CreditsScene");
    }
}
