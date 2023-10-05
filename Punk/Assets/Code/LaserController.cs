using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class LaserController : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
            {
                Destroy(gameObject);
            }
        }
    }
}


