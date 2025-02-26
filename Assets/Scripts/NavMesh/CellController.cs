using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This Script is the main controller for spawning and controlling the cells on the navmesh
/// </summary>
public class CellController : MonoBehaviour
{

    public GameObject CellLayer;
    public GameObject MedLayer;
    
    //Cell variables
    
    public GameObject cellPrefab;
    public GameObject cellPrefab2;
    
    public List<CellData> cells = new List<CellData>();
    
    public int cellCount;
    public int medCount;
    
    //Med Variables
    public GameObject medPrefab;
    public GameObject medPrefab2;
    
    public List<CellData> meds = new List<CellData>();
    private float _timestamp = 0;
    private bool _hasSpawned = false;
    private int _removeIndex = 0;
    
    
    
    //Navmesh Variables
    public Transform parentLocation;
    public Transform centerPoint;
    public Transform edgePoint;
    public Transform MusclePoint;
    public Transform innerPoint1;
    public Transform innerPoint2;
    public Transform spawnFolder;
    public GameObject obstacle;
    public float obstacleRadius1;
    public float obstacleRadius2;
    
    //Yes i realize these are dumb names 
    private float minRadius;
    private float max_minRadius;
    private float min_minRadius;
    
    //Timing variables
    public SharedTimingData sharedTimingData;
    private float _individualCellTimer;
    
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
        CellLayer.SetActive(true);
        MedLayer.SetActive(false);
        obstacle.SetActive(true);
        
        
        Vector3 center = centerPoint.position; 
        min_minRadius = (center - innerPoint1.position).magnitude;
        max_minRadius = (center - innerPoint2.position).magnitude;
        minRadius = min_minRadius;
        
        obstacle.transform.localScale = new Vector3(obstacleRadius1, obstacleRadius1, obstacleRadius1);
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (sharedTimingData.Stage)
        {
            case 2:
                // Stage 2: Some cells start to show up
                sharedTimingData.Time += (Time.deltaTime * sharedTimingData.Speed); // Updates passed time for both cells and crossection
                _individualCellTimer += (Time.deltaTime * sharedTimingData.Speed);

                if (cells.Count < 10 && _individualCellTimer >= 2)
                {
                    SpawnNewCell1();
                    _individualCellTimer = 0;
                }
                else if (cells.Count == 10)
                {
                    sharedTimingData.Stage = 3;
                }
                break;

            case 3:
                // Stage 3: Updates and moving spawn limits
                sharedTimingData.Time += (Time.deltaTime * sharedTimingData.Speed); // Updates passed time for both cells and crossection
                _individualCellTimer += (Time.deltaTime * sharedTimingData.Speed);

                // Moving spawn limits
                if (sharedTimingData.Contraction <= 1)
                {
                    // Moves inner spawn radius in with the model
                    minRadius = Mathf.Lerp(min_minRadius, max_minRadius, sharedTimingData.Contraction);
                    // Moves obstacle in with the model
                    float scale = Mathf.Lerp(obstacleRadius1, obstacleRadius2, sharedTimingData.Contraction);
                    obstacle.transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    obstacle.SetActive(false); //TODO: COORDINATE BETWEEN THIS
                }

                // Cell Spawning
                if (cells.Count < 30 && _individualCellTimer >= 2)
                {
                    SpawnNewCell2();
                    _individualCellTimer = 0;
                }
                else if (cells.Count == 30)
                {
                    sharedTimingData.Stage = 4; //TODO: AND THIS
                }
                break;

            case 4:
                // Stage 4: Time update only
                sharedTimingData.Time += (Time.deltaTime * sharedTimingData.Speed);
                if (sharedTimingData.Time >= 80f)
                {
                    CellLayer.SetActive(false);
                    MedLayer.SetActive(true);
                    obstacle.SetActive(false);
                    sharedTimingData.Stage = 5;
                }
                break;

            case 5:
                // Stage 5: Medicine spawning
                

                sharedTimingData.Time += (Time.deltaTime * sharedTimingData.Speed);
                _individualCellTimer += (Time.deltaTime * sharedTimingData.Speed);

                if (_hasSpawned == false)
                {
                    foreach (CellData cell in cells)
                    {
                        SpawnNewMed1(cell);
                    }
                    _hasSpawned = true;
                }

                if (sharedTimingData.Time >= 100)
                {
                    sharedTimingData.Stage = 6;
                    _individualCellTimer = 0;
                    foreach (CellData medCell in meds)
                    {
                        medCell.cell.SetActive(false);
                    }
                }
                break;
            case 7:
                sharedTimingData.Time += (Time.deltaTime * sharedTimingData.Speed);
                _individualCellTimer += (Time.deltaTime * sharedTimingData.Speed);

                if (_individualCellTimer > 1 && _removeIndex < cells.Count)
                {
                    cells[_removeIndex].cell.SetActive(false);
                    _individualCellTimer = 0;
                    _removeIndex++;
                }

                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Spawns a new cell and adds the cell and navmeshagent to list
    /// </summary>
    void SpawnNewCell1()
    {
        GameObject newCell = Instantiate(cellPrefab, GetRandomNavMeshPos(), Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), spawnFolder);
        NavMeshAgent agent = newCell.GetComponent<NavMeshAgent>();
        newCell.name = "CellVariant1_Number_" + cellCount;
        agent.SetDestination(GetRandomNavMeshPos());
        cells.Add(new CellData(newCell, agent));
        cellCount++;
    }
    void SpawnNewCell2()
    {
        GameObject newCell = Instantiate(cellPrefab2, GetRandomNavMeshPos(), Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), spawnFolder);
        NavMeshAgent agent = newCell.GetComponent<NavMeshAgent>();
        newCell.name = "CellVariant2_Number_" + cellCount;
        agent.SetDestination(GetRandomNavMeshPos());
        cells.Add(new CellData(newCell, agent));
        cellCount++;
    }
    
    void SpawnNewMed1(CellData targetCell)
    {
        GameObject newCell = Instantiate(medPrefab, centerPoint.position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), MedLayer.transform);
        NavMeshAgent agent = newCell.GetComponent<NavMeshAgent>();
        newCell.name = "MedVariant1_Number_" + cellCount;
        agent.SetDestination(targetCell.agent.transform.position);
        meds.Add(new CellData(newCell, agent));
        medCount++;
    }
    
    
    /// <summary>
    /// Gets a random valid point on the navmesh within the outer and inner radius.
    /// Uses unit circle to get the point on a plane, then rotates to match the rotation of navmesh in world.
    /// </summary>
    /// <returns>Vector3 of random valid point</returns>
    Vector3 GetRandomNavMeshPos()
    {
        Vector3 center = centerPoint.position; 
        float maxRadius = (center - edgePoint.position).magnitude;
        
        for (int i = 0; i < 100; i++) // Try 100 times to find a valid point
        {
            Vector3 randomDir = Random.insideUnitCircle.normalized;
            Quaternion rotation = Quaternion.Euler(parentLocation.rotation.eulerAngles.x + 90, parentLocation.rotation.eulerAngles.y, parentLocation.rotation.eulerAngles.z);
            //Quaternion rotation = Quaternion.Euler(90, 0, 0);
            randomDir = rotation * randomDir;
            float randomRadius = Mathf.Sqrt(Random.Range(minRadius * minRadius, maxRadius * maxRadius));
            Vector3 randomPos = center + randomDir * randomRadius;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 0.2f, NavMesh.AllAreas))
            {
                return hit.position; // Found a valid point on the NavMesh
            }
        }

        return center;
    }
    
    
}

/// <summary>
/// Paris a cell object with its navmeshagent to avoid having to use getcomponent
/// </summary>
[System.Serializable]
public struct CellData
{
    public GameObject cell;
    public NavMeshAgent agent;
    
    public CellData(GameObject gameObject, NavMeshAgent navMeshAgent)
    {
        cell = gameObject;
        agent = navMeshAgent;
    }
}
