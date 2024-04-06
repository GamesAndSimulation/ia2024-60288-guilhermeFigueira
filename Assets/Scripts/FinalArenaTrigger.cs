using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FinalArenaTrigger : MonoBehaviour
{
    public Transform door;
    public Transform finalDoor;
    public GameObject FinalLetterPlate;
    public AudioClip congratulationsClip;
    public int enemiesToKill;
    private bool isOpen;   

    private void Start()
    {
        isOpen = false;
    }
    private void Update()
    {
        // TODO For testing only
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            enemiesToKill = 0;
            KilledEnemy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isOpen)
        {
            door.DOMoveY(-2.011f, 1).SetEase(Ease.InOutQuad);
            isOpen = true;
        }
    }

    public void AddEnemyToKill()
    {
        enemiesToKill++;
    }

    public void KilledEnemy()
    {
        Debug.LogWarning("Enemy Killed!");
        enemiesToKill--;
        if (enemiesToKill <= 0)
        {
            AudioManager.Instance.StopAmbientSound(1);
            finalDoor.DOMoveY(-9.772f, 5).SetEase(Ease.InOutQuad);
            var textObject = FinalLetterPlate.GetComponentInChildren<TextMeshPro>();
            AudioManager.Instance.PlaySound(congratulationsClip, false, 1f);
            textObject.text = "Congratulations";
            textObject.color = Color.green;
            FinalLetterPlate.GetComponentInChildren<Light>().color = Color.green;
        }
    }
}
