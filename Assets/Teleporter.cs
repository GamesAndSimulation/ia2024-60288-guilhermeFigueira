using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach(Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    Vector3 teleporterPosition = child.GetComponent<TeleporterArrrow>().pointToTeleporter.transform.position;
                    teleporterPosition.y += 23;
                    other.gameObject.transform.position = child.GetComponent<TeleporterArrrow>().pointToTeleporter.transform.position;
                    Rigidbody otherRB = other.GetComponent<Rigidbody>();
                    otherRB.AddForce(Vector3.up * 40000, ForceMode.Impulse);

                    break;
                }
            }
        }
    }
}
