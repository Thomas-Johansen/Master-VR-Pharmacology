using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

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
    public GameObject particelSpawner;
    private VisualEffect _smokeEffect;
    public GameObject medPrefab;
    public GameObject medPrefab2;
    
    public List<MedData> meds = new List<MedData>();
    private float _timestamp = 0;
    private int _particleBool = 0;
    private int _smokeBool = 0;
    
    
    
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
    private float _individualMedTimer;
    private float _cellTimeRatio;
    
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

        _individualCellTimer = 0;
        _individualMedTimer = 0;
        _cellTimeRatio = 1;
        
        
        //Particle effect
        _smokeEffect = particelSpawner.GetComponent<VisualEffect>();
        _smokeEffect.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (sharedTimingData.Stage)
        {
            case 0: //Stage 0: Temp location? Pause/Talking time before animation starts.
                _individualCellTimer += (Time.deltaTime * sharedTimingData.Speed);
                if (_individualCellTimer >= 20)
                {
                    _individualCellTimer = 0;
                    sharedTimingData.Stage = 1;
                }
                break;
            case 2:
                // Stage 2: Some cells start to show up
                sharedTimingData.Time += (Time.deltaTime * sharedTimingData.Speed); // Updates passed time for both cells and crossection
                _individualCellTimer += (Time.deltaTime * sharedTimingData.Speed);

                if (cells.Count < 15 && _individualCellTimer >= 2)
                {
                    int rand = UnityEngine.Random.Range(0, 2);
                    if (rand == 1)
                    {
                        SpawnNewCell1();
                    }
                    else
                    {
                        SpawnNewCell2();
                    }
                    _individualCellTimer = 0;
                }
                else if (cells.Count == 15)
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
                    obstacle.SetActive(false); 
                    sharedTimingData.Stage = 4; 
                }

                // Cell Spawning
                if (cells.Count < 50 && _individualCellTimer >= 2)
                {
                    int rand = UnityEngine.Random.Range(0, 2);
                    if (rand == 1)
                    {
                        SpawnNewCell1();
                    }
                    else
                    {
                        SpawnNewCell2();
                    }
                    _individualCellTimer = 0;
                }
                break;

            case 4:
                // Stage 4: Time update only
                sharedTimingData.Time += (Time.deltaTime * sharedTimingData.Speed);
                if (sharedTimingData.Time >= 100f)
                {
                    MedLayer.SetActive(true);
                    //CellLayer.SetActive(false); //TODO: Mention this issue? Revisit? 
                    obstacle.SetActive(false);
                    _individualCellTimer = 0;
                    sharedTimingData.Stage = 5;
                    
                }
                break;

            case 5:
                // Stage 5: Medicine spawning
                

                sharedTimingData.Time += (Time.deltaTime * sharedTimingData.Speed);
                _individualCellTimer += (Time.deltaTime * sharedTimingData.Speed);

                if (_smokeBool == 0 && _individualCellTimer >= 3.25f)
                {
                    _smokeEffect.Play();
                    _smokeBool = 1;
                }

                if (_particleBool == 0 && _individualCellTimer >= 4f)
                {
                    foreach (CellData cell in cells)
                    {
                        SpawnNewMed1(cell);
                    }
                    _particleBool = 1;
                    _smokeEffect.Stop();
                }
                if (sharedTimingData.Time >= 105)
                {
                    sharedTimingData.Stage = 6;
                    _individualCellTimer = 0;
                    _smokeBool = 0;
                    _cellTimeRatio = (28f / cellCount); //Devide time it should take on cells to go away. Maybe move to more centralized variable?
                }
                break;
            case 7: //TODO: REMOVE BASED ON DISTANCE FROM CENTER
                sharedTimingData.Time += (Time.deltaTime * sharedTimingData.Speed);
                _individualCellTimer += (Time.deltaTime * sharedTimingData.Speed);
                _individualMedTimer += (Time.deltaTime * sharedTimingData.Speed);

                if (_individualCellTimer > _cellTimeRatio && cells.Count > 0)
                {
                    //Removes cell closest to center to avoid "floating" cells
                    CellData cell = cells[0];
                    float distance = 1000f; //Just a bigger number than max distance to a cell
                    foreach (CellData thisCell in cells)
                    {
                        if (thisCell.cell.activeSelf)
                        {
                            float thisDistance = Vector3.Distance(centerPoint.position, thisCell.cell.transform.position);
                            if (thisDistance < distance)
                            {
                                cell = thisCell;
                                distance = thisDistance;
                            }
                        }
                    }
                    cell.cell.SetActive(false);
                    foreach (MedData med in meds)
                    {
                        if (med.target == cell.cell)
                        {
                            med.particle.SetActive(false);
                        }
                    }
                    _individualCellTimer = 0;
                }
                
                if (_smokeBool == 0 && _individualMedTimer >= 3.25f)
                {
                    _smokeEffect.Play();
                    _smokeBool = 1;
                }

                if (_individualMedTimer >= 4f && _particleBool < 7)
                {
                    _individualMedTimer = 0;
                    _smokeBool = 0;
                    _smokeEffect.Stop();
                    _particleBool++;
                    foreach (CellData cell in cells)
                    {
                        if (!cell.cell.activeSelf)
                        {
                            continue;
                        }
                        SpawnNewMed1(cell);
                    }
                } else if (_particleBool == 7 && _individualMedTimer >= 4f)
                {
                    _smokeEffect.Stop();
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
        meds.Add(new MedData(newCell, agent, targetCell.cell));
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

/// <summary>
/// Paris a cell object with its navmeshagent to avoid having to use getcomponent
/// Med need to eventually keep track of which cell the med is bound to
/// </summary>
[System.Serializable]
public struct MedData
{
    public GameObject particle;
    public NavMeshAgent agent;
    public GameObject target;
    public int canActivate;
    
    public MedData(GameObject gameObject, NavMeshAgent navMeshAgent, GameObject cell)
    {
        particle = gameObject;
        agent = navMeshAgent;
        target = cell;
        canActivate = 1;
    }
    
    public void DeActivate(){canActivate = 0;}
}
