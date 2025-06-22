using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public void GoToStageSelect()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("StageSelectionScene", LoadSceneMode.Single);
        }
    }

    public void GoToStage1()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Stage_1", LoadSceneMode.Single);
        }
    }

    public void GoToStage2()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Stage_2", LoadSceneMode.Single);
        }
    }

    public void GoToStage3()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Stage_3", LoadSceneMode.Single);
        }
    }

    public void GoToStage4()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Stage_4", LoadSceneMode.Single);
        }
    }

    public void GoToStage5()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Stage_5", LoadSceneMode.Single);
        }
    }
}
