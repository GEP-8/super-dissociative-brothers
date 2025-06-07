using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private TransitionOut transitionManager;
    public void GoToStage1()
    {
        transitionManager.StartSceneTransition("StageScene"); //�������� 1 �� �̸����� ���� �ʿ�
    }
    public void GoToStage2()
    {
        transitionManager.StartSceneTransition("StageScene"); //�������� 2 �� �̸����� ���� �ʿ�
    }
    public void GoToStage3()
    {
        transitionManager.StartSceneTransition("StageScene"); //�������� 3 �� �̸����� ���� �ʿ�
    }
    public void GoToStage4()
    {
        transitionManager.StartSceneTransition("StageScene"); //�������� 4 �� �̸����� ���� �ʿ�
    }
    public void GoToStage5()
    {
        transitionManager.StartSceneTransition("StageScene"); //�������� 5 �� �̸����� ���� �ʿ�
    }
}
