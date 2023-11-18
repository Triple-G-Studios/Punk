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
        public HealthBar healthBar;

        // State Tracking
        public bool directionLeft;
        public float curHealth;
        public float maxHealth = 10;

        private float rageStage1Threshold;
        private float rageStage2Threshold;
        private float rageStage3Threshold;

        private float timeUntilNormal;
        private float timeBetweenNormalAndChest;
        private float timeBetweenChestAndLaser;
        private float speed;
        private float lilBulletSpeed;
        private float bigBulletSpeed;

        public bool isAttacking = false;
        public GameObject normalProjectilePrefab;
        public GameObject chestProjectilePrefab;
        public Transform normalShootSpawn;
        public Transform chestShootSpawn;
        public string toLevel;

        public bool isInvincible; 


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

            rageStage1Threshold = maxHealth * 0.75f;
            rageStage2Threshold = maxHealth * 0.50f;
            rageStage3Threshold = maxHealth * 0.25f;

            timeUntilNormal = 2f;
            timeBetweenNormalAndChest = 5f;
            timeBetweenChestAndLaser = 10f;
            speed = 3000f;
            lilBulletSpeed = 10f;
            bigBulletSpeed = 7f;
            isInvincible = false;

            player = GameObject.FindGameObjectWithTag("Player");

            healthBar.SetMaxHealth(maxHealth);

            StartCoroutine(AttackPattern());

        }

        void Update()
        {
            if (MenuController.instance.isPaused || isAttacking) return;

            if (directionLeft)
            {
                _rb.AddForce(Vector2.left * speed * Time.deltaTime, ForceMode2D.Impulse);
                Vector3 theScale = transform.localScale;
                theScale.x = Mathf.Abs(theScale.x) * -1; // Ensure it's always negative when moving left
                transform.localScale = theScale;
            }
            else
            {
                _rb.AddForce(Vector2.right * speed * Time.deltaTime, ForceMode2D.Impulse);
                Vector3 theScale = transform.localScale;
                theScale.x = Mathf.Abs(theScale.x); // Ensure it's always positive when moving right
                transform.localScale = theScale;
            }

            animator.SetBool("isMoving", true);

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
            ShootProjectile(normalProjectilePrefab, normalShootSpawn, lilBulletSpeed, Vector3.zero);
            ShootProjectile(normalProjectilePrefab, normalShootSpawn, lilBulletSpeed, new Vector3(0.65f, 0, 0)); // 0.5 units to the right
            ShootProjectile(normalProjectilePrefab, normalShootSpawn, lilBulletSpeed, new Vector3(-0.65f, 0, 0)); // 0.5 units to the left
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
            ShootProjectile(chestProjectilePrefab, chestShootSpawn, bigBulletSpeed, Vector3.zero);
            ShootProjectile(chestProjectilePrefab, chestShootSpawn, bigBulletSpeed, new Vector3(-2f, 0, 0));

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
            if (curHealth <= 0) return;

            curHealth -= damage;
            healthBar.SetHealth(curHealth); 

            if (curHealth == rageStage1Threshold)
            {
                TriggerRage();
            }
            else if (curHealth == rageStage2Threshold)
            {
                TriggerRage();
            }
            else if (curHealth == rageStage3Threshold)
            {
                TriggerRage();
            }

            if (curHealth <= 0)
            {
                Die();
            }
            Debug.Log("currentHealth: " + curHealth);
        }

        void TriggerRage()
        {
            animator.speed = 1.5f;

            float randomAngleDegrees = Random.Range(15, 45);
            float angleDegrees = 35;
            float angleRadians = angleDegrees * Mathf.Deg2Rad; 

            Vector2 forceDirection = (Vector2.up * Mathf.Sin(angleRadians)) + ((directionLeft ? Vector2.left : Vector2.right) * Mathf.Cos(angleRadians));

            BossEnemySoundManager.instance.PlaySoundRage();

            player.GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized * 1500);

            TemporarilyIncreaseValues(10f);
        }

        void Die()
        {
            PlayerController.instance.sfxPlaying = true;
            BossEnemySoundManager.instance.PlaySoundDead();
            _rb.velocity = Vector2.zero;
            animator.SetTrigger("Dead");

            Invoke("SetToDeadLayer", 2);
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

        public void TemporarilyIncreaseValues(float duration)
        {
            StartCoroutine(TemporarilyIncreaseValuesCoroutine(duration));
        }

        private IEnumerator TemporarilyIncreaseValuesCoroutine(float duration)
        {
            float originalTimeUntilNormal = timeUntilNormal;
            float originalTimeBetweenNormalAndChest = timeBetweenNormalAndChest;
            float originalTimeBetweenChestAndLaser = timeBetweenChestAndLaser;
            float originalSpeed = speed;
            float originalLilBulletSpeed = lilBulletSpeed;
            float originalBigBulletSpeed = bigBulletSpeed;


            timeUntilNormal = 1f;
            timeBetweenNormalAndChest = 2.5f;
            timeBetweenChestAndLaser = 5f;
            speed = 10000f;
            lilBulletSpeed = 20f;
            bigBulletSpeed = 14f;
            isInvincible = true;
            sprite.color = Color.red;

            yield return new WaitForSeconds(duration);

            timeUntilNormal = originalTimeUntilNormal;
            timeBetweenNormalAndChest = originalTimeBetweenNormalAndChest;
            timeBetweenChestAndLaser = originalTimeBetweenChestAndLaser;
            speed = originalSpeed;
            lilBulletSpeed = originalLilBulletSpeed;
            bigBulletSpeed = originalBigBulletSpeed;
            isInvincible = false;
            sprite.color = Color.white;

            animator.speed = 1f;

        }

        void nextScene()
        {
            SceneManager.LoadScene("Victory");
        }
    }
}
