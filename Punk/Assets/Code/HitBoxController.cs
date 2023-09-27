using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class HitBoxController : MonoBehaviour
    {
        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = transform.parent.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check for collision with enemy
            if (other.CompareTag("Enemy"))
            {
                // Check if player is attacking (if you wish to add this condition)
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    // Destroy the enemy
                    Destroy(other.gameObject);
                }
            }
        }
    }
}

