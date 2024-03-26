using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Teleporting");
            foreach(Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    Vector3 teleporterPosition = child.GetComponent<TeleporterArrrow>().pointToTeleporter.transform.position;
                    teleporterPosition.y += 40;
                    other.gameObject.transform.position = teleporterPosition;
                    Rigidbody otherRB = other.GetComponent<Rigidbody>();
                    otherRB.AddForce(Vector3.up * 7500, ForceMode.Force);

                    break;
                }
            }
        }
    }
}
