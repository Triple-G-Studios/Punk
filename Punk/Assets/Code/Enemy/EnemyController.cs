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
        public GameObject healthDrop;

        // State Tracking
        public bool directionLeft;
        public float curHealth;
        public float maxHealth = 1;

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
            curHealth = maxHealth;
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            if (MenuController.instance.isPaused) return;

            float defaultSpeed = 12f;
            Vector2 directionToPlayer = player.transform.position - transform.position;

            // Debug.DrawRay(transform.position, (transform.TransformDirection(directionLeft ? Vector2.left : Vector2.right) * 4f), Color.green); // Visualize Raycast

                Vector2 forward = transform.TransformDirection(directionLeft ? Vector2.left : Vector2.right) * 4f;
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, forward, 4f);

                //We might have multiple things below our character's feet
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];

                    // Check that we collided with ground below our feet
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        _rb.velocity = Vector2.zero;
                        animator.SetFloat("Speed", 0f);
                        animator.SetBool("SpeedBool", false);

                        if (Time.time - lastShootTime >= shootInterval && directionToPlayer.magnitude <= visionRange)
                        {
                            animator.SetTrigger("Attack");
                            Invoke("ShootLaser", 0.5f);
                            lastShootTime = Time.time;
                            // animator.SetBool("IsAttacking", true);
                        }
                    }
                }
                                    
                if (directionLeft)
                {
                    _rb.AddForce(Vector2.left * defaultSpeed * Time.deltaTime, ForceMode2D.Impulse);
                    Vector3 theScale = transform.localScale;
                    theScale.x = Mathf.Abs(theScale.x) * -1; // Ensure it's always negative when moving left
                    transform.localScale = theScale;
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

                //animator.SetFloat("Speed", _rb.velocity.magnitude);
                animator.SetBool("SpeedBool", true);
                animator.SetBool("IsAttacking", false);

        }

        void ShootLaser()
        {
            Vector2 laserSpawnPosition = new Vector2(transform.position.x, transform.position.y + 0.45f);
            GameObject laser = Instantiate(laserPrefab, laserSpawnPosition, Quaternion.identity);
            Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();

            float laserSpeed = 5f;
            laserRb.velocity = directionLeft ? (Vector2.left * laserSpeed) : (Vector2.right * laserSpeed);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("SpeedBool", true);

        }

        public void TakeHit(float damage)
        {
            _rb.velocity = Vector2.zero;
            animator.SetTrigger("Hurt");
            curHealth -= damage;
            if(curHealth <= 0) {
                int randChance = Random.Range(0, 2);
                Vector2 healthSpawn = new Vector2(transform.position.x, transform.position.y);
                Destroy(gameObject);
                if (randChance == 1) Instantiate(healthDrop, healthSpawn, Quaternion.identity); //50% drop health
            }
        }
    }
}
