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
        private bool soundPlayed = false;

        void Start ()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<PlayerController>() && !soundPlayed)
            {
                soundPlayed = true;
                PlayerController.instance.sfxPlaying = true;
                SoundManager.instance.PlaySoundVictory();
                other.gameObject.GetComponent<PlayerController>().saveData();
                Invoke("NextLevel", 4);
            }
        }

        void NextLevel()
        {
            soundPlayed = false;
            PlayerController.instance.sfxPlaying = false;
            PlayerPrefs.SetString("toLevel", toLevel);
            SceneManager.LoadScene("SkillTree");
        }
    }
}