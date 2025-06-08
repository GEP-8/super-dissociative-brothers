using Unity.Netcode;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private TransitionOut transitionManager;

    public void GoToStage1() => TryLoadStage("Stage_1");
    public void GoToStage2() => TryLoadStage("Stage_2");
    public void GoToStage3() => TryLoadStage("Stage_3");
    public void GoToStage4() => TryLoadStage("Stage_4");
    public void GoToStage5() => TryLoadStage("Stage_5");

    private void TryLoadStage(string sceneName)
    {
        if (NetworkManager.Singleton.IsConnectedClient == false)
        {
            Debug.LogWarning("진행하려면 로비에 참가해야 합니다.");
            return;
        }

        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log($"서버에서 {sceneName} 씬 로드 준비 중...");
            StartCoroutine(LoadSceneWithTransition(sceneName));
        }
        else
        {
            Debug.LogWarning("씬 전환은 서버 또는 호스트만 할 수 있습니다.");
        }
    }

    private IEnumerator LoadSceneWithTransition(string sceneName)
    {
        if (transitionManager != null)
        {
            bool isDone = false;

            transitionManager.StartSceneTransition(sceneName, () =>
            {
                isDone = true;
            });

            yield return new WaitUntil(() => isDone);
        }

        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
