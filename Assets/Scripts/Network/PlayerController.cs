﻿
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
        public float jumpForce = 15f; // 점프 힘
        public Animator animator; // 애니메이터 컴포넌트 (필요시 추가)
        private Vector3 originalSize; // 최초 크기를 저장하기 위한 변수
        public JumpState jumpState = JumpState.Idle; // 점프 상태를 나타내는 변수
        private void Awake()
        {
            playerNetwork = PlayerNetwork.Instance;
            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<BoxCollider2D>();
            originalSize = coll.size; // 최초 크기 저장
        }

        private void FixedUpdate()
        {
            CheckGround(); // 바닥에 있는지 확인
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
                        Debug.Log("Jump action received");
                        switch (jumpState)
                        {
                            case JumpState.Idle:
                                Debug.Log("Jump action in Idle state");
                                if (isGrounded) // 바닥에 있을 때만 점프 가능
                                {
                                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // 점프 힘 적용
                                    jumpState = JumpState.JumpUp; // 점프 상태 업데이트
                                }
                                break;
                            case JumpState.JumpUp:
                                Debug.Log("Jump action in JumpUp state");
                                if (!isGrounded) // 바닥에 있을 때만 점프 가능
                                {
                                    jumpState = JumpState.JumpDown; // 점프 상태 업데이트
                                }
                                break;
                            case JumpState.JumpDown:
                                Debug.Log("Jump action in JumpDown state");
                                if (isGrounded) // 바닥에 있을 때만 점프 가능
                                {
                                    jumpState = JumpState.Idle; // 점프 상태 업데이트
                                }
                                break;
                        }break;

                }

            }
        }
        private void CheckGround()
        {
            Vector2 origin = new Vector2(transform.position.x, transform.position.y - (transform.localScale.y * .5f));
            float distance = .50f;
            
            // Raycast를 아래 방향으로 쏴서 groundLayer에 닿는지 확인
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, distance, groundLayer);
            
            if (hit) {
                isGrounded = true;
            }
            else {
                isGrounded = false;
            }
            Debug.DrawRay(origin, Vector2.down * distance, isGrounded ? Color.green : Color.red);
        }
        
        private void ShrinkColliderSafely()
        {
            coll.size = new Vector2(coll.size.x, coll.size.y * 0.7f);
            Physics.SyncTransforms(); // 변경사항 동기화
        }

    }
}