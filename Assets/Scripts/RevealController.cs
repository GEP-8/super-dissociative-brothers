using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RevealController : MonoBehaviour
{
    public Material transitionMaterial;
    public float duration = 2f;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private void Start()
    {
        StartCoroutine(RevealTransition());
    }

    IEnumerator RevealTransition()
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            float cutoff = curve.Evaluate(t);
            transitionMaterial.SetFloat("_Cutoff", cutoff);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
