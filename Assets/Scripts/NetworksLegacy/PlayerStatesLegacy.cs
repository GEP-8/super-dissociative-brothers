// using System.Collections.Generic;
// using Unity.Netcode;
// using UnityEngine;
//
// namespace Networks {
//     public class PlayerStates : INetworkSerializable {
//         [SerializeField] public float rightMoveSpeed;
//         public float leftMoveSpeed;
//         public float upMoveSpeed;
//         public float downMoveSpeed;
//         public bool isJumping;
//
//         public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
//             // serializer.SerializeValue(ref rightMoveSpeed);
//             // serializer.SerializeValue(ref leftMoveSpeed);
//             // serializer.SerializeValue(ref upMoveSpeed);
//             // serializer.SerializeValue(ref downMoveSpeed);
//             // serializer.SerializeValue(ref isJumping);
//
//             for (field in this.SerializeField)
//             {
//                 serializer.SerializeValue(ref field);
//             }
//         }
//     }
//
//     public class ServerPlayerStatesManager : NetworkBehaviour {
//         private enum Mergestrategy {
//             Priority,
//             
//             // allowDuplicates,
//             Max,
//             Mean,
//             Sum,
//             
//         }
//
//         private Mergestrategy mergeStrategy;
//         private PlayerStates mergedPlayerStates;
//         private Dictionary<ulong, PlayerStates> diatributedPlayerStatesMap = new();
//
//         private List<ulong, HashSet<field>> Permissions;
//         
//         [ServerRpc]
//         public void SyncClientStates(PlayerStates playerStates, ServerRpcParams rpcParams = default) {
//             ulong invokedClientId = rpcParams.Receive.SenderClientId;
//             diatributedPlayerStatesMap[invokedClientId] = playerStates;
//             mergePlayerStates();  // TODO: debouncing
//         }
//
//         public void mergePlayerStates() {
//             switch (mergeStrategy) {
//                 case Mergestrategy.Priority:
//                     for (client in diatributedPlayerStatesMap)
//                 {
//                     _permission =  Permissions[client.id]
//                         
//                         // dict에서 나중에 있는 사람이 높은 priority?
//                         for (field in _permission)
//                         {
//                             this.mergedPlayerStates[typeof(field)] = field.value 
//                         }
//                 }
//                     break;
//                 // ...
//             }
//         }
//     }
//
//
//     public class ClientPlayerStates : PlayerStates {
//         allowed = []
//         
//         getAllowedStates();
//
//         SyncClientStates() {
//             ServerPlayerStatesManager.SyncClientStates(this)
//         }
//     }
// }

