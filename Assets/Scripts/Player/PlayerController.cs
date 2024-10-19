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

    public bool hasShot = false;


    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        animator = GetComponent<Animator>();
        animator.SetBool("idle", true);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemyList.Add(enemy.transform);
        }

    }

    void Update()
    {
        #region PLAYER POSITIONS AND SHOOT
        /*if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) 
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

            }


        }*/

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        
        if (Input.GetMouseButtonDown(0))
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
            }

            if(hasShot == true)
            {
                ShootAtEnemy();
            }
            
        }

        
        if (stateInfo.IsName("SHOOT")) 
        {
            if (stateInfo.normalizedTime < 0.1f) // Animation just started a new loop
            {
                hasShot = false;
            }

            if (stateInfo.normalizedTime >= 0.9f && !hasShot)
            {
                //ShootAtEnemy();
                hasShot = true; 
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

    public void ClosestVariable()
    {

        if (enemyList.Count > 0)
        {
            closestEnemy = GetClosestEnemy(enemyList);
        }

    }

}