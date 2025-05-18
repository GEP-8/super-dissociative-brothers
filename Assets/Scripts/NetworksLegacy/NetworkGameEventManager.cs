// using System;
// using System.Collections.Generic;
// using Unity.Netcode;
// using UnityEngine;
//
// namespace Networks {
//     public class NetworkGameEventManager : NetworkBehaviour {
//         private static NetworkGameEventManager _instance;
//
//         private readonly Queue<INetworkEvent> _eventQueue = new();
//
//         // <event 타입, List<사용할 수 있는 Client id>>
//         private readonly Dictionary<Type, List<ulong>> _allowedClients = new();
//
//         // TODO: history?
//
//
//         private readonly HashSet<Type> _availableEventsSet = new() {
//             typeof(UpArrowEvent),
//             typeof(DownArrowEvent),
//             typeof(RightArrowEvent),
//             typeof(LeftArrowEvent)
//         };
//
//         public static NetworkGameEventManager Instance {
//             get {
//                 if (_instance == null) {
//                     _instance = FindFirstObjectByType<NetworkGameEventManager>();
//                     if (_instance == null) {
//                         GameObject singletonObject = new(nameof(NetworkGameEventManager));
//                         _instance = singletonObject.AddComponent<NetworkGameEventManager>();
//                         DontDestroyOnLoad(singletonObject);
//                     }
//                 }
//
//                 return _instance;
//             }
//         }
//
//         private void Awake() {
//             if (_instance != null && _instance != this) {
//                 Destroy(gameObject);
//                 return;
//             }
//
//             _instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//
//
//         // --- 이벤트 호출 로직 ---
//
//         // 클라이언트에서 서버로 이벤트를 요청하는 RPC
//         // [ServerRpc]는 클라이언트가 호출하면 서버에서 실행되는 함수
//         [ServerRpc(RequireOwnership = false)] // 이 NetworkBehaviour의 소유권이 없어도 호출 가능
//         public void InvokeEventServerRpc(UpArrowEvent[] networkEventData, ServerRpcParams rpcParams = default) {
//             ulong invokedClientId = rpcParams.Receive.SenderClientId;
//             Type eventType = networkEventData.GetType();
//
//             Debug.Log($"클라이언트 {invokedClientId}가 {eventType.Name} 이벤트 요청.");
//
//             // 1. 이벤트 타입이 _availableEventTypes에 등록되어 있는지 확인
//             if (!_availableEventsSet.Contains(eventType)) {
//                 Debug.LogWarning($"Unknown event type '{eventType.Name}' requested by client {invokedClientId}.");
//                 return; // 알 수 없는 이벤트 타입은 무시
//             }
//
//             // 2. 해당 클라이언트가 이벤트를 Invoke할 권한이 있는지 확인
//             if (!_allowedClients.TryGetValue(eventType, out List<ulong> allowedClientList) ||
//                 !allowedClientList.Contains(invokedClientId)) {
//                 Debug.LogWarning(
//                     $"Client {invokedClientId} is NOT allowed to invoke {eventType.Name}. Request rejected.");
//                 // 여기서 클라이언트에게 권한 없음을 알리는 RPC를 보낼 수도 있습니다.
//                 return; // 권한이 없으면 처리 중단
//             }
//
//             // 3. 이벤트 큐에 추가
//             // T 타입은 INetworkEvent를 구현하지만, NetworkEvent<T> 형태가 아니므로 직접 형변환 필요
//             // 또는 T를 INetworkEvent로 바로 받도록 제네릭을 수정할 수도 있습니다.
//             // 현재 제약 조건으로 T는 INetworkEvent를 구현하므로 아래 캐스팅은 안전합니다.
//             _eventQueue.Enqueue(networkEventData[0]);
//             Debug.Log(
//                 $"Event '{eventType.Name}' enqueued from client {invokedClientId}. Queue size: {_eventQueue.Count}");
//         }
//     }
// }

