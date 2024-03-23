using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public JetpackManager jetpackManager;
    public float height;
    
    void Start()
    {
    }

    void Update()
    {
        // Float up and down in a sin wave
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * 2) * 0.5f + height, transform.position.z);
        // Rotate around the z axis
        transform.Rotate(0, 0, 50 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            jetpackManager.fullFuel *= 1.3f;
            jetpackManager.fuel = jetpackManager.fullFuel;
            jetpackManager.fuelText.text = $"{jetpackManager.fuel.ToString("F1")} / {jetpackManager.fullFuel.ToString("F1")}";
            Destroy(gameObject);
        }
    }
}
