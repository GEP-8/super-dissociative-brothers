using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.Netcode;
using UnityEngine;

namespace Network
{
    public class PlayerNetwork : NetworkBehaviour
    {
        // for singleton
        public static PlayerNetwork Instance { get; private set; }


        public MergeStrategy MergeStrategy { get; set; } = MergeStrategy.Latest;
        public List<ulong> clientIds { get; } = new List<ulong>();
        public Dictionary<ulong, PlayerAction[]> AllowedActionsDictionary { get; } = new();
        public Dictionary<ulong, Dictionary<PlayerAction, bool>> ActionStatusDictionary { get; } = new();

        public Dictionary<PlayerAction, bool> MergedActionStatusDictionary { get; } = new();

        private void Awake()
        {
            // 싱글톤 인스턴스 설정
            if (Instance == null)
            {
                Instance = this;
            }
            // else
            // {
            //     Debug.LogError("PlayerController 인스턴스가 이미 존재합니다! 중복된 인스턴스를 제거합니다.");
            //     Destroy(gameObject); // 중복된 인스턴스가 있다면 삭제
            // }
        }

        public override void OnNetworkSpawn()
        {
            // server 에서만 플레이어의 움직임을 관리
            if (!IsServer)
            {
                enabled = false;
                return;
            }

            // 이벤트 구독
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }

        public override void OnNetworkDespawn()
        {
            // Despawn 시 이벤트 구독 해제 (서버에서만)
            if (IsServer && NetworkManager.Singleton != null) // 씬 전환 등으로 NetworkManager가 null이 될 수 있으니 체크
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            }
        }

        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"클라이언트 ID: {clientId}가 서버에 연결되었습니다.");
            ClearActionStatusDictionary();

            // 새로운 클라이언트의 값 초기화
            clientIds.Add(clientId);
            ActionStatusDictionary[clientId] = new Dictionary<PlayerAction, bool>();

            for (int i = 0; i < clientIds.Count; i++) {
                AllowedActionsDictionary[clientIds[i]] = PlayerConstants.keyDistribution[clientIds.Count][i];
                SyncAllowedActions(clientIds[i]);
            }
        }
        

        public void ShiftAllowedActions(int shift) {
            ClearActionStatusDictionary();
            
            for (int i = 0; i < clientIds.Count; i++) {
                AllowedActionsDictionary[clientIds[i]] = PlayerConstants.keyDistribution[clientIds.Count][(i + shift) % (clientIds.Count)];
                SyncAllowedActions(clientIds[i]);
            }
        }

        public void ClearActionStatusDictionary() {
            MergedActionStatusDictionary.Clear();
            for (int i = 0; i < clientIds.Count; i++) {
                ActionStatusDictionary[clientIds[i]] = new Dictionary<PlayerAction, bool>();
            }

            MergeActionStatus();
        }

        private void SyncAllowedActions(ulong clientId)
        {
            // TODO: if문 중첩 해결 && 좀더 간단하게 call하는 방법 찾기... 자료가 잘 없어서 힘드네
            // 연결된 클라이언트의 PlayerObject를 찾습니다.
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient networkClient))
            {
                if (networkClient.PlayerObject != null)
                {
                    // PlayerObject에서 PlayerInput 스크립트를 가져옵니다.
                    // 서버에서는 클라이언트의 PlayerObject 인스턴스를 가지고 있습니다.
                    PlayerInput playerInput = networkClient.PlayerObject.GetComponent<PlayerInput>();

                    if (playerInput != null)
                    {
                        // ClientRpcParams 구조체를 생성하고 TargetClientId를 설정합니다.
                        ClientRpcParams rpcParams = new()
                        {
                            Send = new ClientRpcSendParams
                            {
                                TargetClientIds = new[] { clientId } // **여기서 특정 클라이언트 ID를 지정합니다.**
                            }
                        };

                        // PlayerInput 스크립트의 ClientRpc 메소드를 호출하여 액션 목록을 전송합니다.
                        playerInput.ReceiveAllowedActionsClientRpc(AllowedActionsDictionary[clientId], rpcParams);

                        Debug.Log($"Client {clientId}에게 허용된 액션 목록을 전송했습니다.");
                    }
                    else
                    {
                        Debug.LogError($"Client {clientId}'s PlayerObject does not have PlayerInput component.");
                    }
                }
                else
                {
                    Debug.LogError($"Client {clientId}'s PlayerObject is null.");
                }
            }
            else
            {
                Debug.LogError($"NetworkClient not found for client {clientId}.");
            }
        }

        // 클라이언트가 이 RPC를 호출하여 자신의 상태를 서버로 보냅니다.
        [ServerRpc(RequireOwnership = false)] // 소유권 없이 모든 클라이언트가 호출 가능
        public void SyncClientStateServerRpc(PlayerAction playerAction, bool value,
            ServerRpcParams rpcParams = default)
        {
            // server가 아니면 enabled = false라 없어도 됨
            // if (!IsServer) {
            //     return;
            // }

            ulong invokedClientId = rpcParams.Receive.SenderClientId;

            if (!AllowedActionsDictionary[invokedClientId].Contains(playerAction))
            {
                Debug.LogWarning($"Client {invokedClientId} is not allowed to perform {playerAction}.");
                return;
            }

            ActionStatusDictionary[invokedClientId][playerAction] = value;
            MergeActionStatus();
            // Debug.Log($"Server received {playerAction}: {value}. from Client ID: {invokedClientId}");
        }

        private void MergeActionStatus()
        {
            switch (MergeStrategy)
            {
                case MergeStrategy.Latest:
                    foreach ((ulong clientId, Dictionary<PlayerAction, bool> dictionary) in ActionStatusDictionary)
                    {
                        foreach ((PlayerAction playerAction, bool value) in dictionary)
                        {
                            MergedActionStatusDictionary[playerAction] = value;
                        }
                    }

                    break;
            }

            Debug.Log(JsonConvert.SerializeObject(MergedActionStatusDictionary));
        }
    }
}
