using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisionManager : MonoBehaviour
{
    public RawImage fullScreenView; // 전체 화면을 차지하는 단일 RawImage
    public QuadrantVisionController visionController; // 단일 컨트롤러
    public float sharedRadius = 0.15f;

    private int myPlayerIndex;
    private int playerCount;

    void Start()
    {
        playerCount = NetworkManager.Singleton.ConnectedClients.Count;
        Debug.Log($"Connected Players: {playerCount}");

        myPlayerIndex = GetMyPlayerIndex();
        Debug.Log($"My Player Index: {myPlayerIndex}");

        SetupVision();
    }

    void SetupVision()
    {
        // 전체 화면 설정
        SetRectTransform(fullScreenView.rectTransform, new Vector2(0, 0), new Vector2(1, 1));
        fullScreenView.uvRect = new Rect(0, 0, 1, 1);

        // 비전 컨트롤러 설정
        if (visionController && visionController.quadrantMaterial)
        {
            // 플레이어 인덱스 기반으로 Quadrant 설정 (1부터 시작)
            visionController.quadrantIndex = myPlayerIndex + 1;

            // 현재 플레이어 수에 따른 분할 모드 설정
            visionController.quadrantMaterial.SetFloat("_SplitMode", playerCount);
            visionController.quadrantMaterial.SetFloat("_SharedRadius", sharedRadius);
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

    int GetMyPlayerIndex()
    {
        var myId = NetworkManager.Singleton.LocalClientId;
        var keys = new List<ulong>(NetworkManager.Singleton.ConnectedClients.Keys);
        keys.Sort();
        return keys.IndexOf(myId);
    }
}
