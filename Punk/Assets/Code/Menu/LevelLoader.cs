using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace punk
{
    public class LevelLoader : MonoBehaviour
    {
        public Button[] levelButtons; 

        private string[] sceneNames = { "Tutorial", "Level1", "Level2", "Level3", "Boss" };

        void Start()
        {
            InitializeProgress();
            InitializeLevelButtons();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                ResetLevelsUnlocked();
            }
        }

        void InitializeLevelButtons()
        {
            int levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked", 1);

            for (int i = 0; i < levelButtons.Length; i++)
            {
                Button btn = levelButtons[i];
                TMP_Text btnText = btn.GetComponentInChildren<TMP_Text>();

                //had to create this because I was getting an error that the loop was already copleted before the next event was called
                int index = i;

                btn.onClick.AddListener(() => LoadLevel(sceneNames[index]));

                if (index + 1 > levelsUnlocked)
                {
                    btn.interactable = false;
                    btn.GetComponent<Image>().color = Color.gray;
                    btnText.text = "Locked";
                }
                else
                {
                    btn.interactable = true;
                    btn.GetComponent<Image>().color = Color.white;
                    btnText.text = "";
                }
            }
        }

        public void LoadLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        void InitializeProgress()
        {
            if (!PlayerPrefs.HasKey("levelsUnlocked"))
            {
                PlayerPrefs.SetInt("levelsUnlocked", 1);
                PlayerPrefs.Save();
            }
        }

        void ResetLevelsUnlocked()
        {
            PlayerPrefs.SetInt("levelsUnlocked", 1);
            PlayerPrefs.Save();

            InitializeLevelButtons();

            Debug.Log("Progress Reset");
        }
    }
}

