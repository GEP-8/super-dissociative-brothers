using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    public Material transitionMaterial;
    public RawImage transitionImage;
    public float transitionDuration = 1f;

    private static TransitionController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        transitionImage.material = new Material(transitionMaterial);
        transitionImage.material.SetFloat("_Progress", 0f);
        transitionImage.enabled = false;
    }

    public void StartTransition(string sceneName)
    {
        transitionImage.enabled = true;
        StartCoroutine(PlayTransition(sceneName));
    }

    private IEnumerator PlayTransition(string sceneName)
    {
        transitionImage.enabled = true;

        Material mat = transitionImage.material;
        float time = 0f;

        while (time < transitionDuration)
        {
            float progress = Mathf.Lerp(0f, 1f, time / transitionDuration);
            mat.SetFloat("_Progress", progress);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        mat.SetFloat("_Progress", 1f);
        yield return new WaitForSecondsRealtime(0.1f);

        SceneManager.LoadScene(sceneName);
    }
}
