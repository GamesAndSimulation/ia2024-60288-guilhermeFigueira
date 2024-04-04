using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcuts : MonoBehaviour
{
    private GameObject player;
    public GameObject[] shortcuts;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            player.transform.position = shortcuts[0].transform.position;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            player.transform.position = shortcuts[1].transform.position;
        if(Input.GetKeyDown(KeyCode.Alpha3))
            player.transform.position = shortcuts[2].transform.position;

    }
}
