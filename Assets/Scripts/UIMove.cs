using UnityEngine;
using System.Collections;

public class UIMove : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 endPos;
    public float moveDuration = 2f;
    public float waitTime = 10f;
    public AnimationCurve easeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = startPos;
        StartCoroutine(MoveSequence());
    }

    IEnumerator MoveSequence()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        yield return StartCoroutine(MoveUITo(endPos, moveDuration, easeInCurve));
    }

    IEnumerator MoveUITo(Vector2 target, float duration, AnimationCurve easingCurve)
    {
        Vector2 initial = rectTransform.anchoredPosition;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / duration);
            float easedT = easeInCurve.Evaluate(t);
            rectTransform.anchoredPosition = Vector2.Lerp(initial, target, easedT);
            yield return null;
        }

        rectTransform.anchoredPosition = target;
    }
}
