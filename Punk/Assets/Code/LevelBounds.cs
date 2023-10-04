using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Punk
{
    public class LevelBounds : MonoBehaviour
    {

        public string toLevel;
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                other.gameObject.GetComponent<PlayerController>().saveData();
                SceneManager.LoadScene(toLevel);
            }
        }
    }
}

