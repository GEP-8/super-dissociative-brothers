using UnityEngine;
using UnityEngine.SceneManagement;

public class PageController : MonoBehaviour
{
	[SerializeField] private TransitionOut transitionManager;

	public void GoToHowToPlayScene()
	{
		transitionManager.StartSceneTransition("HowToPlayScene_1");
	}

	public void GoToMainScene()
	{
		transitionManager.StartSceneTransition("MainTitleScene");
	}
    public void GoToNetworkConnection()
    {
        transitionManager.StartSceneTransition("StageSelectionScene"); // 프로젝트 머지 시, 네트워크 연결 씬으로 수정 필요
    }

    public void GoToNextScene()
	{
		SceneManager.LoadScene("HowToPlayScene_2");
	}

	public void GoToPreviousScene()
	{
		SceneManager.LoadScene("HowToPlayScene_3");
	}
}