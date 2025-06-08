using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // target object to follow
    public float smoothSpeed = 0.125f; // moving speed of the camera
    public Vector2 minPosition; // camera boundary (left, bottom)
    public Vector2 maxPosition; // camera boundary (right, top)

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // boundary clamping
        float clampedX = Mathf.Clamp(smoothedPosition.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minPosition.y, maxPosition.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        Debug.Log($"Camera Position: {transform.position}, Target Position: {target.position}");
    }
}
