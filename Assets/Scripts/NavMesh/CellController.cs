using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CellController : MonoBehaviour
{
    /// <summary>
    /// This Scripts is the main controller for spawning and controlling the cells on the navmesh
    /// </summary>
    
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
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
