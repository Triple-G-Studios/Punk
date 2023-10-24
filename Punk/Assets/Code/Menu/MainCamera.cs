using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class MainCamera : MonoBehaviour
    {
        public static MainCamera instance;

        AudioSource audioSource;
        public AudioClip backgroundMusic;

        void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = backgroundMusic;
            PlayBackgroundMusic();
        }

        // Update is called once per frame
        void Update()
        {
            audioSource.clip = backgroundMusic;
            if (PlayerController.instance.sfxPlaying == true)
            {
                PauseBackgroundMusic();
            }
        }

        void PauseBackgroundMusic()
        {
            audioSource.Pause();
        }

        void PlayBackgroundMusic()
        {
            audioSource.Play();
        }
    }
}