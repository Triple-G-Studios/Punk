using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class SecretCameraSwitcher : MonoBehaviour
    {
        public GameObject fromCamera;
        public GameObject toCamera;

        void OnTriggerEnter2D(Collider2D collision)
        {
            fromCamera.SetActive(false);
            toCamera.SetActive(true);
        }
    }
}
