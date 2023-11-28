using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class TeleporterController : MonoBehaviour
    {
        public static TeleporterController instance;

        public bool oneOn = true;
        public bool twoOn = true;
        public bool safety = false;

        void Awake()
        {
            instance = this;
        }

        public void tryTeleport(GameObject player, GameObject destination)
        {
            if (!safety)
            {
                if (oneOn && twoOn)
                {
                    oneOn = false;
                    twoOn = false;
                    safety = true;
                    player.transform.position = destination.transform.position + (Vector3.up * 2f);
                }
                /*else if (oneOn && twoOn && collision.gameObject.name == "Player" && source == Tele2)
                {
                    twoOn = false;
                    collision.gameObject.transform.position = Tele1.transform.position + (Vector3.up * 5f);
                    oneOn = false;
                    safety = true;
                    //Invoke("reset", 3);
                }*/
            }
            else
            {
                safety = false;
            }
        }

        public void tryReset()
        {
            if (safety)
            {
                oneOn = true;
                twoOn = true;
            }
        }
    }
}