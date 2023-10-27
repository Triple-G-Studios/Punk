using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if (i <= mosh) moshButtons[i].SetActive(true);
                else moshButtons[i].SetActive(false);
            }
            for (int i = 0; i < theoryButtons.Length; i++)
            {
                if (i <= theory) theoryButtons[i].SetActive(true);
                else theoryButtons[i].SetActive(false);
            }
            for (int i = 0; i < presenceButtons.Length; i++)
            {
                if (i <= presence) presenceButtons[i].SetActive(true);
                else presenceButtons[i].SetActive(false);
            }
        }
    }
}
