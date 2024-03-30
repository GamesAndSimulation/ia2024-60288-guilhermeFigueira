using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyProjectile")
        {
            Debug.Log("PLAYER HIT");
            transform.parent.GetComponent<PlayerScript>().Damage(10);
            Destroy(other.gameObject);
        }
    }
}
