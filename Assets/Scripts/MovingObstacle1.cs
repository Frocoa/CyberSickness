using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle1 : MonoBehaviour
{

    [SerializeField] private Vector3 maxDeviation;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        transform.position = originalPosition + maxDeviation * (float) Math.Cos(Time.time);
    }
}
