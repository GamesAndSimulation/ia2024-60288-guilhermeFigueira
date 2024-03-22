using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFollow : MonoBehaviour
{
    public float rainSmoothSpeed = .2f;
    public Transform player;
    Vector3 currentPlayerPos;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y + 9f, player.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, rainSmoothSpeed);
    }
}
