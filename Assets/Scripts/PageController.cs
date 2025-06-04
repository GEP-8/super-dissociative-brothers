using UnityEngine;
using UnityEngine.SceneManagement;

public class PageController : MonoBehaviour
{
    [SerializeField] private string mainSceneName;
    [SerializeField] private string howtoplaySceneName;
    [SerializeField] private string nextSceneName;
    [SerializeField] private string previousSceneName;

    public void GoToHowToPlayScene()
    {
        SceneManager.LoadScene("HowToPlayScene_1");
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene("HowToPlayScene_2");
    }

    public void GoToPreviousScene()
    {
        SceneManager.LoadScene("HowToPlayScene_3");
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("MainTitleScene");
    }
}