/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public Material VisionConeMaterial;
    public float VisionRange = 10f;  // Make sure this is a reasonable distance for detection
    public float VisionAngle = 90f;  // Set a default vision angle here or modify externally
    public LayerMask VisionObstructingLayer;  // Obstacles layer
    public int VisionConeResolution = 120;  // Resolution of the vision cone mesh

    private Mesh VisionConeMesh;
    private MeshFilter MeshFilter_;

    void Start()
    {
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = VisionConeMaterial;

        MeshFilter_ = gameObject.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();

        // Set initial vision angle
        VisionAngle = 90f;
    }

    void Update()
    {
        DrawVisionCone();
    }

    void DrawVisionCone()
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] vertices = new Vector3[VisionConeResolution + 1];
        vertices[0] = Vector3.zero; // Tip of the cone (center)

        float currentAngle = -VisionAngle / 2;
        float angleIncrement = VisionAngle / (VisionConeResolution - 1);

        // Adjusted origin position for the raycast
        Vector3 coneOrigin = transform.position + Vector3.up * 0.5f; // Slightly raise the origin

        for (int i = 0; i < VisionConeResolution; i++)
        {
            float rad = currentAngle * Mathf.Deg2Rad; // Angle to radians
            Vector3 direction = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)); // Direction vector
            Vector3 vertexPosition;

            // Check for obstacles in the direction
            RaycastHit hit;
            if (Physics.Raycast(coneOrigin, transform.TransformDirection(direction), out hit, VisionRange, VisionObstructingLayer))
            {
                vertexPosition = transform.InverseTransformPoint(hit.point); // Place vertex at hit point
            }
            else
            {
                vertexPosition = direction * VisionRange; // Place vertex at max range if no obstacle
            }

            vertices[i + 1] = vertexPosition; // Assign vertex position
            currentAngle += angleIncrement;
        }

        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;         // Center of the cone
            triangles[i + 1] = j + 1; // Current vertex
            triangles[i + 2] = (j + 2) % VisionConeResolution + 1; // Next vertex
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
                    Debug.DrawRay(coneOrigin, transform.TransformDirection(direction) * hit.distance, Color.green);
                }
                else
                {
                    Debug.Log("Obstacle blocking vision cone.");
                    Debug.DrawRay(coneOrigin, transform.TransformDirection(direction) * hit.distance, Color.red);
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

