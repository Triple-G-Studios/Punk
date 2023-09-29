using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class MenuController : MonoBehaviour
    {
        public static MenuController instance;

        //Outlets
        public GameObject pauseMenu;
        public bool isPaused;

        void Awake()
        {
            instance = this;
            Hide();
        }

        //Make them invisible
        public void Hide()
        {
            gameObject.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
        }

        //Make them visible
        public void Show()
        {
            gameObject.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
    }
}
