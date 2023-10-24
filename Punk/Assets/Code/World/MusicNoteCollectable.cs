using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class MusicNoteCollectable : MonoBehaviour
    {
        // Outlets
        Rigidbody2D _rigidbody2D;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D()
        {
            PlayerController.instance.getAmmo(1);
            Destroy(gameObject);
        }
    }
}