using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Punk
{
    public class Goal : MonoBehaviour
    {
        Rigidbody2D _rigidbody2D;
        public string toLevel;

        void Start ()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                PlayerController.instance.sfxPlaying = true;
                SoundManager.instance.PlaySoundVictory();
                other.gameObject.GetComponent<PlayerController>().saveData();
                Invoke("NextLevel", 4);
            }
        }

        void NextLevel()
        {
            PlayerController.instance.sfxPlaying = false;
            SceneManager.LoadScene(toLevel);
        }
    }
}