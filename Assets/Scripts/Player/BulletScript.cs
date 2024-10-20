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

        if (isEnemy == false)
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

            other.gameObject.GetComponent<EnemyController>().TakeDamage();

            Destroy(this.gameObject);
        }

        if (other.gameObject.tag == "Player" && isEnemy)
        {
            
            //other.gameObject.GetComponent<PlayerController>().TakeDamage();
            Destroy(this.gameObject);
        }
    }
}
