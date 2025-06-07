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
        transitionManager.StartSceneTransition("LobbyScene"); // ������Ʈ ���� ��, ��Ʈ��ũ ���� ������ ���� �ʿ�
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