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
            visionCone.VisionAngle = 90f; // Start with a full cone
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

        // Make sure the vision cone follows the enemy's new rotation, but stays 1f above the enemy
        if (visionCone != null)
        {
            visionCone.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
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

        // Gradually decrease the vision angle from 90 to 30 over 3 seconds
        float elapsedTime = 0f;
        float duration = 5f;
        float startAngle = 90f;
        float targetAngle = 30f;

        while (elapsedTime < duration)
        {
            if (visionCone != null)
            {
                visionCone.VisionAngle = Mathf.Lerp(startAngle, targetAngle, elapsedTime / duration); // Interpolate the vision angle
            }
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // After 3 seconds, turn the enemy back
        TurnBack();

        // Reset vision cone angle to 90 after turning back
        if (visionCone != null)
        {
            visionCone.VisionAngle = 90f;
        }

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
    public GameObject player; // Reference to the player object
    public GameOverManager gameOverManager; // Reference to the GameOverManager script

    private bool isScanning = false; // To track if the enemy is scanning
    private bool isPlayerInVision = false; // Track if the player is within the reduced vision cone

    void Start()
    {
        targetRotation = transform.rotation;
        lastHitTime = -turnBackDelay;

        UpdateLivesText();

        // Disable the vision cone at the start
        if (visionCone != null)
        {
            visionCone.enabled = false;
            visionCone.VisionAngle = 90f; // Start with a full cone
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

        // Detect player in vision cone during scanning
        if (facingPlayer && visionCone != null && isScanning)
        {
            DetectPlayerInVisionCone();
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

        // Make sure the vision cone follows the enemy's new rotation, but stays 1f above the enemy
        if (visionCone != null)
        {
            visionCone.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
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

        // Gradually decrease the vision angle from 90 to 30 over 3 seconds
        float elapsedTime = 0f;
        float duration = 5f;
        float startAngle = 90f;
        float targetAngle = 30f;

        while (elapsedTime < duration)
        {
            if (visionCone != null)
            {
                visionCone.VisionAngle = Mathf.Lerp(startAngle, targetAngle, elapsedTime / duration); // Interpolate the vision angle
            }
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // After 3 seconds, turn the enemy back
        TurnBack();

        // Reset vision cone angle to 90 after turning back
        if (visionCone != null)
        {
            visionCone.VisionAngle = 90f;
        }

        isScanning = false;
    }

    // Detect if the player is within the reduced vision cone (angle check)
    private void DetectPlayerInVisionCone()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < visionCone.VisionAngle)
        {
            isPlayerInVision = true; // Player is within the vision cone angle

            // If both vision cone hits and the angle is within 10 degrees
            if (visionCone.VisionAngle <= 10f && isPlayerInVision)
            {
                TriggerGameOver(); // Trigger game over
            }
        }
        else
        {
            isPlayerInVision = false; // Player is outside the vision cone
        }
    }

    private void TriggerGameOver()
    {
        Debug.Log("Game Over! Player detected by enemy.");
        if (gameOverManager != null)
        {
            gameOverManager.GameOver(); // Trigger the GameOver in the GameOverManager
        }
    }
}
