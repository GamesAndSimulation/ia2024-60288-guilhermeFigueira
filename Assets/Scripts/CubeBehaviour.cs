using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    public int bullets = 30;

    // Start is called before the first frame update
    void Start()
    {

        //example of a script changing a value of another object.
        GameObject player = GameObject.Find("Player");
        MainScript ms = player.GetComponent<MainScript>();

        ms.bullets = bullets;

    }

    // Update is called once per frame
    void Update()
    {

        
    }

}
