using Unity.Netcode;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NetworkObject playerNetObj = other.gameObject.GetComponent<NetworkObject>();

            if (playerNetObj != null)
            {
                // 네트워크 객체에 대한 부모 설정 (서버에서만 호출)
                if (playerNetObj.IsSpawned && playerNetObj.IsOwner)
                {
                    Debug.Log($"Player {other.gameObject.name} collided with platform {gameObject.name}");
                    // 플랫폼 위치를 기준으로 상대적 위치 저장
                    Vector3 localPos = transform.InverseTransformPoint(other.transform.position);

                    // 네트워크 객체의 부모 변경 요청
                    playerNetObj.TrySetParent(transform, worldPositionStays: false);

                    // 저장한 상대적 위치 복원
                    other.transform.localPosition = localPos;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NetworkObject playerNetObj = other.gameObject.GetComponent<NetworkObject>();

            if (playerNetObj != null)
            {
                // 네트워크 객체의 부모 제거 (서버에서만 호출)
                if (playerNetObj.IsSpawned && playerNetObj.IsOwner)
                {
                    // 월드 위치 저장
                    Vector3 worldPos = other.transform.position;

                    // 부모 관계 제거
                    playerNetObj.TrySetParent((Transform)null);

                    // 동일한 월드 위치 유지
                    other.transform.position = worldPos;
                }
            }
        }
    }
}
