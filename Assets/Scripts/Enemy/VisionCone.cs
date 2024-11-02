/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public Material VisionConeMaterial;
    public float VisionRange = 50f;  // Maximum distance for detection
    public float VisionAngle = 90f;  // Vision angle
    public LayerMask VisionObstructingLayer;  // Obstacles layer
    public LayerMask PlayerLayer; // Player layer mask
    public int VisionConeResolution = 120;  // Resolution of the vision cone mesh

    private Mesh VisionConeMesh;
    private MeshFilter MeshFilter_;

    void Start()
    {
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = VisionConeMaterial;

        MeshFilter_ = gameObject.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
    }

    void Update()
    {
        DrawVisionCone();
    }

    void DrawVisionCone()
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] vertices = new Vector3[VisionConeResolution + 1];
        vertices[0] = Vector3.zero; // Center of the cone

        float currentAngle = -VisionAngle / 2;
        float angleIncrement = VisionAngle / (VisionConeResolution - 1);

        Vector3 coneOrigin = transform.position + Vector3.up * 0.5f; // Origin for rays, slightly above ground

        for (int i = 0; i < VisionConeResolution; i++)
        {
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
            Vector3 vertexPosition;

            // Cast ray to detect objects
            RaycastHit hit;
            if (Physics.Raycast(coneOrigin, transform.TransformDirection(direction), out hit, VisionRange, VisionObstructingLayer | PlayerLayer))
            {
                vertexPosition = transform.InverseTransformPoint(hit.point); // Place vertex at hit point

                // Check if the hit object is the player
                if ((PlayerLayer.value & (1 << hit.collider.gameObject.layer)) > 0)
                {
                    Debug.Log("Player detected within vision cone!");
                    Debug.DrawRay(coneOrigin, transform.TransformDirection(direction) * hit.distance, Color.red);
                }
                else
                {
                    Debug.Log("Obstacle blocking vision cone.");
                    Debug.DrawRay(coneOrigin, transform.TransformDirection(direction) * hit.distance, Color.green);
                }
            }
            else
            {
                vertexPosition = direction * VisionRange; // Max range if no obstacle
                Debug.DrawRay(coneOrigin, transform.TransformDirection(direction) * VisionRange, Color.yellow); // Visualize open line of sight
            }

            vertices[i + 1] = vertexPosition;
            currentAngle += angleIncrement;
        }

        // Create triangles for the cone mesh
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = (j + 2) % VisionConeResolution + 1;
        }

        VisionConeMesh.Clear();
        VisionConeMesh.vertices = vertices;
        VisionConeMesh.triangles = triangles;
        MeshFilter_.mesh = VisionConeMesh;

        // Slightly raise the cone position
        transform.localPosition = new Vector3(0, 0.5f, 0);
    }
}
*/

using System.Collections;
using UnityEngine;
using Yunash.UI;

public class VisionCone : MonoBehaviour
{
    public Material VisionConeMaterial;
    public float VisionRange = 50f;          // Maximum distance for detection
    public float VisionAngle = 90f;          // Vision angle
    public LayerMask VisionObstructingLayer; // Obstacles layer
    public LayerMask PlayerLayer;            // Player layer mask
    public int VisionConeResolution = 120;   // Resolution of the vision cone mesh

    private Mesh VisionConeMesh;
    private MeshFilter MeshFilter_;
    private bool playerDetected = false;
    private float detectionTime = 0f;
    private float maxDetectionDuration = 2f; // Time before game over

    private LoginCanvas loginCanvas; // Reference to the LoginCanvas for game-over panel

    void Start()
    {
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = VisionConeMaterial;

        MeshFilter_ = gameObject.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();

        loginCanvas = FindObjectOfType<LoginCanvas>(); // Find the LoginCanvas instance
    }

    void Update()
    {
        DrawVisionCone();

        if (playerDetected)
        {
            detectionTime += Time.deltaTime;
            if (detectionTime >= maxDetectionDuration)
            {
                Debug.Log("GameOver.");
                TriggerGameOver();
            }
        }
        else
        {
            detectionTime = 0f; // Reset detection time if player is not detected
        }
    }

    void TriggerGameOver()
    {
        if (loginCanvas != null)
        {
            loginCanvas.ShowGameOverPanel(); // Display the game-over panel
            //Time.timeScale = 0; // Freeze the game
        }
        else
        {
            Debug.LogError("LoginCanvas not found!");
        }
    }

    void DrawVisionCone()
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] vertices = new Vector3[VisionConeResolution + 1];
        vertices[0] = Vector3.zero; // Center of the cone

        float currentAngle = -VisionAngle / 2;
        float angleIncrement = VisionAngle / (VisionConeResolution - 1);

        Vector3 coneOrigin = transform.position + Vector3.up * 0.5f; // Origin for rays, slightly above ground

        playerDetected = false;

        for (int i = 0; i < VisionConeResolution; i++)
        {
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
            Vector3 vertexPosition;

            // Cast ray to detect objects
            RaycastHit hit;
            if (Physics.Raycast(coneOrigin, transform.TransformDirection(direction), out hit, VisionRange, VisionObstructingLayer | PlayerLayer))
            {
                vertexPosition = transform.InverseTransformPoint(hit.point); // Place vertex at hit point

                // Check if the hit object is the player
                if ((PlayerLayer.value & (1 << hit.collider.gameObject.layer)) > 0)
                {
                    playerDetected = true;
                   // Debug.Log("Player detected within vision cone!");
                    Debug.DrawRay(coneOrigin, transform.TransformDirection(direction) * hit.distance, Color.red);
                }
                else
                {
                   // Debug.Log("Obstacle blocking vision cone.");
                    Debug.DrawRay(coneOrigin, transform.TransformDirection(direction) * hit.distance, Color.green);
                }
            }
            else
            {
                vertexPosition = direction * VisionRange; // Max range if no obstacle
                Debug.DrawRay(coneOrigin, transform.TransformDirection(direction) * VisionRange, Color.yellow); // Visualize open line of sight
            }

            vertices[i + 1] = vertexPosition;
            currentAngle += angleIncrement;
        }

        // Create triangles for the cone mesh
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = (j + 2) % VisionConeResolution + 1;
        }

        VisionConeMesh.Clear();
        VisionConeMesh.vertices = vertices;
        VisionConeMesh.triangles = triangles;
        MeshFilter_.mesh = VisionConeMesh;

        // Slightly raise the cone position
        transform.localPosition = new Vector3(0, 0.5f, 0);
    }
}
