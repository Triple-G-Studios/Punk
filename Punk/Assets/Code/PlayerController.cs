using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class PlayerController : MonoBehaviour
    {
        // Outlet
        Rigidbody2D _rigidbody2D;
        SpriteRenderer sprite;
        Animator animator;

        // State Tracking
        public int jumpsLeft;


        // Methods (Start is called before the first frame update)
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            // This Update Event is sync'd with the Physics Engine
            animator.SetFloat("Speed", _rigidbody2D.velocity.magnitude);
            if (_rigidbody2D.velocity.magnitude > 0)
            {
                animator.speed = _rigidbody2D.velocity.magnitude / 3f;

            }
            else
            {
                animator.speed = 1f;
            }
        }

        // Update is called once per frame
        void Update()
        {

            float defaultSpeed = 18f;
            float speedMultiplier = 1.5f;

            float currentSpeed = defaultSpeed;

            // Run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed *= speedMultiplier;
            }

            // Move Player Left
            if (Input.GetKey(KeyCode.A))
            {
                _rigidbody2D.AddForce(Vector2.left * currentSpeed * Time.deltaTime, ForceMode2D.Impulse);
                sprite.flipX = true;
            }
            // Move Player Right
            if (Input.GetKey(KeyCode.D))
            {
                _rigidbody2D.AddForce(Vector2.right * currentSpeed * Time.deltaTime, ForceMode2D.Impulse);
                sprite.flipX = false;
            }

            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpsLeft > 0)
                {
                    jumpsLeft--;
                    _rigidbody2D.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
                }
            }
            animator.SetInteger("JumpsLeft", jumpsLeft);

            if (Input.GetKeyDown(KeyCode.K))
            {
                animator.SetTrigger("attack");
            }

            print(animator.speed);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            // Check that we collided with Ground
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                // Check what is directly below our character's feet
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.85f);
                // Debug.DrawRay(transform.position, Vector2.down * 0.85f); // Visualize Raycast

                //We might have multiple things below our character's feet
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];

                    // Check that we collided with ground below our feet
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        // Reset jump count
                        jumpsLeft = 2;
                    }
                }
            }
        }
    }
}

