using System.Collections;
using UnityEngine;

public class MoveAB : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float movingSpeed = 1.0f;
    public float waitTime = 1.0f;
    public bool firstToPointA = true;

    private Transform targetPoint;
    private bool isWaiting = false;
    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Point A and Point B must be assigned in the inspector.");
            return;
        }

        targetPoint = firstToPointA ? pointA : pointB;
    }
    
    void Update()
    {
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (!isWaiting)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, movingSpeed * Time.deltaTime);
        }
        
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.01f)
        {
            if (!isWaiting)
            {
                StartCoroutine(WaitThenSwitchTarget());
            }
        }
    }

    IEnumerator WaitThenSwitchTarget()
    {
        // Wait at the target point
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        // Switch target point
        targetPoint = (targetPoint == pointA) ? pointB : pointA;
        isWaiting = false;
    }
}
