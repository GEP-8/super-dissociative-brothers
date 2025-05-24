using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(PlayerNetwork))] // 일단은 이렇게 되어있는데 매니저로 빼는게 좋을려나?
    [RequireComponent(typeof(Rigidbody))] // Rigidbody가 필요하다면 추가
    public class PlayerController : MonoBehaviour
    {
        private PlayerNetwork playerNetwork;
        private Rigidbody rb;
        private Vector2 dir;

        private void Awake()
        {
            playerNetwork = PlayerNetwork.Instance;
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            dir = Vector2.zero; // 이동 후 방향 초기화
            foreach ((PlayerAction playerAction, bool value) in playerNetwork.MergedActionStatusDictionary)
            {
                if (!value)
                {
                    continue;
                }

                switch (playerAction)
                {
                    case PlayerAction.RightMove:
                        dir = Vector2.right;
                        break;
                    case PlayerAction.LeftMove:
                        dir = Vector2.left;
                        break;
                    case PlayerAction.UpMove:
                        dir = Vector2.up;
                        break;
                    case PlayerAction.DownMove:
                        dir = Vector2.down;
                        break;
                    case PlayerAction.Jump:
                        // TODO
                        break;

                }

            }
            // Rigidbody를 사용하여 이동
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y) + dir * 5f; // y축은 0으로 설정하여 2D 이동만 처리



        }
    }
}
