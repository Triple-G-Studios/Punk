using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public bool canDash;
        public float dashTimer;
        public bool isDashing;
        private Vector2 savedVelocity; //for dashing

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
            //float speedMultiplier = 1.5f;
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0) canDash = true;

            float currentSpeed = defaultSpeed;

            //turn off dash on update
            if (isDashing && dashTimer <= 1.4f)
            {
                _rigidbody2D.velocity = savedVelocity;
                isDashing = false;
                animator.SetBool("Is Dashing", false);
            }

            // Run
            /*if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed *= speedMultiplier;
            }*/

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

            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (canDash)
                {
                    savedVelocity = _rigidbody2D.velocity;
                    _rigidbody2D.velocity = new Vector2((_rigidbody2D.velocity.x + (sprite.flipX?-2f:2f)) * 5f, _rigidbody2D.velocity.y);
                    canDash = false;
                    isDashing = true;
                    dashTimer = 1.5f;
                    animator.SetBool("Is Dashing", true);
                }
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

            // Attack
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("Attack");
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

            //Colliding with enemy
            if (other.gameObject.GetComponent<EnemyController>() && !isDashing)
            {
                Die();
            }
        }

        //TODO: Change what Die() does, right now it just resets scene
        void Die()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

