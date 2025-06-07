using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(Rigidbody2D))] // Rigidbody가 필요하다면 추가
    public class PlayerController : MonoBehaviour
    {
        private PlayerNetwork playerNetwork;
        private Rigidbody2D rb;
        public BoxCollider2D coll;
        [SerializeField] private LayerMask groundLayer; // 바닥 레이어를 지정하기 위한 변수
        public bool isGrounded = true; // 바닥에 있는지 여부
        public float speed = 2f; // 이동 속도
        public float jumpForce = 2f; // 점프 힘
        public Animator animator; // 애니메이터 컴포넌트 (필요시 추가)
        private Vector3 originalSize; // 최초 크기를 저장하기 위한 변수
        private void Awake()
        {
            playerNetwork = PlayerNetwork.Instance;
            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<BoxCollider2D>();
            originalSize = coll.size; // 최초 크기 저장
        }

        private void FixedUpdate()
        {
            animator.SetBool("isGrounded", isGrounded); // 애니메이터에 바닥 상태 전달 (필요시 추가)
            animator.SetBool("Walkingright", false); // 애니메이션 상태 초기화 (필요시 추가)
            animator.SetBool("Walkingleft", false); // 애니메이션 상태 초기화 (필요시 추가)
            animator.SetBool("isjumping", false); // 점프 애니메이션 상태 초기화 (필요시 추가)
            animator.SetBool("godown", false); // 점프 애니메이션 상태 초기화 (필요시 추가)
            animator.SetBool("isCrouching", false); // 앉기 애니메이션 상태 초기화 (필요시 추가)
            coll.size = originalSize; // 원래 크기로 복구
            if (rb.linearVelocity.y > 0 && !isGrounded) // 위로 이동 중이고 바닥에 있지 않을 때
            {
                animator.SetBool("isjumping", true); // 점프 애니메이션 상태 업데이트 (필요시 추가)
            }
            if (rb.linearVelocity.y < 0 && !isGrounded) // 아래로 이동 중이고 바닥에 있지 않을 때
            {
                animator.SetBool("godown", true); // 점프 애니메이션 상태 초기화 (필요시 추가)
            }

            foreach ((PlayerAction playerAction, bool value) in playerNetwork.MergedActionStatusDictionary)
            {
                if (!value)
                {
                    continue;
                }

                switch (playerAction)
                {
                    case PlayerAction.Crouch:
                        if (isGrounded) // 앉기는 바닥에 있을 때만 가능
                        {
                            // 앉기 로직을 여기에 추가 (예: 콜라이더 크기 조정 등)
                            ShrinkColliderSafely(); // 콜라이더 크기를 안전하게 축소 
                            animator.SetBool("isCrouching", true); // 앉기 애니메이션 상태 업데이트 (필요시 추가)
                        }
                        break;
                    case PlayerAction.RightMove:
                        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
                        animator.SetBool("Walkingright", true); // 애니메이션 상태 업데이트 (필요시 추가)
                        break;
                    case PlayerAction.LeftMove:
                        rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
                        animator.SetBool("Walkingleft", true); // 애니메이션 상태 업데이트 (필요시 추가)
                        break;
                    case PlayerAction.Jump:
                        if (isGrounded) // 점프는 바닥에 있을 때만 가능
                        {
                            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // 점프를 위해 위쪽으로 힘을 추가
                        }
                        break;

                }

            }
        }
        
        // 충돌이 시작될 때 호출됩니다.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // 충돌한 오브젝트의 레이어가 'GroundLayer'에 포함되는지 확인
            if (((1 << collision.gameObject.layer) & groundLayer) != 0)
            {
                isGrounded = true;
                Debug.Log("땅에 닿았습니다!");
            }
        }
        // 충돌이 끝날 때 호출됩니다.
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (((1 << collision.gameObject.layer) & groundLayer) != 0)
            {
                // 주의: 여러 개의 땅 오브젝트와 동시에 닿아있다가 하나만 떨어질 경우 문제가 될 수 있음.
                // 아래 OverlapCircle 방식과 조합하는 것이 더 정확할 수 있습니다.
                isGrounded = false;
                Debug.Log("땅에서 떨어졌습니다!");
            }
        }
        
        
        private void ShrinkColliderSafely()
        {
            rb.isKinematic = true; // 물리 계산을 잠깐 끔
            coll.size = new Vector2(coll.size.x, coll.size.y * 0.7f);
            Physics.SyncTransforms(); // 변경사항 동기화
            rb.isKinematic = false; // 다시 물리 계산 적용
        }

    }
}
