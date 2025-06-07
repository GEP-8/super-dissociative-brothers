using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LogoMove : MonoBehaviour
{
    public RectTransform logoImage;
    public float floatAmount = 30f;
    public float floatDuration = 1f;
    public float interval = 0.5f;

    private Vector2 originalPos;

    void Start()
    {
        if (logoImage == null)
            logoImage = GetComponent<RectTransform>();

        originalPos = logoImage.anchoredPosition;

        StartCoroutine(FloatingRoutine());
    }

    private IEnumerator FloatingRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(MoveImage(Vector2.up * floatAmount));
            yield return StartCoroutine(MoveImage(Vector2.down * floatAmount));
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator MoveImage(Vector2 offset)
    {
        Vector2 start = logoImage.anchoredPosition;
        Vector2 end = start + offset;
        float elapsed = 0f;

        while (elapsed < floatDuration)
        {
            float t = elapsed / floatDuration;
            float easedT = Mathf.SmoothStep(0f, 1f, t);
            logoImage.anchoredPosition = Vector2.Lerp(start, end, easedT);

            elapsed += Time.deltaTime;
            yield return null;
        }

        logoImage.anchoredPosition = end;
    }
}
