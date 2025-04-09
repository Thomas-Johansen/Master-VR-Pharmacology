using System;
using UnityEngine;

public class Button1 : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    public SharedTimingData sharedTimingData;
    private bool isEntered = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (sharedTimingData.Stage == 21)
        {
            _meshRenderer.enabled = true;

            if (isEntered)
            {
                sharedTimingData.Stage = 5;
            }
        } else if (sharedTimingData.Stage == 22)
        {
            _meshRenderer.enabled = true;
            if (isEntered)
            {
                sharedTimingData.Stage = 8;
            }
        }
        else
        {
            _meshRenderer.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isEntered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isEntered = false;
    }
}
