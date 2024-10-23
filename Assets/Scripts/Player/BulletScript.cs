/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float moveSpeed = 10f;
    public bool isEnemy;
    private Transform target;
    private PlayerController getClosestEnemy;
    private Vector3 moveDirection;
    private float initialY;

    void Start()
    {
        initialY = transform.position.y;

        if (!isEnemy)
        {
            getClosestEnemy = FindObjectOfType<PlayerController>();
            getClosestEnemy.ClosestVariable();
            target = getClosestEnemy.closestEnemy;
        }
        else
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        if (target != null)
        {
            moveDirection = (target.position - transform.position).normalized;
            moveDirection.y = 0; // Lock the Y axis
        }

        Destroy(gameObject, 2f); // Destroy the bullet after 2 seconds
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                     new Vector3(target.position.x, initialY, target.position.z),
                                                     moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !isEnemy)
        {
            
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.OnHit(); 
                enemyController.TakeDamage(); 
            }
            Destroy(this.gameObject); 
        }

        if (other.gameObject.tag == "Player" && isEnemy)
        {
            
            Destroy(this.gameObject);
        }
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float moveSpeed = 10f;
    public bool isEnemy; // Indicates whether this bullet is shot by an enemy
    private Transform target;
    private PlayerController getClosestEnemy;
    private Vector3 moveDirection;
    private float initialY;

    void Start()
    {
        initialY = transform.position.y;

        // If the bullet is fired by the player, find the closest enemy
        if (!isEnemy)
        {
            getClosestEnemy = FindObjectOfType<PlayerController>();
            getClosestEnemy.ClosestVariable(); // Calculate closest enemy
            target = getClosestEnemy.closestEnemy; // Assign the target as the closest enemy
        }
        else
        {
            // If fired by the enemy, the target is the player
            target = GameObject.FindWithTag("Player").transform;
        }

        // Calculate the direction of movement
        if (target != null)
        {
            moveDirection = (target.position - transform.position).normalized;
            moveDirection.y = 0; // Lock movement to the XZ plane (ignore Y axis)
        }

        // Destroy the bullet after 2 seconds to prevent it from lingering indefinitely
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        // If the target exists, move towards the target
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                     new Vector3(target.position.x, initialY, target.position.z),
                                                     moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the bullet hits the enemy and it's fired by the player
        if (other.gameObject.tag == "Enemy" && !isEnemy)
        {
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.OnHit();     // Trigger the enemy's hit behavior
                enemyController.TakeDamage(); // Apply damage to the enemy
            }
            Destroy(this.gameObject); // Destroy the bullet after it hits the enemy
        }

        // If the bullet hits the player and it's fired by the enemy
        if (other.gameObject.tag == "Player" && isEnemy)
        {
            Destroy(this.gameObject); // Destroy the bullet upon hitting the player
        }
    }
}
