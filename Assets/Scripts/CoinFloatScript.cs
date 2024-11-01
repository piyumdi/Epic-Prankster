using UnityEngine;

public class CoinFloatScript : MonoBehaviour
{
    public float floatSpeed = 1f;       // Speed of floating up and down
    public float floatHeight = 0.5f;    // Maximum height of the floating effect

    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial position of the coin
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave for smooth floating motion
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // Apply the new position to the coin container
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}

