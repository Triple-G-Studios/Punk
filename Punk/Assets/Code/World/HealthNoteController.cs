using Punk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace punk
{
    public class HealthNoteController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D()
        {
            PlayerController.instance.heal(1);
            Destroy(gameObject);
        }
    }
}
