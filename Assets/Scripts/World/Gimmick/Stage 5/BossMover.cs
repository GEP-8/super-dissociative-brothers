using UnityEngine;

public class BossMover : MonoBehaviour
{
    public float moveSpeed = 2f;       // 오른쪽으로 이동 속도
    public float jumpHeight = 1f;      // 점프 높이 (y축 오실레이션 크기)
    public float jumpFrequency = 2f;   // 점프 주기

    private Vector3 startPosition;
    private float timer;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        Move();
        ChangeScale();
    }

    void Move()
    {
        // 수평 이동
        Vector3 move = Vector3.right * moveSpeed * Time.deltaTime;

        // 수직 움직임 (sin 곡선 사용)
        float verticalOffset = Mathf.Sin(timer * jumpFrequency) * jumpHeight;
        move.y = verticalOffset;

        transform.position = startPosition + move + Vector3.right * moveSpeed * timer;
    }

    void ChangeScale()
    {
        float scaleY = 1f + Mathf.Sin(timer * jumpFrequency) * 0.1f;
        transform.localScale = new Vector3(1f, scaleY, 1f);
    }
}
