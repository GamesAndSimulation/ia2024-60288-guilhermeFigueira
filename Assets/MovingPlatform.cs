using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float speed = 1.0f;

    private float startTime;
    private float moveLength;
    private bool isMovingToEnd = true;

    void Start()
    {
        startPosition = transform.localPosition;
        moveLength = Vector3.Distance(startPosition, endPosition);
        startTime = Time.time;
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float moveFraction = distCovered / moveLength;

        if (isMovingToEnd)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, moveFraction);

            if (transform.localPosition== endPosition)
            {
                isMovingToEnd = false;
                startTime = Time.time; 
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(endPosition, startPosition, moveFraction);

            if (transform.localPosition == startPosition) 
            {
                isMovingToEnd = true;
                startTime = Time.time;
            }
        }
    }
}
