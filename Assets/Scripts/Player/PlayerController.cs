using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        #region PLAYER MOUSE CONTROLLER WITH BULLET
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime) 
        {
            
            animator.SetBool("attack", true);
            animator.SetBool("idle", false);

            closestEnemy = GetClosestEnemy(enemyList); 

            if (closestEnemy != null)
            {
                transform.LookAt(closestEnemy); 

                Vector3 playerPosition = transform.position;
                playerPosition.x = closestEnemy.position.x; 
                transform.position = playerPosition;

                ShootAtEnemy(); 
                nextFireTime = Time.time + fireRate; 
            }
        }

        
        if (stateInfo.IsName("SHOOT"))
        {
            if (stateInfo.normalizedTime < 0.1f) 
            {
                hasShot = false;
            }
        }

        
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("attack", false);
            animator.SetBool("idle", true);

            transform.position = initialPosition;
            transform.rotation = initialRotation;
            hasShot = false;
        }

        #endregion
    }


    #region CLOSEST ENEMY 
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


    #region SHOOT ENEMY
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
}
