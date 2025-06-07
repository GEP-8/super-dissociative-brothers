using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private TransitionOut transitionManager;
    public void GoToStage1()
    {
        transitionManager.StartSceneTransition("StageScene"); //스테이지 1 씬 이름으로 수정 필요
    }
    public void GoToStage2()
    {
        transitionManager.StartSceneTransition("StageScene"); //스테이지 2 씬 이름으로 수정 필요
    }
    public void GoToStage3()
    {
        transitionManager.StartSceneTransition("StageScene"); //스테이지 3 씬 이름으로 수정 필요
    }
    public void GoToStage4()
    {
        transitionManager.StartSceneTransition("StageScene"); //스테이지 4 씬 이름으로 수정 필요
    }
    public void GoToStage5()
    {
        transitionManager.StartSceneTransition("StageScene"); //스테이지 5 씬 이름으로 수정 필요
    }
}
