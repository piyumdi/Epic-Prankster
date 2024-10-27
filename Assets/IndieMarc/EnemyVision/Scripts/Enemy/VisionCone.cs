using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yunash.Game;

namespace IndieMarc.EnemyVision
{
    public class VisionCone : MonoBehaviour
    {
        [Header("Linked Enemy")]
        public EnemyVision target;

        [Header("Vision")]
        public float vision_angle = 30f;
        public float vision_range = 5f;
        public float vision_near_range = 3f;
        public LayerMask obstacle_mask = ~(0);
        public bool show_two_levels = false;

        [Header("Material")]
        public Material cone_material;
        public Material cone_far_material;
        public int sort_order = 1;

        [Header("Optimization")]
        public int precision = 60;
        public float refresh_rate = 0f;

        [Header("Player Detection")]
        public Transform player; // Reference to the player's transform
        public LayerMask player_mask;
       

        private MeshRenderer render;
        private MeshFilter mesh;
        private MeshRenderer render_far;
        private MeshFilter mesh_far;
        private float timer = 0f;

        private void Awake()
        {
            render = gameObject.AddComponent<MeshRenderer>();
            mesh = gameObject.AddComponent<MeshFilter>();
            render.sharedMaterial = cone_material;
            render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            render.receiveShadows = false;
            render.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            render.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            render.allowOcclusionWhenDynamic = false;
            render.sortingOrder = sort_order;

            if (show_two_levels)
            {
                GameObject far_cone = new GameObject("FarCone");
                far_cone.transform.position = transform.position;
                far_cone.transform.SetParent(gameObject.transform);

                render_far = far_cone.AddComponent<MeshRenderer>();
                mesh_far = far_cone.AddComponent<MeshFilter>();
                render_far.sharedMaterial = cone_far_material;
                render_far.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                render_far.receiveShadows = false;
                render_far.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                render_far.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                render_far.allowOcclusionWhenDynamic = false;
                render.sortingOrder = sort_order;
            }
        }

        private void Start()
        {
            InitMesh(mesh, false);

            if (show_two_levels)
                InitMesh(mesh_far, true);
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = target.eye.transform.position;
            transform.rotation = target.transform.rotation;

            if (timer > refresh_rate)
            {
                timer = 0f;

                float range = vision_range;
                if (show_two_levels)
                    range = vision_near_range;

                UpdateMainLevel(mesh, range);

                if (show_two_levels)
                    UpdateFarLevel(mesh_far, vision_near_range, vision_range - vision_near_range);
            }

            DetectPlayer();
        }

        private void DetectPlayer()
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if player is within vision angle and range
            if (Vector3.Angle(transform.forward, directionToPlayer) < vision_angle / 2f && distanceToPlayer <= vision_range)
            {
                // Raycast to ensure player is not behind an obstacle
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacle_mask))
                {
                    // Player detected, trigger Game Over
                    GameOver();
                }
            }
        }

        private void GameOver()
        {
            Debug.Log("Game Over! Player Detected");
            GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
            Time.timeScale = 0f; 
        }

        private void InitMesh(MeshFilter mesh, bool far)
        {
            // Initialization of vision cone mesh (same as before)
        }

        private void UpdateMainLevel(MeshFilter mesh, float range)
        {
            // Update main level of vision cone (same as before)
        }

        private void UpdateFarLevel(MeshFilter mesh, float offset, float range)
        {
            // Update far level of vision cone (same as before)
        }
    }
}
