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
                // Check if player is attacking
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    SoundManager.instance.PlaySoundSlash();
                    // Destroy the enemy
                    var enemy = other.GetComponentInParent<EnemyController>();
                    var idleEnemy = other.GetComponentInParent<IdleEnemyController>();
                    var meleeEnemy = other.GetComponentInParent<MeleeEnemyController>();
                    var damage = 3 * PlayerController.instance.damageMultiplier;
                    if (enemy)
                    {
                        enemy.TakeHit(damage);
                    }
                    else if (idleEnemy)
                    {
                        idleEnemy.TakeHit(damage);
                    }
                    else if (meleeEnemy)
                    {
                        meleeEnemy.TakeHit(damage);
                    }
                }
            }

            else if (other.CompareTag("BossHitBox"))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    SoundManager.instance.PlaySoundSlash();
                    GreenBossRobotController boss = other.GetComponentInParent<GreenBossRobotController>();
                    if (boss != null && !boss.isInvincible)
                    {
                        boss.TakeHit(1);
                    }
                }
            }
        }

    }
}

