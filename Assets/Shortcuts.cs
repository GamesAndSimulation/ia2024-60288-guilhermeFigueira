using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcuts : MonoBehaviour
{
    private GameObject player;
    private GameObject[] shortcuts;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        int i = 0;
        shortcuts = new GameObject[transform.childCount];
        foreach (Transform child in transform)
        {
            shortcuts[i] = child.gameObject;
            i++;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            player.transform.position = shortcuts[0].transform.position;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            player.transform.position = shortcuts[1].transform.position;
        if(Input.GetKeyDown(KeyCode.Alpha3))
            player.transform.position = shortcuts[2].transform.position;
        if(Input.GetKeyDown(KeyCode.Alpha4))
            player.transform.position = shortcuts[3].transform.position;
        if(Input.GetKeyDown(KeyCode.Alpha5))
            player.transform.position = shortcuts[4].transform.position;

    }
}
