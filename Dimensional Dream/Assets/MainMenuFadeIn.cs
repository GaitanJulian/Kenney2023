using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuFadeIn : MonoBehaviour
{
    public float fadeInDuration = 2.5f;
    private float startDelay = 2.0f;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        transform.parent.gameObject.SetActive(true);
        canvasGroup = GetComponent<CanvasGroup>();

        // Set the logo to fully opaque (alpha = 1)
        canvasGroup.alpha = 1f;

        // Use DoTween to fade in the logo's alpha value to 0 over the specified duration
        Invoke("StartFadeIn", startDelay);
    }

    private void StartFadeIn()
    {
        // Use DoTween to fade in the logo's alpha value to 0 over the specified duration
        canvasGroup.DOFade(0f, fadeInDuration).OnComplete(OnFadeInComplete);
    }

    private void OnFadeInComplete()
    {
        // After the fade-in animation is complete, disable the parent object
        transform.parent.gameObject.SetActive(false);
    }
}
