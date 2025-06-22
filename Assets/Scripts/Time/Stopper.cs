using UnityEngine;

public class Stopper : MonoBehaviour
{
    void Awake()
    {
        // Reset time scale
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
    }

    void StopGame()
    {
        Time.timeScale = 0f; // stop game process
    }


    void OnEnable()
    {
        PlayerCollisionHandler.OnGameStopped += StopGame;
    }

    void OnDisable()
    {
        PlayerCollisionHandler.OnGameStopped -= StopGame;
    }
}
