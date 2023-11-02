using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Punk
{
    public class KeybindController : MonoBehaviour
    {
        // Outlets
        public TextMeshProUGUI buttonLabel;

        private void Start()
        {
            buttonLabel.text = PlayerPrefs.GetString("CustomKey");
        }

        private void Update()
        {
            if (buttonLabel.text == "Awaiting Input")
            {
                foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(keycode))
                    {
                        buttonLabel.text = keycode.ToString();
                        PlayerPrefs.SetString("CustomKey", keycode.ToString());
                        PlayerPrefs.Save();
                    }
                }
            }
        }

        public void ChangeKey()
        {
            buttonLabel.text = "Awaiting Input";
        }
    }
}
