using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public JetpackManager jetpackManager;
    
    void Start()
    {
    }

    void Update()
    {
        // Float up and down in a sin wave
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * 2) * 0.5f + 0.5f, transform.position.z);
        // Rotate around the z axis
        transform.Rotate(0, 0, 50 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            jetpackManager.fullFuel = 1.2f;
            Destroy(gameObject);
        }
    }
}
