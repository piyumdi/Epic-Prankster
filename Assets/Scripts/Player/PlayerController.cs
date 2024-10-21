using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yunash.Game;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    public Transform quadTransform;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    public List<Transform> enemyList = new List<Transform>();
    public Transform closestEnemy;

    public float bulletSpeed = 10f;
    public float fireRate = 0.5f; 
    private float nextFireTime = 0f; 
    public bool hasShot = false;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        animator = GetComponent<Animator>();
        animator.SetBool("idle", true);

        // Initialize the enemy list
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemyList.Add(enemy.transform);
        }
    }

    void Update()
    {
        #region PLAYER CONTROLLER AND BULLET
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Fire bullets when the mouse is pressed (Input.GetMouseButton)
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime) // Check if enough time has passed to fire again
        {
            // Set attack animation
            animator.SetBool("attack", true);
            animator.SetBool("idle", false);

            closestEnemy = GetClosestEnemy(enemyList); // Find the closest enemy

            if (closestEnemy != null)
            {
                transform.LookAt(closestEnemy); // Face the closest enemy

                Vector3 playerPosition = transform.position;
                playerPosition.x = closestEnemy.position.x; // Optional: Only move on the X axis
                transform.position = playerPosition;

                ShootAtEnemy(); // Shoot at the closest enemy
                nextFireTime = Time.time + fireRate; // Update next fire time based on fire rate
            }
        }

        // Reset the hasShot flag if the animation starts a new loop
        if (stateInfo.IsName("SHOOT"))
        {
            if (stateInfo.normalizedTime < 0.1f) // Animation just started a new loop
            {
                hasShot = false;
            }
        }

        // Reset to idle when mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("attack", false);
            animator.SetBool("idle", true);

            transform.position = initialPosition;
            transform.rotation = initialRotation;
            hasShot = false;
        CheckForLevelComplete();
        }
        #endregion

    }

    #region ClosestEnemy


    // Find the closest enemy
    Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    #endregion
    

    #region Shoot Enemy
    void ShootAtEnemy()
    {
        if (closestEnemy != null && bulletPrefab != null && bulletSpawnPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Debug.Log("bullet");
            Vector3 direction = (closestEnemy.position - bulletSpawnPoint.position).normalized;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
                rb.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
            }

        }
    }

    #endregion

    public void ClosestVariable()
    {
        if (enemyList.Count > 0)
        {
            closestEnemy = GetClosestEnemy(enemyList);
        }
    }

    public void CheckForLevelComplete()
    {
        if (enemyList.Count == 0)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.LevelComplete);
        }
    }
}
