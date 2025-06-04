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

	// �Ʒ��� ���� ��� ����
	public void GoToNextScene()
	{
		SceneManager.LoadScene("HowToPlayScene_2");
	}

	public void GoToPreviousScene()
	{
		SceneManager.LoadScene("HowToPlayScene_3");
	}
}