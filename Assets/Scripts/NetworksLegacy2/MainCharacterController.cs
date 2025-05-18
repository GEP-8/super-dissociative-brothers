using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // 예시로 Rigidbody 사용
public class MainCharacterController : NetworkBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody _rb;
    private PlayerStateData _currentInputs;
    private bool _jumpRequested;

    public override void OnNetworkSpawn() {
        if (!IsServer) // 서버에서만 캐릭터 로직을 실행
        {
            enabled = false;
            return;
        }

        _rb = GetComponent<Rigidbody>();
    }

    // ServerPlayerStateManager가 호출할 함수
    public void SetInputs(PlayerStateData inputs) {
        _currentInputs = inputs;
        if (_currentInputs.isJumping) // 점프는 이벤트성으로 처리
            _jumpRequested = true;
    }

    private void FixedUpdate() {
        if (!IsServer) return;

        // 이동 처리
        float horizontalInput = _currentInputs.rightMoveSpeed - _currentInputs.leftMoveSpeed;
        float verticalInput = _currentInputs.upMoveSpeed - _currentInputs.downMoveSpeed; // 2D Top-Down 또는 Z축 이동에 사용

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        _rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, _rb.linearVelocity.y, moveDirection.z * moveSpeed);

        // 점프 처리
        if (_jumpRequested) {
            // 실제로는 지면 체크(IsGrounded) 등이 필요
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _jumpRequested = false; // 점프 요청 처리 완료
        }
    }
}