using UnityEngine;

public class FloatingEntity : MonoBehaviour
{
    public float floatAmplitude = 0.5f; // Amplitude of the floating motion
    public float floatFrequency = 1.0f; // Frequency of the floating motion
    
    private Vector3 startPosition; // Initial position of the entity
    
    void Start()
    {
        // Store the initial position of the entity
        startPosition = transform.position;
    }
    
    void Update()
    {
        // Calculate position offset based on sine wave for floating effect
        float offset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        
        // Update the position of the entity
        transform.position = startPosition + new Vector3(0f, offset, 0f);
    }
}
