using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallY;
    public float fallDuration;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Falling");
            transform.parent.DOMoveY(fallY, fallDuration).SetEase(Ease.InOutQuad);
        }
    }
}
