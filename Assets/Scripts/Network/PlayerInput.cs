using Unity.Netcode;
using UnityEngine;

namespace Network {
    public class PlayerInput : NetworkBehaviour {
        // 서버로부터 받은 데이터를 저장
        private PlayerAction[] _allowedActions;

        public override void OnNetworkSpawn() {
            if (!IsClient) {
                enabled = false;
                return;
            }

            Debug.Log(_allowedActions);
        }

        public override void OnNetworkDespawn() {
            _allowedActions = null;
        }

        // 서버로부터 허용된 액션 목록을 받는 ClientRpc 메소드
        // [ClientRpc] 속성은 서버에서만 호출될 수 있으며, 호출한 서버가 대상 클라이언트에게 이 메소드를 실행하도록 요청합니다.
        [ClientRpc]
        public void ReceiveAllowedActionsClientRpc(PlayerAction[] actions, ClientRpcParams clientRpcParams = default) {
            // 이 코드는 RPC를 보낸 특정 클라이언트의 PlayerInput 인스턴스에서 실행됩니다.
            _allowedActions = actions;

            Debug.Log(
                $"클라이언트 {NetworkManager.Singleton.LocalClientId}가 허용된 액션 목록을 받았습니다: {string.Join(", ", _allowedActions)}");
        }


        // 실제 입력 처리 로직 예시
        private void Update() {
            // 로컬 플레이어의 클라이언트에서만 실행
            if (!IsClient || !IsOwner) return;

            // 허용된 액션 목록을 받았는지 확인
            if (_allowedActions == null)
                // 액션 목록을 받을 때까지 입력 처리를 건너뜁니다.
                return;
        }
    }
}