using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class LaserController : MonoBehaviour
    {
        //State Tracking
        public float existFor = 5;

        void Update()
        {
            existFor -= Time.deltaTime;
            if (existFor < 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
            {
                Destroy(gameObject);
            }
        }
    }
}


