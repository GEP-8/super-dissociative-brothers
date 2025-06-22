using UnityEngine;
using System.Collections;

public class TextMove : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 centerPos;
    public Vector2 endPos;
    public float moveDuration = 2f;
    public float startWaitTime = 1f;
    public float waitTime = 5f;
    public AnimationCurve easeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve easeOutCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = startPos;
        StartCoroutine(MoveSequence());
    }

    IEnumerator MoveSequence()
    {
        yield return new WaitForSecondsRealtime(startWaitTime);
        yield return StartCoroutine(MoveUITo(centerPos, moveDuration, easeInCurve));
        yield return new WaitForSecondsRealtime(waitTime);
        yield return StartCoroutine(MoveUITo(endPos, moveDuration, easeOutCurve));
    }

    IEnumerator MoveUITo(Vector2 target, float duration, AnimationCurve easingCurve)
    {
        Vector2 initial = rectTransform.anchoredPosition;
        float time = 0f;
        float lastDt = 0f;

        while (time < duration)
        {
            // 부드럽게 보정된 deltaTime
            float dt = Mathf.Lerp(lastDt, Time.unscaledDeltaTime, 0.5f);
            lastDt = dt;

            time += dt;
            float t = Mathf.Clamp01(time / duration);
            float easedT = easeInCurve.Evaluate(t);
            rectTransform.anchoredPosition = Vector2.Lerp(initial, target, easedT);
            yield return null;
        }

        rectTransform.anchoredPosition = target;
    }
}
