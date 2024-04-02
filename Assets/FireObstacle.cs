using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObstacle : MonoBehaviour
{
    public GameObject CanonToRotate;
    
    void Start()
    {
        
    }

    void Update()
    {
        CanonToRotate.transform.Rotate(0, 50 * Time.fixedDeltaTime, 0);
        
    }
}
