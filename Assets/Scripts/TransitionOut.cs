using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionOut : MonoBehaviour
{
    [SerializeField] private RectTransform blackScreen;
    [SerializeField] private float transitionDuration = 1.0f;
    [SerializeField] private AnimationCurve transitionCurve;

    private bool isTransitioning = false;

    public void StartSceneTransition(string sceneName)
    {
        if (!isTransitioning)
            StartCoroutine(TransitionAndLoadScene(sceneName));
    }
    public void StartSceneTransition(string sceneName, Action onTransitionComplete)
    {
        if (!isTransitioning)
            StartCoroutine(TransitionAndCallback(onTransitionComplete));
    }

    private IEnumerator TransitionAndLoadScene(string sceneName)
    {
        isTransitioning = true;

        yield return PlayTransitionAnimation();

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator TransitionAndCallback(Action onComplete)
    {
        isTransitioning = true;

        yield return PlayTransitionAnimation();

        onComplete?.Invoke();
    }

    private IEnumerator PlayTransitionAnimation()
    {
        Vector2 startPos = blackScreen.anchoredPosition;
        Vector2 endPos = Vector2.zero;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            float curveT = transitionCurve.Evaluate(t);
            blackScreen.anchoredPosition = Vector2.Lerp(startPos, endPos, curveT);
            yield return null;
        }
    }
}
