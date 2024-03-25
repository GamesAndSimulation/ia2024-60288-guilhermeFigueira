using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public Transform target;
    public GameObject elevator;
    private bool canMoveElevator = false; 

    void Update(){
        //if(canMoveElevator)
        //{
        //    Vector3 move = new Vector3(0.0f, 0.1f, 0.0f);
        //    elevator.transform.position += move;
        //    //GameObject.FindWithTag("Player").transform.position += move;
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.parent = transform;
            canMoveElevator = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.parent = null;
            canMoveElevator = false;
        }
    }
    
}
