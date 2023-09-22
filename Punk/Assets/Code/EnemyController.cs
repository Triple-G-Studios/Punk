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

        //State Tracking
        bool directionRight;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            directionRight = true;
        }

        // Update is called once per frame
        void Update()
        {
            float defaultSpeed = 10f;

            if (directionRight)
            {
                _rb.AddForce(Vector2.right * defaultSpeed * Time.deltaTime, ForceMode2D.Impulse);
                sprite.flipX = false;
            }

            else
            {
                _rb.AddForce(Vector2.left * defaultSpeed * Time.deltaTime, ForceMode2D.Impulse);
                sprite.flipX = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.GetComponent<PlayerController>())
            {
                directionRight = !directionRight;
            }
        }
    }
}
