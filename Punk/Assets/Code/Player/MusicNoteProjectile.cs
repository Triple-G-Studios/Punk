using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class MusicNoteProjectile : MonoBehaviour
    {
        // Outlets
        Rigidbody2D _rigidbody2D;

        //State tracking
        public float existFor = 1;

        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.velocity = transform.right * 10f;
            existFor = PlayerController.instance.projectileDistanceTimer;
        }

        void Update()
        {
            existFor -= Time.deltaTime;
            if ( existFor < 0)
            {
                Destroy(gameObject);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var enemy = collision.collider.GetComponent<EnemyController>();
            var idleEnemy = collision.collider.GetComponent<IdleEnemyController>();
            var meleeEnemy = collision.collider.GetComponent<MeleeEnemyController>();
            if (enemy)
            {
                enemy.TakeHit(1);
            } else if (idleEnemy)
            {
                idleEnemy.TakeHit(1);
            } else if (meleeEnemy)
            {
                meleeEnemy.TakeHit(1);
            }

            Destroy(gameObject);
        }
    }
}


