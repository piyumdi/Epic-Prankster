using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [Range(0, 360)]  public float viewAngle = 90f;
    public float ViewDistance = 15f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;


    [SerializeField] private float DetectionDistance = 3f;
    [SerializeField] private Transform EnemyEye;
    [SerializeField] private Transform Target;
    private GameObject spottedPlayer; 
    public GameObject _spottedPlayer { get { return spottedPlayer; } }


    private NavMeshAgent agent;
    private Transform agentTransform;
    private Animator anim;
    private float rotationSpeed;
    public bool playerBlockView;

    private List<Transform> points = new List<Transform>();



    private void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.updateRotation = false;
        rotationSpeed = agent.angularSpeed;
        agentTransform = agent.transform;


        GameObject[] pointsObject = GameObject.FindGameObjectsWithTag("Points");
        foreach (GameObject point in pointsObject)
            points.Add(point.transform);

        agent.SetDestination(points[Random.Range(0, points.Count)].position);
    }
    private void Update()
    {
        if (IsInView())
        {
            MoveToTarget();

        }
        else
        {
            Patrol();
        }

        Rotate();
        DrawViewState();
    }

  

    private bool IsInView() 
    {
        float realAngle = Vector3.Angle(EnemyEye.forward, Target.position - EnemyEye.position);
        float distanceToPlayer = Vector3.Distance(Target.transform.position, agent.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(EnemyEye.transform.position, Target.position - EnemyEye.position, out hit, ViewDistance))
        {
            if (realAngle < viewAngle / 2f && Vector3.Distance(EnemyEye.position, Target.position) <= ViewDistance && hit.transform == Target.transform)
            {
                return true;
            }
        }
        else if (distanceToPlayer <= DetectionDistance) return true;

        return false;
    }
    private void Rotate() 
    {
        if (agent.velocity == Vector3.zero) return;
        agentTransform.rotation = Quaternion.RotateTowards
            (
                agentTransform.rotation,
                Quaternion.LookRotation(agent.velocity, Vector3.up),
                rotationSpeed * Time.deltaTime
            );

    }
    private void MoveToTarget()
    {
        agent.SetDestination(Target.position);
    }
    private void DrawViewState()
    {
        Vector3 left = EnemyEye.position + Quaternion.Euler(new Vector3(0, viewAngle / 2f, 0)) * (EnemyEye.forward * ViewDistance);
        Vector3 right = EnemyEye.position + Quaternion.Euler(-new Vector3(0, viewAngle / 2f, 0)) * (EnemyEye.forward * ViewDistance);
        Debug.DrawLine(EnemyEye.position, left, Color.yellow);
        Debug.DrawLine(EnemyEye.position, right, Color.yellow);
    }

    private void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(points[Random.Range(0, points.Count)].position);
        }
    }

    public Vector2 DirFromAngle(float angleDeg)
    {
        angleDeg += transform.eulerAngles.y;
        return new Vector2(Mathf.Cos(angleDeg * Mathf.Deg2Rad), Mathf.Sin(angleDeg * Mathf.Deg2Rad));
    }

    
}