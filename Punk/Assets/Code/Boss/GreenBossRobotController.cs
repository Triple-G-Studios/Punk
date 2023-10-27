using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Punk
{
    public class GreenBossRobotController : MonoBehaviour
    {
        public static GreenBossRobotController instance;

        // Outlet
        Rigidbody2D _rb;
        SpriteRenderer sprite;
        Animator animator;
        GameObject player;

        // State Tracking
        public bool directionLeft;
        public float curHealth;
        public float maxHealth = 10;

        public bool isAttacking = false;
        public float timeUntilNormal; 
        public float timeBetweenNormalAndChest;
        public float timeBetweenChestAndLaser; 
        public GameObject normalProjectilePrefab;
        public GameObject chestProjectilePrefab;
        public Transform normalShootSpawn;
        public Transform chestShootSpawn;
        public string toLevel;

        //private float invincibilityDuration = 1.0f;
        //private float invincibilityTimer = 0; 
        //private bool isInvincible = false; 


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

            StartCoroutine(AttackPattern());

        }

        void Update()
        {
            if (MenuController.instance.isPaused || isAttacking) return;

            float defaultSpeed = 5000f;

            if (directionLeft)
            {
                _rb.AddForce(Vector2.left * defaultSpeed * Time.deltaTime, ForceMode2D.Impulse);
                Vector3 theScale = transform.localScale;
                theScale.x = Mathf.Abs(theScale.x) * -1; // Ensure it's always negative when moving left
                transform.localScale = theScale;
            }
            else
            {
                _rb.AddForce(Vector2.right * defaultSpeed * Time.deltaTime, ForceMode2D.Impulse);
                Vector3 theScale = transform.localScale;
                theScale.x = Mathf.Abs(theScale.x); // Ensure it's always positive when moving right
                transform.localScale = theScale;
            }

            animator.SetBool("isMoving", true);

            //if (isInvincible)
            //{
            //    invincibilityTimer -= Time.deltaTime;
            //    if (invincibilityTimer <= 0)
            //    {
            //        isInvincible = false;
            //    }
            //}

        }

        IEnumerator AttackPattern()
        {
            while (true)
            {
                yield return new WaitForSeconds(timeUntilNormal);
                NormalShoot();
                yield return new WaitForSeconds(timeBetweenNormalAndChest);
                ChestShoot();
                //yield return new WaitForSeconds(timeBetweenChestAndLaser);
                //SuperLaser();
            }
        }

        void NormalShoot()
        {
            animator.SetBool("isMoving", false);
            isAttacking = true;
            animator.SetBool("normalShoot", true);
        }

        public void PlayNormalShootSound()
        {
            BossEnemySoundManager.instance.PlaySoundNormalShoot();
            ShootProjectile(normalProjectilePrefab, normalShootSpawn, 10f, Vector3.zero);
            ShootProjectile(normalProjectilePrefab, normalShootSpawn, 10f, new Vector3(0.65f, 0, 0)); // 0.5 units to the right
            ShootProjectile(normalProjectilePrefab, normalShootSpawn, 10f, new Vector3(-0.65f, 0, 0)); // 0.5 units to the left
        }

        void ChestShoot()
        {
            animator.SetBool("isMoving", false);
            isAttacking = true;
            animator.SetBool("chestShoot", true);

        }

        public void PlayChestShootSound()
        {
            BossEnemySoundManager.instance.PlaySoundChestshoot();
            ShootProjectile(chestProjectilePrefab, chestShootSpawn, 7f, Vector3.zero);
            ShootProjectile(chestProjectilePrefab, chestShootSpawn, 7f, new Vector3(-2f, 0, 0));

        }

        void SuperLaser()
        {
            animator.SetBool("isMoving", false);

            isAttacking = true;

            animator.SetBool("laserShoot", true);

        }

        public void PlayLaserShootSound()
        {
            BossEnemySoundManager.instance.PlaySoundLaserShoot();
        }

        public void EndAttack()
        {
            animator.SetBool("normalShoot", false);
            animator.SetBool("chestShoot", false);
            animator.SetBool("laserShoot", false);
            isAttacking = false;

        }

        void ShootProjectile(GameObject projectilePrefab, Transform spawnPoint, float speed, Vector3 offset)
        {
            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position + offset, spawnPoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            var projectileSprite = projectile.GetComponent<SpriteRenderer>();

            if (directionLeft)
            {
                rb.velocity = new Vector2(-speed, 0);
            }
            else
            {
                projectileSprite.flipX = !projectileSprite.flipX;
                rb.velocity = new Vector2(speed, 0);
            }
        }


        public void TakeHit(float damage)
        {
            //if (isInvincible) return;
            PlayerController.instance.sfxPlaying = true;
            BossEnemySoundManager.instance.PlaySoundDead();
            _rb.velocity = Vector2.zero;
            animator.SetTrigger("Dead");

            Invoke("SetToDeadLayer", 2);

            //curHealth -= damage;
            //if (curHealth <= 0)
            //{
            //    Destroy(gameObject);
            //}

            //isInvincible = true;
            //invincibilityTimer = invincibilityDuration;
        }

        private void SetToDeadLayer()
        {
            int deadBossLayer = LayerMask.NameToLayer("DeadBoss");
            gameObject.layer = deadBossLayer;

            foreach (Transform child in transform)
            {
                child.gameObject.layer = deadBossLayer;
            }
            SoundManager.instance.PlaySoundVictory();
            Invoke("nextScene", 4);
        }

        void nextScene()
        {
            SceneManager.LoadScene("Victory");
        }
    }
}
