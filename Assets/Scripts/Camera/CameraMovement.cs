using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;

    private Vector3 target;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = gameObject.transform.position - player.position;
    }

    private void Update()
    {
        transform.position = player.position + target;
    }
}
