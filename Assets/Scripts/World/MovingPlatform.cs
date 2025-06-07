using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Make the player a child of the platform to move with it
            other.transform.SetParent(transform);
        }
    }
    
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Remove the player from being a child of the platform
            other.transform.SetParent(null);
        }
    }
}
