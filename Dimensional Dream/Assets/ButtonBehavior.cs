using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float fadeDuration = 0.2f;
    public string Scene;
    private Image buttonImage;
    private Color originalColor;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Apply a hover effect when the pointer hovers over the button
        buttonImage.DOColor(originalColor * 0.8f, fadeDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset the button color when the pointer exits the button
        buttonImage.DOColor(originalColor, fadeDuration);
    }

    public void LoadMainGameScene()
    {
        // Load the main game scene when the button is clicked
        SceneManager.LoadScene(Scene);
    }
}

