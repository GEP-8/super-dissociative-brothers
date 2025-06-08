using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{   
    public float collectedPills = 0f; // 플레이어가 수집한 알약의 개수
    public Rigidbody2D rb;
    public Animator animator; // 애니메이터 컴포넌트 (필요시 사용)
    private BoxCollider2D boxCollider; // 플레이어의 박스 콜라이더
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody 컴포넌트가 없습니다. Rigidbody를 추가해주세요.");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        switch (tag)
        {
            case "Enemy"or "Obstacle":
                Debug.Log("적과 충돌했습니다! 플레이어 사망.");
                Die(); // 플레이어 사망

                break;

            case "Pill":
                Debug.Log("알약과 충돌했습니다! 알약 수집.");
                CollectPill(collision.gameObject); // 알약 수집
                break;
            case "Flag":
                Debug.Log("플래그에 도달했습니다! 다음 단계로 이동.");
                // 예: 다음 단계로 이동하는 로직 추가
                break;
        }
    }

    private void Die()
    {
        Debug.Log("플레이어 사망!");
        // 플레이어 사망 처리 로직 추가
        animator.SetTrigger("Die"); // 애니메이터 트리거 설정 (필요시 사용)
        rb.AddForce(Vector2.up * 3f, ForceMode2D.Impulse); // 플레이어를 위로 튕겨내기
        boxCollider.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    private void CollectPill(GameObject pill)
    {
        Debug.Log("알약 획득!");
        collectedPills += 1; // 알약 개수 증가
        Destroy(pill); // 알약 제거
    }
}
