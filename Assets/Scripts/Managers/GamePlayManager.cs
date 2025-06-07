// using Unity.Netcode;
//
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// namespace Managers {
//     public class GamePlayManager : MonoBehaviour {
//
//         [SerializeField] private GameObject inputHandlerPrefab;
//
//         // NetworkManager의 SceneManager가 씬 로딩을 완료했을 때 호출됩니다.
//         private void OnEnable()
//         {
//             NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoadComplete;
//         }
//
//         private void OnDisable()
//         {
//             if (NetworkManager.Singleton != null) // 씬이 파괴될 때 NetworkManager가 이미 null일 수 있으므로 null 체크
//             {
//                 NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnSceneLoadComplete;
//             }
//         }
//
//         private void OnSceneLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
//         {
//             // 이 콜백은 모든 클라이언트가 씬 로드를 완료했을 때 서버에서 호출됩니다.
//             // 그리고 해당 클라이언트의 ID와 로드된 씬 이름이 전달됩니다.
//
//             // 게임 씬으로 전환이 완료되었고, 이 콜백이 서버(또는 호스트)에서 실행될 때만 플레이어를 스폰합니다.
//             if (NetworkManager.Singleton.IsServer)
//             {
//                 // 플레이어가 아직 스폰되지 않았다면 스폰 (재연결 시 중복 스폰 방지 로직 추가 가능)
//                 // 각 클라이언트(clientId)에 대해 플레이어를 스폰합니다.
//                 // 이 예시에서는 플레이어 프리팹을 인스턴스화하고, NetworkObject를 가져와 소유권을 부여하고 스폰합니다.
//                 GameObject inputHandler = Instantiate(inputHandlerPrefab);
//                 NetworkObject playerNetworkObject = inputHandler.GetComponent<NetworkObject>();
//
//                 // 해당 클라이언트 ID에 플레이어 소유권을 부여하고 스폰합니다.
//                 playerNetworkObject.SpawnAsPlayerObject(clientId);
//
//                 Debug.Log($"서버가 클라이언트 {clientId}를 위한 플레이어 '{inputHandlerPrefab.name}'를 스폰했습니다.");
//             }
//         }
//     }
// }