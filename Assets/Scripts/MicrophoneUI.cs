using UnityEngine;
using UnityEngine.UI;

public class MicrophoneUI : MonoBehaviour
{
    public Image Mic;
    public string micDevice;
    public float sensitivity = 0.1f;

    private AudioClip micClip;
    private bool micInitialized = false;
    private int sampleWindow = 128;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            micDevice = Microphone.devices[0]; // 첫 번째 마이크 사용
            micClip = Microphone.Start(micDevice, true, 1, 44100);
            micInitialized = true;
        }
        else
        {
            Debug.LogWarning("마이크 디바이스를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (!micInitialized || Mic == null) return;

        float loudness = GetMicLoudness();

        Color currentColor = Mic.color;
        currentColor.a = loudness > sensitivity ? 1f : 0f;
        Mic.color = currentColor;
    }

    float GetMicLoudness()
    {
        float maxLevel = 0f;
        float[] waveData = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(micDevice) - sampleWindow + 1;
        if (micPosition < 0) return 0f;

        micClip.GetData(waveData, micPosition);

        for (int i = 0; i < sampleWindow; ++i)
        {
            float wavePeak = Mathf.Abs(waveData[i]);
            if (wavePeak > maxLevel)
                maxLevel = wavePeak;
        }

        return maxLevel;
    }
}
