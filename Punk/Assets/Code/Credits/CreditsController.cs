using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Punk
{
    public class CreditsController : MonoBehaviour
    {
        public void switchScene()
        {
            SceneManager.LoadScene("Title Screen");
        }
    }
}
