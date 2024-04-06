using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject[] enemiesToSpawn;
    public int killGoal;
    
    [Header("Door")]
    public Animator doorAnimator;
    public TextMeshPro doorSign;
    public Light doorLight;
    private PlayerScript _playerScript;
    

    private void Start()
    {
        _playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }

    private void Update()
    {
        if (_playerScript.enemyKillCount >= killGoal)
        {
            doorAnimator.SetTrigger("DoorOpen");
            doorSign.text = "Proceed";
            doorSign.color = Color.green;
            doorLight.color = Color.green;
            _playerScript.enemyKillCount = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject enemy in enemiesToSpawn)
            {
                enemy.SetActive(true);
            }
        }
    }

}
