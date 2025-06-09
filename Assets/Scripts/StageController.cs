using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    public void GoToStageSelect()
    {
        Debug.Log("¤©¤©¤©¤©");
        NetworkManager.Singleton.SceneManager.LoadScene("StageSelectionScene", LoadSceneMode.Single);
    }

    public void GoToStage1()
    {
        Debug.Log("¤©¤©¤©¤©");
        NetworkManager.Singleton.SceneManager.LoadScene("Stage_1", LoadSceneMode.Single);
    }
    public void GoToStage2()
    {
        Debug.Log("¤©¤©¤©¤©");
        NetworkManager.Singleton.SceneManager.LoadScene("Stage_2", LoadSceneMode.Single);
    }
    public void GoToStage3()
    {
        Debug.Log("¤©¤©¤©¤©");
        NetworkManager.Singleton.SceneManager.LoadScene("Stage_3", LoadSceneMode.Single);
    }
    public void GoToStage4()
    {
        Debug.Log("¤©¤©¤©¤©");
        NetworkManager.Singleton.SceneManager.LoadScene("Stage_4", LoadSceneMode.Single);
    }
    public void GoToStage5()
    {
        Debug.Log("¤©¤©¤©¤©");
        NetworkManager.Singleton.SceneManager.LoadScene("Stage_5", LoadSceneMode.Single);
    }
}

