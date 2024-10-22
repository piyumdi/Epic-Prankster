/*
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float rotationSpeed = 180f; // Degrees per second
    public float turnBackDelay = 3f;   // Time to turn back after no hits
    private bool hit = false;          // Bool to check if the enemy was hit
    private float lastHitTime;         // Time of the last hit
    private Quaternion targetRotation; // Target rotation
    private bool facingPlayer = false; // Is the enemy currently facing the player?
    public GameObject deadCount;

    public VisionCone visionCone; // Reference to the VisionCone script
    public int lives = 5;
    public TMP_Text livesText;
    public bool isEnemy = true;

    private bool isScanning = false; // To track if the enemy is scanning

    void Start()
    {
        targetRotation = transform.rotation;
        lastHitTime = -turnBackDelay;

        UpdateLivesText();

        // Disable the vision cone at the start
        if (visionCone != null)
        {
            visionCone.enabled = false;
        }
    }

    void Update()
    {
        // If the enemy is facing the player and 3 seconds have passed since the last hit, turn back
        if (facingPlayer && !isScanning && Time.time - lastHitTime >= turnBackDelay)
        {
            StartCoroutine(ScanRoom());
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

            // Enable the vision cone when hit
            if (visionCone != null)
            {
                visionCone.enabled = true;
            }
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

        // Disable the vision cone when the enemy turns back
        if (visionCone != null)
        {
            visionCone.enabled = false;
        }
    }

    public void TakeDamage()
    {
        lives -= 1;
        UpdateLivesText();

        if (lives <= 0)
        {
            Die();
        }
    }

    void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = lives.ToString();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
        Destroy(deadCount);
    }

    // Coroutine to handle the enemy scanning the room for 3 seconds
    IEnumerator ScanRoom()
    {
        isScanning = true;

        // Wait for 3 seconds while scanning the room
        yield return new WaitForSeconds(3f);

        // After 3 seconds, turn the enemy back
        TurnBack();

        isScanning = false;
    }
}
*/


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float rotationSpeed = 180f; // Degrees per second
    public float turnBackDelay = 3f;   // Time to turn back after no hits
    private bool hit = false;          // Bool to check if the enemy was hit
    private float lastHitTime;         // Time of the last hit
    private Quaternion targetRotation; // Target rotation
    private bool facingPlayer = false; // Is the enemy currently facing the player?
    public GameObject deadCount;

    public VisionCone visionCone; // Reference to the VisionCone script
    public int lives = 5;
    public TMP_Text livesText;
    public bool isEnemy = true;

    private bool isScanning = false; // To track if the enemy is scanning

    void Start()
    {
        targetRotation = transform.rotation;
        lastHitTime = -turnBackDelay;

        UpdateLivesText();

        // Disable the vision cone at the start
        if (visionCone != null)
        {
            visionCone.enabled = false;
        }
    }

    void Update()
    {
        // If the enemy is facing the player and 3 seconds have passed since the last hit, start scanning the room
        if (facingPlayer && !isScanning && Time.time - lastHitTime >= turnBackDelay)
        {
            StartCoroutine(ScanRoom());
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

            // Enable the vision cone when hit
            if (visionCone != null)
            {
                visionCone.enabled = true;
            }
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

        // Make sure the vision cone follows the enemy's new rotation, but stays 0.5f below the enemy
        if (visionCone != null)
        {
            // Position the vision cone 0.5f below the enemy while keeping the same X and Z
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y +1f, transform.position.z);
            visionCone.transform.position = newPosition;

            // Rotate the vision cone to match the enemy's rotation
            visionCone.transform.rotation = transform.rotation;
        }
    }




    private void TurnBack()
    {
        // Set the target rotation to turn the enemy back (facing away from the player)
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0);
        facingPlayer = false; // The enemy is no longer facing the player
        hit = true; // Start rotating the enemy back

        // Disable the vision cone when the enemy turns back
        if (visionCone != null)
        {
            visionCone.enabled = false; // Disable the vision cone after turning back
        }
    }

    public void TakeDamage()
    {
        lives -= 1;
        UpdateLivesText();

        if (lives <= 0)
        {
            Die();
        }
    }

    void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = lives.ToString();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        // Disable the vision cone when the enemy dies
        if (visionCone != null)
        {
            visionCone.enabled = false; // Disable vision cone on death
        }
        Destroy(gameObject);
        Destroy(deadCount);
    }

    // Coroutine to handle the enemy scanning the room for 3 seconds
    IEnumerator ScanRoom()
    {
        isScanning = true;

        // Wait for 3 seconds while scanning the room
        yield return new WaitForSeconds(3f);

        // After 3 seconds, turn the enemy back
        TurnBack();

        isScanning = false;
    }
}
