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
        public GameObject keybindsMenu;
        public GameObject resetMenu;
        //public GameObject levelsMenu;
        public bool isPaused;

        void Awake()
        {
            instance = this;
            Hide();
        }

        //Make them invisible
        public void Hide()
        {
            pauseMenu.SetActive(false);
            keybindsMenu.SetActive(false);
            resetMenu.SetActive(false);
            //levelsMenu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
        }

        //Make them visible
        public void Show()
        {
            pauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
        public void Switch(GameObject menu)
        {
            pauseMenu.SetActive(false);
            keybindsMenu.SetActive(false);
            resetMenu.SetActive(false);
            //levelsMenu.SetActive(false);
            menu.SetActive(true);
        }
    }
}
