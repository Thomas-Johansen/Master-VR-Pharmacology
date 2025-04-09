using System;
using UnityEngine;

public class Button2 : MonoBehaviour
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
        if (sharedTimingData.Stage == 22)
        {
            _meshRenderer.enabled = true;
            if (isEntered)
            {
                sharedTimingData.Stage = 9;
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
