using UnityEngine;

public class StarUIHandler : MonoBehaviour
{
    [Header("Star UI ������Ʈ (Flag��)")]
    public GameObject star1;    // Ȱ�� ���� Star_1
    public GameObject oStar1;   // ��Ȱ�� ���� OStar_1

    [Header("Star UI ������Ʈ (Pill��)")]
    public GameObject star2;    // Ȱ�� ���� Star_2
    public GameObject oStar2;   // ��Ȱ�� ���� OStar_2

    private void OnEnable()
    {
        PlayerCollisionHandler.OnPillCollected += HandlePillStar;
        PlayerCollisionHandler.OnFlagReached += HandleFlagStar;
    }

    private void OnDisable()
    {
        PlayerCollisionHandler.OnPillCollected -= HandlePillStar;
        PlayerCollisionHandler.OnFlagReached -= HandleFlagStar;
    }

    private void HandleFlagStar()
    {
        if (star1 != null) star1.SetActive(false);
        if (oStar1 != null) oStar1.SetActive(true);
    }

    private void HandlePillStar()
    {
        if (star2 != null) star2.SetActive(false);
        if (oStar2 != null) oStar2.SetActive(true);
    }
}
