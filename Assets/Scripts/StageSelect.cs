using Unity.Netcode;

using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private TransitionOut transitionManager;
    public void GoToStage1()
    {
        GoToScene("Stage_1"); //�������� 1 �� �̸����� ���� �ʿ�
    }
    public void GoToStage2()
    {
        GoToScene("Stage_2"); //�������� 2 �� �̸����� ���� �ʿ�
    }
    public void GoToStage3()
    {
        GoToScene("Stage_3"); //�������� 3 �� �̸����� ���� �ʿ�
    }
    public void GoToStage4()
    {
        GoToScene("Stage_4"); //�������� 4 �� �̸����� ���� �ʿ�
    }
    public void GoToStage5()
    {
        GoToScene("Stage_5"); //�������� 5 �� �̸����� ���� �ʿ�
    }

    private void GoToScene(string sceneName) {
        
        if (NetworkManager.Singleton.IsConnectedClient == false) {
            Debug.LogWarning("진행하려면 로비에 참가해야 합니다.");

            return;
        }
        
        if (NetworkManager.Singleton.IsServer) // 또는 IsHost
        {
            // 서버 또는 호스트만 씬 전환을 시작할 수 있습니다.
            Debug.Log($"서버에서 {sceneName} 씬 로드 시작.");
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning("씬 전환은 서버 또는 호스트만 할 수 있습니다.");
        }
    }
}
