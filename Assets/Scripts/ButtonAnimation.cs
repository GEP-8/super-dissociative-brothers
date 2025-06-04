using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ButtonAnimation : MonoBehaviour
{
    private Vector3 originalScale;
    private Coroutine scaleCoroutine;
    private Coroutine colorCoroutine;

    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float clickScale = 0.9f;
    [SerializeField] private float animationSpeed = 10f;

    private bool isHovered = false;
    private bool isClicked = false;

    private RectTransform rectTransform;
    private Canvas canvas;

    private Image buttonImage;
    private Color originalColor;
    [SerializeField] private Color clickColor = new Color(0.7f, 0.7f, 0.7f);

    private void Awake()
    {
        originalScale = transform.localScale;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // Image 컴포넌트 가져오기
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
    }

    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        bool isMouseInside = RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform, mousePos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera
        );

        if (isMouseInside)
        {
            if (!isHovered)
            {
                isHovered = true;
                StartScaleAnimation(originalScale * hoverScale);
            }

            if (Mouse.current.leftButton.isPressed)
            {
                if (!isClicked)
                {
                    isClicked = true;
                    StartScaleAnimation(originalScale * clickScale);
                    StartColorAnimation(clickColor);
                }
            }
            else if (isClicked)
            {
                isClicked = false;
                StartScaleAnimation(originalScale * hoverScale);
                StartColorAnimation(originalColor);
            }
        }
        else
        {
            if (isHovered)
            {
                isHovered = false;
                isClicked = false;
                StartScaleAnimation(originalScale);
                StartColorAnimation(originalColor);
            }
        }
    }

    private void StartScaleAnimation(Vector3 targetScale)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(ScaleTo(targetScale));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.001f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
            yield return null;
        }
        transform.localScale = targetScale;
    }

    private void StartColorAnimation(Color targetColor)
    {
        if (buttonImage == null) return;

        if (colorCoroutine != null)
            StopCoroutine(colorCoroutine);

        colorCoroutine = StartCoroutine(ColorLerpTo(targetColor));
    }

    private IEnumerator ColorLerpTo(Color targetColor)
    {
        Color startColor = buttonImage.color;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * animationSpeed;
            buttonImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        buttonImage.color = targetColor;
    }
}
