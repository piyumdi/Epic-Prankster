
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float rotationSpeed = 180f; // Degrees per second
    public float turnBackDelay = 3f;   // Time to turn back after no hits
    private bool hit = false;          // Bool to check if the enemy was hit
    private float lastHitTime;         // Time of the last hit
    private Quaternion targetRotation; // Target rotation
    private bool facingPlayer = false; // Is the enemy currently facing the player?

    void Start()
    {
        targetRotation = transform.rotation; // Initial rotation (facing away from player)
        lastHitTime = -turnBackDelay; // Initialize so that the enemy doesn't immediately turn back
    }

    void Update()
    {
        // If the enemy has been hit and 3 seconds have passed with no hits, turn the enemy back
        if (facingPlayer && Time.time - lastHitTime >= turnBackDelay)
        {
            TurnBack();
        }

        // Rotate the enemy when needed
        if (hit)
        {
            RotateEnemy();
        }
    }

    public void OnHit()
    {
        // If the enemy is not yet facing the player, turn the enemy toward the player
        if (!facingPlayer)
        {
            targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0); // Rotate by 180 degrees
            hit = true; // Start rotating
            facingPlayer = true; // Enemy is now facing the player
        }

        // Reset the last hit time to keep the enemy facing the player as long as the player is shooting
        lastHitTime = Time.time;
    }

    private void RotateEnemy()
    {
        // Smoothly rotate the enemy towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // If the rotation is close to the target, stop rotating
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            hit = false; // Stop rotating after completing the turn
        }
    }

    private void TurnBack()
    {
        // Set the target rotation to turn the enemy back (facing away from the player)
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0);
        facingPlayer = false; // The enemy is no longer facing the player
        hit = true; // Start rotating the enemy back
    }
}
