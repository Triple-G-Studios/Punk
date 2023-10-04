using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class EnemyBounds : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<EnemyController>())
            {
                other.gameObject.GetComponent<EnemyController>().directionLeft = !other.gameObject.GetComponent<EnemyController>().directionLeft;
            }
        }
    }
}
