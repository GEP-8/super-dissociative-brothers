using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    [SerializeField] private TransitionOut transitionManager;

    public void GoToStageSelect()
    {
        Debug.Log("¿Ãµø");
        SceneManager.LoadScene("StageSelectionScene");
    }

    public void GoToStage1()
    {
        transitionManager.StartSceneTransition("Stage_1");
    }
    public void GoToStage2()
    {
        transitionManager.StartSceneTransition("Stage_2");
    }
    public void GoToStage3()
    {
        transitionManager.StartSceneTransition("Stage_3");
    }
    public void GoToStage4()
    {
        transitionManager.StartSceneTransition("Stage_4");
    }
    public void GoToStage5()
    {
        transitionManager.StartSceneTransition("Stage_5");
    }
}
