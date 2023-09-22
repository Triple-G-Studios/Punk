using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class EnemyController : MonoBehaviour
    {
        //Outlet
        Rigidbody2D _rb;
        SpriteRenderer sprite;
        Animator animator;

        //State Tracking
        bool directionLeft;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            directionLeft = true;
        }

        // Update is called once per frame
        void Update()
        {
            float defaultSpeed = 12f;

            if (directionLeft)
            {
                _rb.AddForce(Vector2.left * defaultSpeed * Time.deltaTime, ForceMode2D.Impulse);
                sprite.flipX = true;
            }

            else
            {
                _rb.AddForce(Vector2.right * defaultSpeed * Time.deltaTime, ForceMode2D.Impulse);
                sprite.flipX = false;
            }

            animator.SetFloat("Speed", _rb.velocity.magnitude);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.GetComponent<PlayerController>())
            {
                directionLeft = !directionLeft;
            }
        }
    }
}
