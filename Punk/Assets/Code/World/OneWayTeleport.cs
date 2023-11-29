using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class OneWayTeleport : MonoBehaviour
    {
        public GameObject player;
        public GameObject destination;

        void OnTriggerEnter2D()
        {
            player.transform.position = destination.transform.position + (Vector3.up * 0.2f);
        }
    }
}
