using UnityEngine;

public class BreathController : MonoBehaviour
{
    public SharedTimingData sharedTimingData;
    public AudioSource normal;
    public AudioSource asthma;
    
    private float timer;
    private bool breathing = true;
    private bool trigger1 = true;
    private bool trigger2 = true;
    public float maxVolume = 0.4f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        normal.volume = maxVolume;
        asthma.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        normal.volume = maxVolume - sharedTimingData.Contraction * maxVolume;
        asthma.volume = sharedTimingData.Contraction * maxVolume;
        
        timer += Time.deltaTime;
        
        if (timer >= 3.25 && breathing)
        {
            breathing = false;
            normal.Play();
            asthma.Play();
        } else if (timer >= 4 && !breathing)
        {
            breathing = true;
            timer = 0;
        }
        
        if (trigger1 && sharedTimingData.Stage == 5)
        {
            trigger1 = false;
            timer = 0;
        }
        
        else if (trigger2 && sharedTimingData.Stage == 7)
        {
            trigger2 = false;
            timer = 0;
        }
    }
}
