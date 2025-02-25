using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This Scripts is the main controller for spawning and controlling the cells on the navmesh
/// </summary>
public class CellController : MonoBehaviour
{
    
    
    //Cell variables
    
    public GameObject cellPrefab;
    public GameObject cellPrefab2;
    
    public List<GameObject> cells = new List<GameObject>();
    
    public int cellCount;
    public int stage;
    
    //Navmesh Variables
    public NavMeshAgent agent;
    
    public Transform parentLocation;
    public Transform centerPoint;
    public Transform edgePoint;
    public float innerRadi;
    
    //Timing variables
    public SharedTimingData sharedTimingData;
    /*
     * Stages of animation:
     *  Stage1: The muscle layer contracts
     *  Stage2: Some cells starts to show up.
     *  Stage3: The tissue gets irritated and contracts, slime starts showing up
     *
     * Stage 4: Pause for explanation or similar mid animation
     *
     * Stage 5: Medicine is administered
     * Stage 6: Quick acting medicine makes the muscle layer expand/return to original size.
     * Stage 7: Cells go away over time and tissue layer calms down.
     */
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
