using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCubesManager : MonoBehaviour
{
    public GameObject[]  teleportSpots;
    public int changeArrowTime = 2;
    private float changeArrowTimer;

    private void Start()
    {
        teleportSpots = GameObject.FindGameObjectsWithTag("TeleportSpot");
            changeArrowTimer = 0f;
    }

    private void Update()
    {
        changeArrowTimer -= Time.deltaTime;
        if (changeArrowTimer <= 0)
        {
            changeArrowTimer = changeArrowTime;
            foreach (GameObject teleportSpot in teleportSpots)
            {
                int arrowCount = teleportSpot.transform.childCount;
                int randomArrowIndex = Random.Range(0, arrowCount);
                for (int i = 0; i < arrowCount; i++)
                {
                    teleportSpot.transform.GetChild(i).gameObject.SetActive(i == randomArrowIndex);
                }
            }
        }
    }

}
