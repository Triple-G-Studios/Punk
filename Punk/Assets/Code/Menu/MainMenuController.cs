using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Punk
{
    public class MainMenuController : MonoBehaviour
    {
        // Outlets
        public GameObject mainmenu;
        public GameObject optionsmenu;
        public GameObject keybindsmenu;
        public GameObject levelsMenu;

        public void Show(GameObject menu)
        {
            mainmenu.SetActive(false);
            optionsmenu.SetActive(false);
            keybindsmenu.SetActive(false);
            levelsMenu.SetActive(false);

            menu.SetActive(true);
        }

        public void playGame()
        {
            ClearPlayerPrefsExceptProgress();
            SceneManager.LoadScene("Tutorial");
        }

        public void ClearPlayerPrefsExceptProgress()
        {
            int levelProgress = PlayerPrefs.GetInt("levelsUnlocked", 1);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("levelsUnlocked", levelProgress);
            PlayerPrefs.Save();
        }



    }
}
