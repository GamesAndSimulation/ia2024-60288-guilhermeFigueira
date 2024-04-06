using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public GameObject tempPlayer;
    public GameObject tempEnemy;
    public Light tempLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(CutsceneEvent());
    }

    private IEnumerator CutsceneEvent()
    {
        tempLight.DOIntensity(0, 2f);
        yield return new WaitForSeconds(2f);
        tempPlayer.SetActive(false);
        tempEnemy.SetActive(true);
        tempLight.DOIntensity(1, 2f);
        
    }
}
