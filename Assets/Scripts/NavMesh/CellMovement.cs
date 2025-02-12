using UnityEngine;
using UnityEngine.AI;

public class CellMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    

    public Transform parentLocation;
   public Transform centerPoint;
   public Transform edgePoint;
   public float innerRadi;
    
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
            //Vector3 targetPos = target.position;
            Vector3 targetPos = GetRandomNavMeshPos();
            
            
            
            agent.SetDestination(targetPos);
        }
    }

    Vector3 GetRandomNavMeshPos()
    {
        innerRadi = 0.8f;
        Vector3 center = centerPoint.position; 
        float maxRadius = (center - edgePoint.position).magnitude;
        float minRadius = maxRadius * innerRadi;
        print("Parent: " + parentLocation.rotation.eulerAngles.ToString());
        
        for (int i = 0; i < 100; i++) // Try 10 times to find a valid point
        {
            Vector3 randomDir = Random.insideUnitCircle.normalized;
            Quaternion rotation = Quaternion.Euler(parentLocation.rotation.eulerAngles.x + 90, parentLocation.rotation.eulerAngles.y, parentLocation.rotation.eulerAngles.z);
            print("Calculated: " + rotation.ToString());
            //Quaternion rotation = Quaternion.Euler(90, 0, 0);
            randomDir = rotation * randomDir;
            float randomRadius = Mathf.Sqrt(Random.Range(minRadius * minRadius, maxRadius * maxRadius));
            Vector3 randomPos = center + randomDir * randomRadius;
            
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, maxRadius, NavMesh.AllAreas))
            {
                return hit.position; // Found a valid point on the NavMesh
            }
        }

        return center;
    }
}
