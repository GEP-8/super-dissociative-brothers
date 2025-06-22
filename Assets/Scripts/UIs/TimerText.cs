using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
    public Text timerText;

    [Header("Clear Time Condition")]
    public float clearTime = 60f; // Example clear time condition in seconds
    private float elapsedTime = 0f;

    public static event Action OnClearTimeConditionMet;

    void OnEnable()
    {
        PlayerCollisionHandler.OnFlagReached += CheckClearTime;
    }

    void OnDisable()
    {
        PlayerCollisionHandler.OnFlagReached -= CheckClearTime;
    }

    private void CheckClearTime()
    {
        if (elapsedTime <= clearTime)
        {
            // Handle the case where the player cleared the level within the time limit
            Debug.Log("Level cleared within the time limit!");
            
            OnClearTimeConditionMet?.Invoke();
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimerDisplay(elapsedTime);
    }

    void UpdateTimerDisplay(float time)
    {
        int min = Mathf.FloorToInt(time / 60F);
        int sec = Mathf.FloorToInt(time % 60F);
        timerText.text = min.ToString("D2") + ":" + sec.ToString("D2");
    }
}
