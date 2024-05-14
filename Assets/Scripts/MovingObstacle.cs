using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private bool moveHorizontal = true;
    [SerializeField] private bool moveVertical = true;

    private Vector3 originalPosition;

    private void Update()
    {
        float newX = moveHorizontal ? originalPosition.x + Mathf.Cos(Time.time) : originalPosition.x;
        float newY = moveVertical ? originalPosition.y + Mathf.Cos(Time.time) : originalPosition.y;

        transform.position =  new Vector3(newX, newY, originalPosition.z);
    }
}
