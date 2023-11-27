using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform journey;
    public Transform destination;
    public GameObject[] waypoints;
    public int index = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.gameObject.name == "journey")
        {
            player.position = destination.position;
            delay(3000);
        } else if (collsion.gameObject.name == "Player" && collision.gameObject.name == "journey")
        {
            player.position = journey.position;
            delay(3000);
        }
    }
}
