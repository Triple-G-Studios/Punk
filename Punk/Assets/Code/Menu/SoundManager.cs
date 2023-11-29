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
        public AudioClip whooshSound;
        public AudioClip gameOverSound;
        public AudioClip jumpSound;
        public AudioClip victorySound;
        public AudioClip teleportSound;
        public AudioClip healSound;
        public AudioClip collectSound;

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

        public void PlaySoundWhoosh()
        {
            audioSource.PlayOneShot(whooshSound);
        }

        public void PlaySoundGameOver()
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        public void PlaySoundJump()
        {
            audioSource.PlayOneShot(jumpSound);
        }

        public void PlaySoundVictory()
        {
            audioSource.PlayOneShot(victorySound);
        }

        public void PlaySoundTeleport()
        {
            audioSource.PlayOneShot(teleportSound);
        }

        public void PlaySoundHeal()
        {
            audioSource.PlayOneShot(healSound);
        }

        public void PlaySoundCollect()
        {
            audioSource.PlayOneShot(collectSound);
        }
    }
}
