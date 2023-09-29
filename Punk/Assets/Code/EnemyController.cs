using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class EnemyController : MonoBehaviour
    {
        public static EnemyController instance;

        // Outlet
        Rigidbody2D _rb;
        SpriteRenderer sprite;
        Animator animator;
        GameObject player;

        public GameObject laserPrefab; 
        public float visionRange = 5f; 
        public float shootInterval = 2f; 
        private float lastShootTime;

        // State Tracking
        public bool directionLeft;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            directionLeft = true;

            player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            float defaultSpeed = 12f;
            Vector2 directionToPlayer = player.transform.position - transform.position;

            if (directionToPlayer.magnitude <= visionRange)
            {
                _rb.velocity = Vector2.zero;
                animator.SetFloat("Speed", 0);
                animator.SetBool("Is Attacking", true);


                if (Time.time - lastShootTime >= shootInterval)
                {
                    ShootLaser();
                    lastShootTime = Time.time;
                }
            }
            else
            {
                if (directionLeft)
                {
                    _rb.AddForce(Vector2.left * defaultSpeed * Time.deltaTime, ForceMode2D.Impulse);
                    Vector3 theScale = transform.localScale;
                    theScale.x = Mathf.Abs(theScale.x) * -1; // Ensure it's always negative when moving left
                    transform.localScale = theScale;
                    // print("WORKING");
                    //sprite.flipX = true;
                }
                else
                {
                    _rb.AddForce(Vector2.right * defaultSpeed * Time.deltaTime, ForceMode2D.Impulse);
                    Vector3 theScale = transform.localScale;
                    theScale.x = Mathf.Abs(theScale.x); // Ensure it's always positive when moving right
                    transform.localScale = theScale;
                    //sprite.flipX = false;
                }

                animator.SetFloat("Speed", _rb.velocity.magnitude);
                animator.SetBool("Is Attacking", false);

            }
        }

        void ShootLaser()
        {
            Vector2 laserSpawnPosition = new Vector2(transform.position.x, transform.position.y + 0.45f);
            GameObject laser = Instantiate(laserPrefab, laserSpawnPosition, Quaternion.identity);
            Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();

            float laserSpeed = 5f;
            if (directionLeft)
            {
                laserRb.velocity = Vector2.left * laserSpeed;
            }
            else
            {
                laserRb.velocity = Vector2.right * laserSpeed;
            }
        }

        /*public void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.GetComponent<PlayerController>())
            {
                directionLeft = !directionLeft;
            }
        }*/
    }
}
