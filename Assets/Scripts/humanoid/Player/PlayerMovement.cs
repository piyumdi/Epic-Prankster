using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{

    private NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("First if");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
