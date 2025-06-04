using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource internalAudioSource;
    private AudioClip currentClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            internalAudioSource = gameObject.AddComponent<AudioSource>();
            internalAudioSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayFromSource(AudioSource source)
    {
        if (source == null || source.clip == null)
        {
            return;
        }

        AudioClip newClip = source.clip;

        if (internalAudioSource.clip == newClip && internalAudioSource.isPlaying)
        {
            // ���� Ŭ���̸� ��� ����
            return;
        }

        currentClip = newClip;
        internalAudioSource.clip = newClip;
        internalAudioSource.volume = source.volume;
        internalAudioSource.pitch = source.pitch;

        internalAudioSource.Play();
    }

    public void StopBGM()
    {
        internalAudioSource.Stop();
        currentClip = null;
    }
}
