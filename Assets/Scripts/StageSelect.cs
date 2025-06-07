using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private TransitionOut transitionManager;
    public void GoToStage1()
    {
        transitionManager.StartSceneTransition("Stage_1"); //�������� 1 �� �̸����� ���� �ʿ�
    }
    public void GoToStage2()
    {
        transitionManager.StartSceneTransition("Stage_2"); //�������� 2 �� �̸����� ���� �ʿ�
    }
    public void GoToStage3()
    {
        transitionManager.StartSceneTransition("Stage_3"); //�������� 3 �� �̸����� ���� �ʿ�
    }
    public void GoToStage4()
    {
        transitionManager.StartSceneTransition("Stage_4"); //�������� 4 �� �̸����� ���� �ʿ�
    }
    public void GoToStage5()
    {
        transitionManager.StartSceneTransition("Stage_5"); //�������� 5 �� �̸����� ���� �ʿ�
    }
}
