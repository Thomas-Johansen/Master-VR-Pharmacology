using UnityEngine;
using UnityEngine.AI;

public class CellMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    
    bool go = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (go == false)
        {
            go = true;
            Vector3 targetPos = target.position;
            Vector3 avalidNavMeshPos;
            
            agent.SetDestination(targetPos);
        }
    }
}
