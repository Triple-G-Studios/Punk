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

       public  void Show(GameObject menu)
        {
            mainmenu.SetActive(false);
            optionsmenu.SetActive(false);

            menu.SetActive(true);
        }

        public void playGame()
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Tutorial");
        }
    }
}
