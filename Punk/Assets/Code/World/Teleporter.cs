using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class Teleporter : MonoBehaviour
    {
        public GameObject dest;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Player")
            {
                TeleporterController.instance.tryTeleport(collision.gameObject, dest);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Player")
            {
                TeleporterController.instance.tryReset();
            }
        }
    }
}
