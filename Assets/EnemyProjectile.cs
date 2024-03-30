using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit Player");
            other.gameObject.GetComponent<PlayerScript>().health -= 10;
            Destroy(gameObject);
        }
    }
}
