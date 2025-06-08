using Unity.Netcode;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayLobby : MonoBehaviour
{
    [SerializeField] private TransitionOut transitionManager;
    public void GoStageSelectScene()
    {
        if (NetworkManager.Singleton.IsConnectedClient == false)
        {
            Debug.LogWarning("진행하려면 로비에 참가해야 합니다.");
            return;
        }

        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log($"서버에서 StageSelectionScene 씬 로드 준비 중...");

            // 트랜지션 + 씬 로딩 수행
            StartCoroutine(LoadSceneWithTransition("StageSelectionScene"));
        }
        else
        {
            Debug.LogWarning("씬 전환은 서버 또는 호스트만 할 수 있습니다.");
        }
    }
    public void GoToMainScene()
    {
        if (NetworkManager.Singleton.IsConnectedClient) {
            Debug.LogWarning("타이틀로 가려면 로비에서 나와야 합니다.");

            return;
        }
        transitionManager.StartSceneTransition("MainTitleScene");
    }

    private IEnumerator LoadSceneWithTransition(string sceneName)
    {
        if (transitionManager != null)
        {
            bool isDone = false;

            // 콜백을 통해 트랜지션 완료 후 로딩
            transitionManager.StartSceneTransition(sceneName, () =>
            {
                isDone = true;
            });

            // 콜백 완료 대기
            yield return new WaitUntil(() => isDone);
        }

        // 트랜지션 완료 후 네트워크 씬 로딩
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
