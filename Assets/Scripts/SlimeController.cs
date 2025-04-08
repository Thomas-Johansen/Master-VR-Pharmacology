using UnityEngine;

public class SlimeCOntroller : MonoBehaviour
{
    public GameObject slime;
    public GameObject point1;
    public GameObject point2;
    public SharedTimingData sharedTimingData;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slime.transform.position = point2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        slime.transform.position = Vector3.Lerp(point2.transform.position, point1.transform.position, sharedTimingData.Contraction);
    }
}
