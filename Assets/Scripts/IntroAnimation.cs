using UnityEngine;

public class IntroAnimation : MonoBehaviour
{
    public SharedTimingData timingData;
    
    public GameObject LungStartPoint;
    public GameObject LungEndPoint;
    public GameObject Lung;
    public GameObject PlayerStartPoint;
    public GameObject PlayerEndPoint;
    public GameObject Player;
    private float timer;

    private bool soundStarted;
    public GameObject sound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timingData.Stage == 0)
        {
            timer += Time.deltaTime * timingData.Speed;

            if (timer <= 10)
            {
                if (timer >= 5)
                {
                    Lung.transform.position = Vector3.Lerp(LungStartPoint.transform.position, LungEndPoint.transform.position, (timer * 0.1f) - 0.5f); 
                }
                Player.transform.position = Vector3.Lerp(PlayerStartPoint.transform.position, PlayerEndPoint.transform.position, timer * 0.1f);
            } 
            else if (timer <= 37)
            {
                if (!soundStarted)
                {
                    soundStarted = true;
                    sound.GetComponent<AudioSource>().Play();
                }
                
            }
            else
            {
                timingData.Stage = 1;
            }
        }
    }
}
