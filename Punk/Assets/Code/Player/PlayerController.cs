using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Punk
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;
        private PlayerActionControls playerActionControls;

        // Outlets
        Rigidbody2D _rigidbody2D;
        SpriteRenderer sprite;
        Animator animator;
        public Image healthBar;
        public Sprite[] healthStates;
        public GameObject musicNotePrefab;
        public Transform aimPivot;
        public TMP_Text ammoText;

        // State Tracking
        public int jumpsLeft;
        public int ammoLeft;
        public bool canDash;
        public float dashTimer;
        public bool isDashing;
        public bool isInvincible;
        public bool isAttacking;
        private float attackCooldown = 0.3f;
        private float attackCooldownTimer = 0f;
        private float invincibleTimer;
        public int health;
        private Vector2 savedVelocity; //for dashing
        private bool facingRight;
        public bool sfxPlaying = false;
        public float KBForce = 4;
        public float KBCounter;
        public float KBTotalTime = 0.2f;
        public bool knockFromRight;
        public int parry = 0;

        //Upgradables
        public float projectileDistanceTimer;
        public float damageMultiplier;
        public float speedMultiplier;
        public float dashMultiplier;
        public float jumpMultiplier;
        public int critOn;
        public int moshScore;
        public int theoryScore;
        public int presenceScore;
        public int ammoPer;

        // Methods (Start is called before the first frame update)
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            health = 3;
            canDash = true;
            facingRight = true;
        }
        void Awake()
        {
            instance = this;
            playerActionControls = new PlayerActionControls();
            loadData();
            updateDisplay();
        }

        private void OnEnable()
        {
            playerActionControls.Enable();
        }

        private void OnDisable()
        {
            playerActionControls.Disable();
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
            if (MenuController.instance.isPaused) return;

            // TESTING PURPOSES
            float movementInput = playerActionControls.Game.Move.ReadValue<float>();

            float defaultSpeed = 18f;
            //Only lessen timers if they are positive to avoid underflow
            dashTimer -= (dashTimer > 0f) ? Time.deltaTime : 0f;
            invincibleTimer -= (invincibleTimer > 0f) ? Time.deltaTime : 0f;
            if (dashTimer <= 0) canDash = true;
            if (invincibleTimer > 0f) isInvincible = true;
            else isInvincible = false;

            float currentSpeed = defaultSpeed;

            //turn off dash on update
            if (isDashing && dashTimer <= 1.4f)
            {
                _rigidbody2D.velocity = savedVelocity;
                isDashing = false;
                animator.SetBool("Is Dashing", false);
            }

            //a lil extra time to move thru stuff
            if (dashTimer <= 1.3f)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Projectile"), false);
            }

            //Open Menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuController.instance.Show();
            }

            if(KBCounter <= 0)
            {
                // Move Player Left
                if (Input.GetKey(KeyCode.A))
                {
                    _rigidbody2D.AddForce(Vector2.left * currentSpeed * speedMultiplier * Time.deltaTime, ForceMode2D.Impulse);
                    Flip(false);

                }

                // Move Player Right
                if (Input.GetKey(KeyCode.D))
                {
                    _rigidbody2D.AddForce(Vector2.right * currentSpeed * speedMultiplier * Time.deltaTime, ForceMode2D.Impulse);
                    Flip(true);
                }

                // Move Player Down
                if (Input.GetKey(KeyCode.S) && _rigidbody2D.velocity.y != 0)
                {
                    _rigidbody2D.AddForce(Vector2.down * 0.1f, ForceMode2D.Impulse);
                }

            } else
            {
                if(knockFromRight == true)
                {
                    _rigidbody2D.velocity = new Vector2(-KBForce, KBForce);
                }
                if(knockFromRight == false)
                {
                    _rigidbody2D.velocity = new Vector2(KBForce, KBForce);
                }

                KBCounter -= Time.deltaTime;
            }

            // Jump
            animator.SetInteger("JumpsLeft", jumpsLeft);

            if (isAttacking)
            {
                attackCooldownTimer -= Time.deltaTime;
                if (attackCooldownTimer <= 0f)
                {
                    isAttacking = false;
                }
            }

            // Aim Toward Mouse
            Vector3 mousePosition = Input.mousePosition;
            Vector3 mousePositionInWorld = (Camera.main.enabled) ? Camera.main.ScreenToWorldPoint(mousePosition) : new Vector3();
            Vector3 directionFromPlayerToMouse = mousePositionInWorld - transform.position;

            float radiansToMouse = Mathf.Atan2(directionFromPlayerToMouse.y, directionFromPlayerToMouse.x);
            float angleToMouse = radiansToMouse * Mathf.Rad2Deg;

            aimPivot.rotation = Quaternion.Euler(0, 0, angleToMouse);
        }
        public void Jump(InputAction.CallbackContext ctxt)
        {
            if (ctxt.performed)
            {
                if (jumpsLeft > 0)
                {
                    jumpsLeft--;
                    SoundManager.instance.PlaySoundJump();
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x * jumpMultiplier, 0);
                    _rigidbody2D.AddForce(Vector2.up * 16f, ForceMode2D.Impulse);
                }
            }
        }

        public void Dash(InputAction.CallbackContext ctxt)
        {
            if (ctxt.performed)
            {
                if (canDash)
                {
                    // SoundManager.instance.PlaySoundWhoosh();
                    savedVelocity = _rigidbody2D.velocity;
                    float dashForce = facingRight ? 50f : -50f;
                    _rigidbody2D.velocity = new Vector2(dashForce * dashMultiplier, _rigidbody2D.velocity.y);
                    canDash = false;
                    isDashing = true;
                    dashTimer = 1.5f;
                    animator.SetBool("Is Dashing", true);
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Projectile"), true);
                }
            }
        }

        public void Shoot(InputAction.CallbackContext ctxt)
        {
            if (ctxt.performed)
            {
                if (ammoLeft > 0)
                {
                    GameObject newProjectile = Instantiate(musicNotePrefab);
                    newProjectile.transform.position = transform.position;
                    newProjectile.transform.rotation = aimPivot.rotation;
                    animator.SetTrigger("Shoot");
                    ammoLeft -= 1;
                    updateDisplay();
                }
            }
        }

        public void Attack(InputAction.CallbackContext ctxt)
        {
            if (ctxt.performed && !isAttacking)
            {
                animator.SetTrigger("Attack");
                isAttacking = true;
                attackCooldownTimer = attackCooldown;
            }
        }

        void Flip(bool right)
        {
            facingRight = right ? true : false;
            // Multiply the player's x local scale by -1
            Vector3 theScale = transform.localScale;
            theScale.x = facingRight ? Mathf.Abs(theScale.x) : Mathf.Abs(theScale.x) * -1;
            transform.localScale = theScale;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            // Check that we collided with Ground
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                // Check what is directly below our character's feet
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.85f);
                // Debug.DrawRay(transform.position, Vector2.down * 0.85f); // Visualize Raycast

                //We might have multiple things below our character's feet
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];

                    // Check that we collided with ground below our feet
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
                    {
                        // Reset jump count
                        jumpsLeft = 1;
                    }
                }
            }

            // Colliding with enemy
            if ((other.gameObject.GetComponent<EnemyController>() || other.gameObject.GetComponent<MeleeEnemyController>() 
                || other.gameObject.GetComponent<IdleEnemyController>() || other.gameObject.GetComponent<LaserController>()) && !isDashing && !isInvincible)
            {
                if (other.gameObject.GetComponent<LaserController>()) Destroy(other.gameObject);
                TakeDamage(1);
                KBCounter = KBTotalTime;
                if(other.transform.position.x <= transform.position.x)
                {
                    knockFromRight = false;
                }
                if (other.transform.position.x > transform.position.x)
                {
                    knockFromRight = true;
                }
                animator.SetTrigger("Hurt");
                SoundManager.instance.PlaySoundHurt();
            }

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("DeathBox") && !isDashing && !isInvincible)
            {
                animator.SetTrigger("Hurt");
                sfxPlaying = true;
                SoundManager.instance.PlaySoundGameOver();
                Invoke("Die", 1);
            }
            if (other.gameObject.CompareTag("BossBullet") && !isDashing && !isInvincible)
            {
                TakeDamage(1);
                KBCounter = KBTotalTime;
                if (other.transform.position.x <= transform.position.x)
                {
                    knockFromRight = false;
                }
                if (other.transform.position.x > transform.position.x)
                {
                    knockFromRight = true;
                }
                animator.SetTrigger("Hurt");
                SoundManager.instance.PlaySoundHurt();
            }
        }

        //Take Damage and set image
        void TakeDamage(int dmg)
        {
            health -= dmg;

            if (health <= 0)
            {
                healthBar.sprite = healthStates[0];
                sfxPlaying = true;
                SoundManager.instance.PlaySoundGameOver();
                Invoke("Die", 1);
                // Die();
            }
            else
            {
                healthBar.sprite = healthStates[health];
            }

            invincibleTimer = 1f;
        }

        //heal a bit
        public void heal(int amt)
        {
            SoundManager.instance.PlaySoundHeal();
            if (health + amt >= 3) health = 3;
            else health = health + amt;
            healthBar.sprite = healthStates[health];
        }

        // TODO: Change what Die() does, right now it just resets scene
        public void Die()
        {
            sfxPlaying = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void saveData()
        {
            PlayerPrefs.SetInt("health", health);
            PlayerPrefs.SetFloat("dmgMult", damageMultiplier);
            PlayerPrefs.SetFloat("dashMult", dashMultiplier);
            PlayerPrefs.SetFloat("spMult", speedMultiplier);
            PlayerPrefs.SetFloat("jMult", jumpMultiplier);
            PlayerPrefs.SetInt("critOn", critOn);
            PlayerPrefs.SetInt("mosh", moshScore);
            PlayerPrefs.SetInt("theory", theoryScore);
            PlayerPrefs.SetInt("presence", presenceScore);
            PlayerPrefs.SetInt("ammoPer", ammoPer);
            PlayerPrefs.SetInt("ammoLeft", ammoLeft);
            PlayerPrefs.SetFloat("projTime", projectileDistanceTimer);
            PlayerPrefs.SetInt("parry", parry);
        }

        public void getAmmo(int ammoAmt)
        {
            SoundManager.instance.PlaySoundCollect();
            ammoLeft += ammoAmt;
            updateDisplay();
        }

        //Load all fields from
        public void loadData()
        {
            if (PlayerPrefs.HasKey("health")) health = PlayerPrefs.GetInt("health");
            else health = 3;
            if (PlayerPrefs.HasKey("dmgMult")) damageMultiplier = PlayerPrefs.GetFloat("dmgMult");
            else damageMultiplier = 1f;
            if (PlayerPrefs.HasKey("dashMult")) dashMultiplier = PlayerPrefs.GetFloat("dashMult");
            else dashMultiplier = 1f;
            if (PlayerPrefs.HasKey("spMult")) speedMultiplier = PlayerPrefs.GetFloat("spMult");
            else speedMultiplier = 1f;
            if (PlayerPrefs.HasKey("jMult")) jumpMultiplier = PlayerPrefs.GetFloat("jMult");
            else jumpMultiplier = 1f;
            if (PlayerPrefs.HasKey("critOn")) critOn = PlayerPrefs.GetInt("critOn");
            else critOn = 20;
            if (PlayerPrefs.HasKey("mosh")) moshScore = PlayerPrefs.GetInt("mosh");
            else moshScore = 0;
            if (PlayerPrefs.HasKey("theory")) theoryScore = PlayerPrefs.GetInt("theory");
            else theoryScore = 0;
            if (PlayerPrefs.HasKey("presence")) presenceScore = PlayerPrefs.GetInt("presence");
            else presenceScore = 0;
            if (PlayerPrefs.HasKey("ammoLeft")) ammoLeft = PlayerPrefs.GetInt("ammoLeft");
            else ammoLeft = 0;
            if (PlayerPrefs.HasKey("ammoPer")) ammoPer = PlayerPrefs.GetInt("ammoPer");
            else ammoPer = 1;
            if (PlayerPrefs.HasKey("projTime")) projectileDistanceTimer = PlayerPrefs.GetFloat("projTime");
            else projectileDistanceTimer = 1;
            if (PlayerPrefs.HasKey("parry")) parry = PlayerPrefs.GetInt("parry");
            else parry = 0;
        }

        void updateDisplay()
        {
            ammoText.text = "Left: " + ammoLeft.ToString();
        }
    }
}
