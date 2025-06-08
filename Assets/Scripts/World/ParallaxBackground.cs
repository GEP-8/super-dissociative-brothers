using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform target; // 따라갈 카메라 또는 플레이어
    public float parallaxSpeed = 0.6f; // 얼마나 느리게 따라갈지 (0~1)

    private Vector3 initialOffset;

    void Start()
    {
        if (target == null)
            target = Camera.main.transform;

        initialOffset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + initialOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, parallaxSpeed * Time.deltaTime);
    }
}
