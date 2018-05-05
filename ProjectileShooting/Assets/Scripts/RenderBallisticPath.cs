﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderBallisticPath : MonoBehaviour
{
    public float initialVelocity = 10.0f;
    public float timeResolution = 0.02f;
    public float maxTime = 10.0f;

    private LineRenderer lineRenderer;
    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocityVector = transform.forward * initialVelocity;
        lineRenderer.positionCount = (int)(maxTime / timeResolution);

        int index = 0;
        Vector3 currentPossition = transform.position;

        for (float t = 0.0f; t  < maxTime; t += timeResolution)
        {
            lineRenderer.SetPosition(index, currentPossition);
            currentPossition += velocityVector * timeResolution;
            velocityVector += Physics.gravity;
            index++;
        }
    }
}
