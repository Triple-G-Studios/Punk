using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Punk
{
    public class LevelBounds : MonoBehaviour
    {

        public string toLevel;
        public GreenBossRobotController boss;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                if (toLevel == "Victory" && (boss == null || boss.curHealth > 0))
                {
                    return;
                }
                other.gameObject.GetComponent<PlayerController>().saveData();
                SceneManager.LoadScene(toLevel);
            }
        }
    }
}

