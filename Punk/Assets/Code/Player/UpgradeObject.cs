using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class UpgradeObject : MonoBehaviour
    {
        public string upgradeType;
        public float upgradeAmount;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                doUpgrade();
                Destroy(gameObject);
            }
        }

        void doUpgrade()
        {
            if (upgradeType.Equals("distance"))
            {
                PlayerController.instance.projectileDistanceTimer += upgradeAmount;
            }

            if (upgradeType.Equals("damage"))
            {
                PlayerController.instance.damageMultiplier += upgradeAmount;
            }
        }
    }
}
