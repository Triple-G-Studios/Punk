using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Punk
{
    public class UpgradeObject : MonoBehaviour
    {
        //this class does skill trees
        public GameObject[] moshButtons;
        public GameObject[] theoryButtons;
        public GameObject[] presenceButtons;


        // Start is called before the first frame update
        void Start()
        {
            int mosh = PlayerPrefs.GetInt("mosh");
            int theory = PlayerPrefs.GetInt("theory");
            int presence = PlayerPrefs.GetInt("presence");

            for (int i = 0; i < moshButtons.Length; i++) {
                if (i == mosh) moshButtons[i].SetActive(true);
                else moshButtons[i].SetActive(false);
            }
            for (int i = 0; i < theoryButtons.Length; i++)
            {
                if (i == theory) theoryButtons[i].SetActive(true);
                else theoryButtons[i].SetActive(false);
            }
            for (int i = 0; i < presenceButtons.Length; i++)
            {
                if (i == presence) presenceButtons[i].SetActive(true);
                else presenceButtons[i].SetActive(false);
            }
        }

        public void upDash()
        {
            PlayerPrefs.SetFloat("dashMult", PlayerPrefs.GetFloat("dashMult") + .25f);
            PlayerPrefs.SetInt("presence", PlayerPrefs.GetInt("presence") + 1);
            nextScene();
        }

        public void upSpeed()
        {
            PlayerPrefs.SetFloat("spMult", PlayerPrefs.GetFloat("spMult") + .25f);
            PlayerPrefs.SetInt("presence", PlayerPrefs.GetInt("presence") + 1);
            nextScene();
        }

        public void upJump()
        {
            PlayerPrefs.SetFloat("jMult", PlayerPrefs.GetFloat("jMult") + .25f);
            PlayerPrefs.SetInt("presence", PlayerPrefs.GetInt("presence") + 1);
            nextScene();
        }

        public void upDamage()
        {
            PlayerPrefs.SetFloat("dmgMult", PlayerPrefs.GetFloat("dmgMult") + .5f);
            PlayerPrefs.SetInt("mosh", PlayerPrefs.GetInt("mosh") + 1);
            nextScene();
        }

        public void upCrit()
        {
            PlayerPrefs.SetInt("critOn", PlayerPrefs.GetInt("critOn") - 1);
            PlayerPrefs.SetInt("mosh", PlayerPrefs.GetInt("mosh") + 1);
            nextScene();
        }

        public void upAmmoPer()
        {
            PlayerPrefs.SetInt("ammoPer", PlayerPrefs.GetInt("ammoPer") + 1);
            PlayerPrefs.SetInt("theory", PlayerPrefs.GetInt("theory") + 1);
            nextScene();
        }

        private void nextScene()
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("toLevel"));
        }

        public void nextScene(string type)
        {

            PlayerPrefs.SetInt(type, PlayerPrefs.GetInt(type) + 1);
            SceneManager.LoadScene(PlayerPrefs.GetString("toLevel"));
        }
    }
}
