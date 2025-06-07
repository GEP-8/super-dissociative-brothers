using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayLobby : MonoBehaviour
{
    [SerializeField] private TransitionOut transitionManager;
    public void GoToMainScene()
    {
        transitionManager.StartSceneTransition("MainTitleScene");
    }
    public void GoStageSelectScene()
    {
        transitionManager.StartSceneTransition("StageSelectionScene"); //�������� 1 �� �̸����� ���� �ʿ�
    }
}
