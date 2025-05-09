using System;
using UnityEngine;

public class Button2 : MonoBehaviour
{
    public SharedTimingData sharedTimingData;
    public GameObject mesh;
    public GameObject leftHand;
    private GameObject camera;
    
    public GameObject rightHand;
    public GameObject voicePart1;
    public GameObject voicePart2;
    
    
    private AudioSource audioSource;
    private AudioSource voice1;
    private AudioSource voice2;
    
    private bool hasSpawned = false;
    private bool isEntered = false;
    private bool isPressed = false;
    private bool canFunction = false;
    
    
    private bool startPause = true;
    private bool endPause = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        voice1 = voicePart1.GetComponent<AudioSource>();
        voice2 = voicePart2.GetComponent<AudioSource>();
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (sharedTimingData.Stage == 21)
        {
            if (!hasSpawned)
            {
                mesh.SetActive(true);
                transform.position = camera.transform.position + (camera.transform.forward.normalized * 0.45f);  //rightHand.transform.position + new Vector3(0, 0.1f, 0);
                canFunction = true;
                hasSpawned = true;
            }
            
            if (isEntered && sharedTimingData.IsGrabingLeft)
            {
                transform.position = leftHand.transform.position + new Vector3(0, 0.1f, 0);
            }
            
            if (isEntered && sharedTimingData.IsGrabingRight)
            {
                transform.position = rightHand.transform.position + new Vector3(0, 0.1f, 0);
            }
            
            transform.right = transform.position - camera.transform.position;
        }

        if (sharedTimingData.IsPaused && startPause)
        {
            startPause = false;
            endPause = true;

            if (sharedTimingData.Stage < 5)
            {
                voice1.Pause();
            }
            else if (sharedTimingData.Stage < 10)
            {
                voice2.Pause();
            }
        }

        if (!sharedTimingData.IsPaused && endPause)
        {
            endPause = false;
            startPause = true;
            
            if (sharedTimingData.Stage < 5)
            {
                voice1.Play();
            }
            else if (sharedTimingData.Stage < 10)
            {
                voice2.Play();
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        isEntered = true;
        if (other.CompareTag("MainCamera") && !isPressed && canFunction)
        {
            isPressed = true;
            audioSource.Play();
            voice2.Play();
            sharedTimingData.Stage = 5;
            mesh.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isEntered = false;
    }
}
