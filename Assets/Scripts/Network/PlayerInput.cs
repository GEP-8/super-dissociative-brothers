using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine;

namespace Network {
    public class PlayerInput : NetworkBehaviour {
        public static PlayerInput Instance { get; private set; }

        // 서버로부터 받은 데이터를 저장
        public PlayerAction[] _allowedActions;

        // 키가 눌린 상태이면 true, 아니면 false
        private bool[] _allowedActionsStatus;


        public Dictionary<KeyCode, PlayerAction> KeyToActionDictionary { get; } = new() {
            { KeyCode.W, PlayerAction.Jump },
            { KeyCode.UpArrow, PlayerAction.Jump },
            { KeyCode.A, PlayerAction.LeftMove },
            { KeyCode.LeftArrow, PlayerAction.LeftMove },
            { KeyCode.D, PlayerAction.RightMove },
            { KeyCode.RightArrow, PlayerAction.RightMove },
            { KeyCode.S, PlayerAction.Crouch },
            { KeyCode.DownArrow, PlayerAction.Crouch },
            { KeyCode.Space, PlayerAction.Jump },
            { KeyCode.LeftShift, PlayerAction.Crouch }
        };
        
        private void Start()
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
        
        public override void OnNetworkSpawn() {
            if (!IsClient || !IsOwner) {
                enabled = false;
                return;
            }

            _allowedActionsStatus = new bool[Enum.GetValues(typeof(PlayerAction)).Length];
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }

        public override void OnNetworkDespawn() {
            _allowedActions = null;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        }

        // 서버로부터 허용된 액션 목록을 받는 ClientRpc 메소드
        // [ClientRpc] 속성은 서버에서만 호출될 수 있으며, 호출한 서버가 대상 클라이언트에게 이 메소드를 실행하도록 요청합니다.
        [ClientRpc]
        public void ReceiveAllowedActionsClientRpc(PlayerAction[] actions, ClientRpcParams clientRpcParams = default) {
            // 이 코드는 RPC를 보낸 특정 클라이언트의 PlayerInput 인스턴스에서 실행됩니다.
            _allowedActions = actions;
            Array.Fill(_allowedActionsStatus, false);

            Debug.LogWarning(
                $"클라이언트 {NetworkManager.Singleton.LocalClientId}가 허용된 액션 목록을 받았습니다: {string.Join(", ", _allowedActions)}");
        }


        // 실제 입력 처리 로직 예시
        private void Update() {
            foreach (KeyValuePair<KeyCode, PlayerAction> keyToAction in KeyToActionDictionary) {
                // 키보드가 눌리니 이벤트 시작
                if (Input.GetKeyDown(keyToAction.Key)) {
                    // 기존과 값이 달라진 경우
                    if (_allowedActionsStatus[(int)keyToAction.Value] == false) {
                        _allowedActionsStatus[(int)keyToAction.Value] = true;

                        // sync to server
                        PlayerNetwork.Instance.SyncClientStateServerRpc(keyToAction.Value, true);
                    }
                }

                // 키보드가 떨어지니 이벤트 끝
                if (Input.GetKeyUp(keyToAction.Key)) {
                    // 기존과 값이 달라진 경우
                    if (_allowedActionsStatus[(int)keyToAction.Value]) {
                        _allowedActionsStatus[(int)keyToAction.Value] = false;

                        // sync to server
                        PlayerNetwork.Instance.SyncClientStateServerRpc(keyToAction.Value, false);
                    }
                }
            }
        }
        
        private void OnClientDisconnect(ulong clientId)
        {
            Debug.Log($"클라이언트 연결 끊김: {clientId}");
            
            // 현재 네트워크 세션을 종료합니다.
            // 클라이언트라면 서버와의 연결을 끊습니다.
            // 호스트라면 서버를 닫고 모든 클라이언트의 연결을 끊습니다.
            NetworkManager.Singleton.Shutdown();
            
            Debug.Log("연결을 종료하고 로비로 돌아갑니다.");
            SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
        }
    }
}