using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingEntity : MonoBehaviour
{
    public float movingSpeed = 1.0f;
    public float checkDistance = 0.1f;
    public LayerMask groundLayer;

    public bool useGroundCheck = true;
    public Transform groundCheck;

    public bool useWallCheck = true;
    public Transform wallCheck;

    public bool debugMode = false;

    public bool initialDirectionRight = true;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = initialDirectionRight ? Vector2.right : Vector2.left;
        groundLayer = LayerMask.GetMask("Ground");
    }

    void FixedUpdate()
    {
        bool shouldFlip = false;

        rb.linearVelocityX = moveDirection.x * movingSpeed;

        if (useGroundCheck && !IsGroundAhead()) shouldFlip = true;

        if (useWallCheck && IsWallAhead()) shouldFlip = true;

        if (shouldFlip)
        {
            FlipDirection();
        }
    }

    bool IsGroundAhead()
    {
        Vector2 checkPosition = groundCheck.position;
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, checkDistance, groundLayer);

        if (debugMode)
        {
            Debug.DrawRay(checkPosition, Vector2.down * checkDistance, Color.green);
            Debug.Log($"Ground check at {checkPosition} hit: {hit.collider != null}");
        }

        return hit.collider != null;
    }

    bool IsWallAhead()
    {
        Vector2 checkPosition = wallCheck.position;
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, moveDirection, checkDistance, groundLayer);

        if (debugMode)
        {
            Debug.DrawRay(checkPosition, moveDirection * checkDistance, Color.red);
            Debug.Log($"Wall check at {checkPosition} hit: {hit.collider != null}");
        }

        return hit.collider != null;
    }

    void FlipDirection()
    {
        moveDirection *= -1;

        // Flip the sprite
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
