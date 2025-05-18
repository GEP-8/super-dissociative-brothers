using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerPlayerStateManager : NetworkBehaviour {
    public static ServerPlayerStateManager Instance { get; private set; } // 싱글톤 접근

    public enum MergeStrategy {
        Priority, // 권한에 따라, 또는 특정 순서에 따라
        Max, // 각 필드별 최댓값
        Sum, // 각 필드별 합계 (속도 등에 적합할 수 있음)
        Mean // 각 필드별 평균값
    }

    [Header("Settings")] [SerializeField] private MergeStrategy mergeStrategy = MergeStrategy.Priority;
    [SerializeField] private MainCharacterController mainCharacter; // 서버에서 제어할 메인 캐릭터

    // [Header("Permissions (Manual Setup Example)")]
    // 인스펙터에서 설정할 수 있는 권한 리스트 (ClientId, 허용 필드)
    // 실제 게임에서는 더 동적인 방식이나 ScriptableObject를 사용할 수 있습니다.
    [Serializable]
    public class ClientPermissionEntry {
        public ulong clientId;
        public List<ControllableStateField> allowedFields;
    }

    [SerializeField] private List<ClientPermissionEntry> clientPermissionsSetup = new();

    // 내부적으로 사용할 권한 맵
    private readonly Dictionary<ulong, HashSet<ControllableStateField>> _clientPermissionsMap = new();

    // 클라이언트별 수신된 상태 저장
    private readonly Dictionary<ulong, PlayerStateData> _distributedPlayerStatesMap = new();

    // 최종 병합된 상태
    private PlayerStateData _mergedPlayerStates;
    public PlayerStateData MergedPlayerStates => _mergedPlayerStates; // 외부에서 읽기 전용으로 접근 가능

    private bool _statesDirty; // 상태 변경 여부 플래그 (Debouncing용)

    public override void OnNetworkSpawn() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (!IsServer) {
            enabled = false; // 서버 전용 스크립트
            return;
        }

        InitializePermissions();
        _mergedPlayerStates = new PlayerStateData(); // 초기화
    }

    private void InitializePermissions() {
        _clientPermissionsMap.Clear();
        foreach (ClientPermissionEntry entry in clientPermissionsSetup) {
            if (!_clientPermissionsMap.ContainsKey(entry.clientId))
                _clientPermissionsMap[entry.clientId] = new HashSet<ControllableStateField>();
            foreach (ControllableStateField field in entry.allowedFields)
                _clientPermissionsMap[entry.clientId].Add(field);
        }
    }

    // 클라이언트가 이 RPC를 호출하여 자신의 상태를 서버로 보냅니다.
    [ServerRpc(RequireOwnership = false)] // 소유권 없이 모든 클라이언트가 호출 가능
    public void SyncClientStateServerRpc(PlayerStateData clientStateData, ServerRpcParams rpcParams = default) {
        if (!IsServer) return;

        ulong invokedClientId = rpcParams.Receive.SenderClientId;
        _distributedPlayerStatesMap[invokedClientId] = clientStateData;
        _statesDirty = true; // 상태가 변경되었음을 표시
        Debug.Log($"Server received state from Client ID: {invokedClientId}");
    }

    private void FixedUpdate() // 물리 업데이트 주기에 맞춰 상태 병합 및 적용
    {
        if (!IsServer || !_statesDirty) return;

        MergeAllClientStates();
        ApplyMergedStateToCharacter();
        _statesDirty = false; // 플래그 리셋
    }

    private void MergeAllClientStates() {
        // 병합 전 초기화 (기본값 또는 이전 상태 유지 등 전략에 따라 다름)
        PlayerStateData newMergedState = new(); // 기본값으로 시작

        switch (mergeStrategy) {
            case MergeStrategy.Priority:
                // Priority: 권한을 가진 클라이언트의 값으로 덮어쓰기
                // 간단한 예: clientPermissionsSetup에 먼저 정의된 클라이언트가 낮은 우선순위
                // 또는, 특정 클라이언트 ID 순서대로 (예: 오름차순)
                // 여기서는 _distributedPlayerStatesMap의 순회 순서에 의존 (일반적으로는 보장되지 않음)
                // 좀 더 명확한 우선순위가 필요하다면, 클라이언트 목록을 정렬해서 순회해야 합니다.
                // 여기서는 간단하게, 권한이 있는 클라이언트의 값을 덮어쓰는 형태로 구현합니다.
                // (만약 여러 클라이언트가 같은 필드 권한을 가진다면 마지막으로 처리된 클라이언트 값이 적용될 수 있음)
                foreach (KeyValuePair<ulong, PlayerStateData> kvp in _distributedPlayerStatesMap) {
                    ulong clientId = kvp.Key;
                    PlayerStateData clientState = kvp.Value;

                    if (_clientPermissionsMap.TryGetValue(clientId, out HashSet<ControllableStateField> permissions)) {
                        if (permissions.Contains(ControllableStateField.RightMoveSpeed))
                            newMergedState.rightMoveSpeed = clientState.rightMoveSpeed;
                        if (permissions.Contains(ControllableStateField.LeftMoveSpeed))
                            newMergedState.leftMoveSpeed = clientState.leftMoveSpeed;
                        if (permissions.Contains(ControllableStateField.UpMoveSpeed))
                            newMergedState.upMoveSpeed = clientState.upMoveSpeed;
                        if (permissions.Contains(ControllableStateField.DownMoveSpeed))
                            newMergedState.downMoveSpeed = clientState.downMoveSpeed;
                        if (permissions.Contains(ControllableStateField.IsJumping))
                            newMergedState.isJumping = clientState.isJumping;
                    }
                }

                break;

            case MergeStrategy.Max:
                foreach (KeyValuePair<ulong, PlayerStateData> kvp in _distributedPlayerStatesMap) {
                    ulong clientId = kvp.Key;
                    PlayerStateData clientState = kvp.Value;
                    if (_clientPermissionsMap.TryGetValue(clientId, out HashSet<ControllableStateField> permissions)) {
                        if (permissions.Contains(ControllableStateField.RightMoveSpeed))
                            newMergedState.rightMoveSpeed =
                                Mathf.Max(newMergedState.rightMoveSpeed, clientState.rightMoveSpeed);
                        if (permissions.Contains(ControllableStateField.LeftMoveSpeed))
                            newMergedState.leftMoveSpeed =
                                Mathf.Max(newMergedState.leftMoveSpeed,
                                    clientState.leftMoveSpeed); // Max가 적절한지 고려 필요 (보통 절대값으로)
                        if (permissions.Contains(ControllableStateField.UpMoveSpeed))
                            newMergedState.upMoveSpeed = Mathf.Max(newMergedState.upMoveSpeed, clientState.upMoveSpeed);
                        if (permissions.Contains(ControllableStateField.DownMoveSpeed))
                            newMergedState.downMoveSpeed =
                                Mathf.Max(newMergedState.downMoveSpeed, clientState.downMoveSpeed);
                        if (permissions.Contains(ControllableStateField.IsJumping))
                            newMergedState.isJumping =
                                newMergedState.isJumping || clientState.isJumping; // 하나라도 true면 true
                    }
                }

                break;

            case MergeStrategy.Sum:
            case MergeStrategy.Mean: // Mean은 Sum 이후에 참여자 수로 나누어야 함
                int q = 0; // 점프는 Sum/Mean이 이상하므로 별도 처리 또는 Max/Priority 사용 권장
                foreach (KeyValuePair<ulong, PlayerStateData> kvp in _distributedPlayerStatesMap) {
                    ulong clientId = kvp.Key;
                    PlayerStateData clientState = kvp.Value;
                    if (_clientPermissionsMap.TryGetValue(clientId, out HashSet<ControllableStateField> permissions)) {
                        if (permissions.Contains(ControllableStateField.RightMoveSpeed))
                            newMergedState.rightMoveSpeed += clientState.rightMoveSpeed;
                        if (permissions.Contains(ControllableStateField.LeftMoveSpeed))
                            newMergedState.leftMoveSpeed += clientState.leftMoveSpeed;
                        if (permissions.Contains(ControllableStateField.UpMoveSpeed))
                            newMergedState.upMoveSpeed += clientState.upMoveSpeed;
                        if (permissions.Contains(ControllableStateField.DownMoveSpeed))
                            newMergedState.downMoveSpeed += clientState.downMoveSpeed;
                        if (permissions.Contains(ControllableStateField.IsJumping) && clientState.isJumping)
                            newMergedState.isJumping = true; // Sum/Mean 보다는 OR 연산이 적합
                    }
                }

                if (mergeStrategy == MergeStrategy.Mean && _distributedPlayerStatesMap.Count > 0) {
                    // 참여하고 권한이 있는 클라이언트 수로 나눠야 정확하지만, 여기서는 단순화하여 전체 클라이언트 수로 나눔
                    // (실제로는 각 필드별 기여자 수를 세어야 함)
                    int activeContributors = _distributedPlayerStatesMap.Count; // 이 부분은 필드별 기여자로 세분화 필요
                    newMergedState.rightMoveSpeed /= activeContributors;
                    newMergedState.leftMoveSpeed /= activeContributors;
                    newMergedState.upMoveSpeed /= activeContributors;
                    newMergedState.downMoveSpeed /= activeContributors;
                }

                break;
        }

        _mergedPlayerStates = newMergedState;
        Debug.Log(
            $"States Merged: R:{_mergedPlayerStates.rightMoveSpeed}, L:{_mergedPlayerStates.leftMoveSpeed}, U:{_mergedPlayerStates.upMoveSpeed}, D:{_mergedPlayerStates.downMoveSpeed}, J:{_mergedPlayerStates.isJumping}");
    }

    private void ApplyMergedStateToCharacter() {
        if (mainCharacter != null)
            mainCharacter.SetInputs(_mergedPlayerStates);
        else
            Debug.LogWarning("MainCharacter is not assigned in ServerPlayerStateManager.");
    }
}