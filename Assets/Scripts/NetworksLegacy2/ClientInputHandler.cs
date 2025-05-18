using Unity.Netcode;
using UnityEngine;

public class ClientInputHandler : NetworkBehaviour {
    // 이 클라이언트가 어떤 입력을 담당하는지 (선택적, UI 표시나 자체 검증용)
    // [SerializeField] private List<ControllableStateField> myResponsibilities;

    private PlayerStateData _currentStateData;

    public override void OnNetworkSpawn() {
        if (!IsOwner) // 각 클라이언트는 자신의 ClientInputHandler만 제어
        {
            enabled = false;
            return;
        }

        _currentStateData = new PlayerStateData();
    }

    private void Update() {
        // 로컬 입력 감지 (예시)
        // 실제 게임에서는 더 정교한 입력 처리가 필요할 수 있습니다.
        // 또한, 어떤 클라이언트가 어떤 입력을 담당하는지에 따라 이 부분을 조건부로 활성화해야 합니다.
        // 예를 들어, Client A는 X축, Client B는 Y축, Client C는 점프를 담당한다면,
        // 각 클라이언트의 ClientInputHandler는 자신이 담당하는 부분의 입력만 읽도록 해야 합니다.
        // 여기서는 모든 클라이언트가 모든 입력을 보내고 서버가 권한에 따라 필터링한다고 가정합니다.

        _currentStateData.rightMoveSpeed = Input.GetKey(KeyCode.D) ? 1f : 0f; // 오른쪽 이동 시도
        _currentStateData.leftMoveSpeed = Input.GetKey(KeyCode.A) ? 1f : 0f; // 왼쪽 이동 시도
        _currentStateData.upMoveSpeed = Input.GetKey(KeyCode.W) ? 1f : 0f; // 위쪽 이동 시도
        _currentStateData.downMoveSpeed = Input.GetKey(KeyCode.S) ? 1f : 0f; // 아래쪽 이동 시도
        _currentStateData.isJumping = Input.GetKeyDown(KeyCode.Space); // 점프 시도

        Debug.LogWarning(_currentStateData);

        // 서버로 현재 상태 전송
        // ServerPlayerStateManager.Instance를 직접 사용하려면 서버와 클라이언트 모두에 해당 스크립트가 존재해야하며,
        // 클라이언트에서는 Instance가 null이 아님을 보장할 수 없음.
        // 대신, 이 ClientInputHandler가 ServerRpc를 가지고 있고, 서버에서 그 RPC가 ServerPlayerStateManager의 메소드를 호출하게 할 수도 있음.
        // 여기서는 ServerPlayerStateManager가 싱글톤이고 서버에만 존재한다고 가정, RPC를 직접 호출.
        if (NetworkManager.Singleton.IsClient && ServerPlayerStateManager.Instance != null) // 클라이언트이고, 서버 매니저를 찾을 수 있다면
            // ServerRpc는 NetworkBehaviour 위에서만 호출될 수 있으므로,
            // ServerPlayerStateManager.Instance의 ServerRpc를 호출해야 함.
            ServerPlayerStateManager.Instance.SyncClientStateServerRpc(_currentStateData);
        else if (NetworkManager.Singleton.IsHost) // 호스트인 경우 (서버와 클라이언트 역할 동시)
            // 호스트는 RPC를 통하지 않고 직접 서버 로직을 호출할 수도 있으나,
            // 일관성을 위해 RPC를 사용하는 것이 좋음 (또는 직접 맵에 추가)
            // ServerPlayerStateManager.Instance.SyncClientStateServerRpc(_currentStateData); //
            // 아래는 호스트가 로컬에서 직접 처리하는 경우 (RPC Latency 없음)
            // ServerPlayerStateManager.Instance._distributedPlayerStatesMap[NetworkManager.Singleton.LocalClientId] = _currentStateData;
            // ServerPlayerStateManager.Instance._statesDirty = true;
            // 위와 같이 직접 접근보다는 RPC를 동일하게 사용하는 것이 구조적으로 나을 수 있음.
            ServerPlayerStateManager.Instance.SyncClientStateServerRpc(_currentStateData);


        // 전송 후 점프 상태는 리셋 (KeyDown 이벤트이므로)
        // _currentStateData.isJumping = false; // 매 프레임 전송 시 이렇게 하면 안됨. 서버가 처리하고 상태를 받아오거나, 입력 이벤트를 큐잉해야함.
        // 현재 구조에서는 isJumping이 true로 전송된 후, 다음 프레임에 GetKeyDown이 false면 false로 전송될 것임.
    }
}