using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisionManager : MonoBehaviour
{
    public List<RawImage> playerViews; // 인스펙터에서 미리 RawImage를 4개 등록
    public List<QuadrantVisionController> visionControllers; // 인스펙터에서 미리 QuadrantVisionController를 4개 등록
    public float sharedRadius = 0.15f;
    private int myIndex;

    public void SetupVision(int playerCount)
    {
        switch (playerCount)
        {
            case 1:
                SetRectTransform(playerViews[0].rectTransform, new Vector2(0, 0), new Vector2(1, 1));
                playerViews[0].uvRect = new Rect(0, 0, 1, 1);
                //ActivateViews(1);
                break;

            case 2:
                // Player 1 (좌측)
                SetRectTransform(playerViews[0].rectTransform, new Vector2(0, 0), new Vector2(0.5f, 1));
                playerViews[0].uvRect = new Rect(0, 0, 0.5f, 1);
                // Player 2 (우측)
                SetRectTransform(playerViews[1].rectTransform, new Vector2(0.5f, 0), new Vector2(1, 1));
                playerViews[1].uvRect = new Rect(0.5f, 0, 0.5f, 1);
                //ActivateViews(2);
                break;

            case 3:
                // Player 1 (좌상)
                SetRectTransform(playerViews[0].rectTransform, new Vector2(0, 0.5f), new Vector2(0.5f, 1));
                playerViews[0].uvRect = new Rect(0, 0.5f, 0.5f, 0.5f);
                // Player 2 (우상)
                SetRectTransform(playerViews[1].rectTransform, new Vector2(0.5f, 0.5f), new Vector2(1, 1));
                playerViews[1].uvRect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                // Player 3 (하단 전체 - 2영역)
                SetRectTransform(playerViews[2].rectTransform, new Vector2(0, 0), new Vector2(1, 0.5f));
                playerViews[2].uvRect = new Rect(0, 0, 1, 0.5f);
                //ActivateViews(3);
                break;

            case 4:
                // 좌상, 우상, 좌하, 우하
                SetRectTransform(playerViews[0].rectTransform, new Vector2(0, 0.5f), new Vector2(0.5f, 1));
                playerViews[0].uvRect = new Rect(0, 0.5f, 0.5f, 0.5f);

                SetRectTransform(playerViews[1].rectTransform, new Vector2(0.5f, 0.5f), new Vector2(1, 1));
                playerViews[1].uvRect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);

                SetRectTransform(playerViews[2].rectTransform, new Vector2(0, 0), new Vector2(0.5f, 0.5f));
                playerViews[2].uvRect = new Rect(0, 0, 0.5f, 0.5f);

                SetRectTransform(playerViews[3].rectTransform, new Vector2(0.5f, 0), new Vector2(1, 0.5f));
                playerViews[3].uvRect = new Rect(0.5f, 0, 0.5f, 0.5f);

                //ActivateViews(4);
                break;
        }

        // 현재 플레이어의 인덱스를 가져옴
        ActivateViews(myIndex);

        // SplitMode를 각 컨트롤러의 Material에 직접 전달
        foreach (var ctrl in visionControllers)
        {
            if (ctrl && ctrl.quadrantMaterial)
            {
                ctrl.quadrantMaterial.SetFloat("_SplitMode", playerCount);
                ctrl.quadrantMaterial.SetFloat("_SharedRadius", sharedRadius);
            }
        }
    }

    void SetRectTransform(RectTransform rt, Vector2 anchorMin, Vector2 anchorMax)
    {
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
    }

    void ActivateViews(int myIndex)
    {
        for (int i = 0; i < playerViews.Count; i++)
        {
            // playerViews[i].gameObject.SetActive(i < count);
            playerViews[i].gameObject.SetActive(i == myIndex);
        }
    }

    int GetMyPlayerIndex()
    {
        var myId = NetworkManager.Singleton.LocalClientId;
        var keys = new List<ulong>(NetworkManager.Singleton.ConnectedClients.Keys);
        keys.Sort();
        return keys.IndexOf(myId);
    }

    void Start()
    {
        int playerCount = NetworkManager.Singleton.ConnectedClients.Count;
        Debug.Log($"Connected Players: {playerCount}");
        GetComponent<PlayerVisionManager>().SetupVision(playerCount);

        int myIndex = GetMyPlayerIndex();
        Debug.Log($"My Player Index: {myIndex}");
    }

}
