using Unity.Netcode;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool debugMode = false;

    void OnCollisionEnter2D(Collision2D other)
    {
        // 서버에서만 호출
        if (!NetworkManager.Singleton.IsServer) return;

        if (other.gameObject.CompareTag("Player"))
        {
            NetworkObject playerNetObj = other.gameObject.GetComponent<NetworkObject>();

            if (playerNetObj != null && playerNetObj.IsSpawned)
            {
                if(debugMode) Debug.Log($"Player {other.gameObject.name} parented with platform {gameObject.name}");

                // 네트워크 객체의 부모 변경 요청
                playerNetObj.TrySetParent(transform, worldPositionStays: true);
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        // 서버에서만 호출
        if (!NetworkManager.Singleton.IsServer) return;

        if (other.gameObject.CompareTag("Player"))
        {
            NetworkObject playerNetObj = other.gameObject.GetComponent<NetworkObject>();

            if (playerNetObj != null && playerNetObj.IsSpawned)
            {
                if(debugMode) Debug.Log($"Player {other.gameObject.name} unparented with platform {gameObject.name}");

                // 부모 관계 제거
                playerNetObj.TrySetParent((Transform)null, worldPositionStays: true);
            }
        }
    }
}
