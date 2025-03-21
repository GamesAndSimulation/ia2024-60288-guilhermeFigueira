using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType
    {
        FUEL,
        MEDKIT
    }
    
    public AudioClip pickUpSound;
    public CollectableType type;
    private JetpackManager jetpackManager;
    private float height;
    
    
    void Start()
    {
        height = transform.position.y;
        jetpackManager = GameObject.FindWithTag("Jetpack").GetComponent<JetpackManager>();
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
            //jetpackManager.fullFuel *= 1.3f;
            AudioManager.instance.PlaySound(pickUpSound, false, 1.0f);
            switch (type)
            {
                 case CollectableType.FUEL:
                     FuelTrigger();
                     break;
                 case CollectableType.MEDKIT:
                     MedkitTrigger();
                     break;
            } 
            Destroy(gameObject);
        }
    }

    private void MedkitTrigger()
    {
        PlayerScript playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        playerScript.ChangeHealth(-60);
    }

    private void FuelTrigger()
    {
        jetpackManager.fuel = jetpackManager.fullFuel;
        jetpackManager.fuelText.text = $"{jetpackManager.fuel.ToString("F1")} / {jetpackManager.fullFuel.ToString("F1")}";
        Dashing dashing = GameObject.FindWithTag("Player").GetComponent<Dashing>();
        dashing.dashesCount = 1;
    }
}
