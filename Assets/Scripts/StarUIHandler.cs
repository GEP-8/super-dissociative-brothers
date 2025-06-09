using UnityEngine;

public class StarUIHandler : MonoBehaviour
{
    [Header("Star UI 오브젝트 (Flag용)")]
    public GameObject star1;    // 활성 상태 Star_1
    public GameObject oStar1;   // 비활성 상태 OStar_1

    [Header("Star UI 오브젝트 (Pill용)")]
    public GameObject star2;    // 활성 상태 Star_2
    public GameObject oStar2;   // 비활성 상태 OStar_2

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
