using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObstacle : MonoBehaviour
{
    public GameObject CanonToRotate;
    public float RotationSpeed = 50f;
    
    void Start()
    {
        
    }

    void Update()
    {
        CanonToRotate.transform.Rotate(0, RotationSpeed * Time.fixedDeltaTime, 0);
        
    }
}
