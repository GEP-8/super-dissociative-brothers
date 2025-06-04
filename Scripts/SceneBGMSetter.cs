using UnityEngine;

public class SceneBGMSetter : MonoBehaviour
{
    public AudioSource bgmSource;

    void Start()
    {
        if (bgmSource == null)
        {
            return;
        }

        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PlayFromSource(bgmSource);
        }
    }
}