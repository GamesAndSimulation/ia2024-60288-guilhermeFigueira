using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FinalArenaTrigger : MonoBehaviour
{
    public Transform door;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            door.DOMoveY(-2.011f, 1).SetEase(Ease.InOutQuad);
            this.GetComponent<FinalArenaTrigger>().enabled = false;
        }

    }
}
