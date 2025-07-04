using UnityEngine;
using System.Collections;
using System;

public class PlayerCollisionHandler : MonoBehaviour
{
    public static event Action OnPillCollected;
    public static event Action OnFlagReached;

    public float collectedPills = 0f;

    [Header("Component References")]
    public Rigidbody2D rb;
    public Animator animator;
    private BoxCollider2D boxCollider;

    [Header("UI")]
    public RectTransform gameOverUI;
    public RectTransform clearUI;
    public Vector2 uiTargetPos = Vector2.zero;
    public float uiMoveDuration = 1.0f;
    public AnimationCurve moveCurve;


    public static event Action OnGameStopped;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D 컴포넌트가 없습니다.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        switch (tag)
        {
            case "Enemy":
            case "Obstacle":
                Debug.Log("적 또는 장애물과 충돌했습니다! 플레이어 사망.");
                Die();
                break;

            case "Pill":
                Debug.Log("알약과 충돌했습니다!");
                CollectPill(collision.gameObject);
                OnPillCollected?.Invoke();
                break;

            case "Flag":
                Debug.Log("플래그에 도달했습니다!");
                OnFlagReached?.Invoke();
                StartCoroutine(ShowUIWithCurve(clearUI));
                break;
        }
    }

    public void Die()
    {
        Debug.Log("플레이어 사망!");
        animator.SetTrigger("Die");
        rb.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
        boxCollider.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;

        StartCoroutine(ShowUIWithCurve(gameOverUI));
    }

    private void CollectPill(GameObject pill)
    {
        collectedPills += 1;
        Destroy(pill);
    }

    private IEnumerator ShowUIWithCurve(RectTransform ui)
    {
        yield return new WaitForSecondsRealtime(1f);

        Vector2 start = ui.anchoredPosition;
        Vector2 end = uiTargetPos;
        float elapsed = 0f;
        float lastDt = 0f;

        while (elapsed < uiMoveDuration)
        {
            // 부드럽게 보정된 deltaTime
            float dt = Mathf.Lerp(lastDt, Time.unscaledDeltaTime, 0.5f);
            lastDt = dt;

            elapsed += dt;
            float t = Mathf.Clamp01(elapsed / uiMoveDuration);
            float curvedT = moveCurve.Evaluate(t);
            ui.anchoredPosition = Vector2.Lerp(start, end, curvedT);
            yield return null;
        }

        ui.anchoredPosition = end;

        OnGameStopped?.Invoke(); // Stop the game process
    }
}
