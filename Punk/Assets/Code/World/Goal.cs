using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.SceneManagement;

namespace Punk
{
    public class Goal : MonoBehaviour
    {
        Rigidbody2D _rigidbody2D;
        public string toLevel;
        public GameObject[] waypoints;
        public int idx = 0;
        private bool soundPlayed = false;
        public float speed = 1.5f;

        private string[] sceneNames = { "Tutorial", "Level1", "Level2", "Level3", "Boss" };

        void Start ()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        void Update ()
        {
            if (Vector2.Distance(waypoints[idx].transform.position, transform.position) < 1f)
            {
                idx++;
                if (idx >= waypoints.Length)
                {
                    idx = 0;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, waypoints[idx].transform.position, Time.deltaTime * speed);
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
            MarkLevelComplete(toLevel);
            SceneManager.LoadScene("SkillTree");
        }

        public void MarkLevelComplete(string completedLevel)
        {
            int completedLevelIndex = System.Array.IndexOf(sceneNames, completedLevel);

            if (completedLevelIndex == -1)
            {
                Debug.LogError("Level name not found in sceneNames array.");
                return;
            }

            int levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked", 1);

            if (completedLevelIndex + 1 > levelsUnlocked)
            {
                PlayerPrefs.SetInt("levelsUnlocked", completedLevelIndex + 1);
                PlayerPrefs.Save();
            }
        }
    }
}