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
                EnemyController.instance.directionLeft = !EnemyController.instance.directionLeft;
            }
        }
    }
}
