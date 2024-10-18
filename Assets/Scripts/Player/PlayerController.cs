using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    Animator animator;
    public Transform quadTransform;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        animator = GetComponent<Animator>();



        animator.SetBool("idle", true);

    }

    void Update()
    {
        // Check if the mouse button is pressed down or held
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) // Mouse button clicked once
        {
            animator.SetBool("attack", true);
            animator.SetBool("idle", false);

            Vector3 playerPosition = transform.position;
            playerPosition.x = quadTransform.position.x; 
            transform.position = playerPosition;


        }
      
        else if (Input.GetMouseButtonUp(0)) // Mouse button released
        {
            animator.SetBool("attack", false); // Return to Idle
            animator.SetBool("idle", true);

            transform.position = initialPosition;

        }
    }
}
