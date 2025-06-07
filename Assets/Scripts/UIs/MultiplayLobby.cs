using Unity.Netcode;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayLobby : MonoBehaviour
{
    [SerializeField] private TransitionOut transitionManager;
    public void GoStageSelectScene()
    {
        
        if (NetworkManager.Singleton.IsConnectedClient == false) {
            Debug.LogWarning("진행하려면 로비에 참가해야 합니다.");

            return;
        }
        
        if (NetworkManager.Singleton.IsServer) // 또는 IsHost
        {
            // 서버 또는 호스트만 씬 전환을 시작할 수 있습니다.
            Debug.Log($"서버에서 StageSelectionScene 씬 로드 시작.");
            NetworkManager.Singleton.SceneManager.LoadScene("StageSelectionScene", LoadSceneMode.Single);
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
        transitionManager.StartSceneTransition("MainTitleScene"); //�������� 1 �� �̸����� ���� �ʿ�
    }
}
