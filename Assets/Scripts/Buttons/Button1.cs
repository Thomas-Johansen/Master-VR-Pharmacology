using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button1 : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    public SharedTimingData sharedTimingData;
    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject player;
    private bool isEntered = false;
    private bool doAnimation = false;
    private float animationTime = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEntered && (sharedTimingData.IsGrabingLeft || sharedTimingData.IsGrabingRight))
        {
            doAnimation = true;
            _meshRenderer.enabled = false;
        }

        if (doAnimation)
        {
            player.transform.position = Vector3.Lerp(startPoint.transform.position, endPoint.transform.position, (animationTime*0.2f));
            player.transform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(0.1f, 0.1f, 0.1f), (animationTime*0.2f));
            animationTime += Time.deltaTime;
        }

        if (animationTime >= 4.8f)
        {
            SceneManager.LoadScene(1);
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
