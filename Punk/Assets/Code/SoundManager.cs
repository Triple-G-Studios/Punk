using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        // Outlet
        AudioSource audioSource;
        public AudioClip slashSound;
        public AudioClip hurtSound;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySoundSlash()
        {
            audioSource.PlayOneShot(slashSound);
        }

        public void PlaySoundHurt()
        {
            audioSource.PlayOneShot(hurtSound);
        }
    }
}