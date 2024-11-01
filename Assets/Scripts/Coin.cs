using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotationSpeed = 100f; // Speed of rotation

    void Update()
    {
        // Rotate the coin around its Y-axis every frame
        transform.Rotate( 0, 0, rotationSpeed * Time.deltaTime);
    }
}
