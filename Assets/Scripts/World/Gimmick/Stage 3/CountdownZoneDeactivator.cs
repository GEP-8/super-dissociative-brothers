using UnityEngine;

public class CountdownZoneDeactivator : MonoBehaviour
{    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 트리거 영역에 들어오면 카운트다운 비활성화
            CountdownTriggerZone countdownZone = FindAnyObjectByType<CountdownTriggerZone>();
            if (countdownZone != null)
            {
                countdownZone.Deactivate();
            }
        }
    }
}
