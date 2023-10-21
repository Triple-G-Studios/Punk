using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class BossProjectileController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}

