using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Punk
{
    public class BossEnemySoundManager : MonoBehaviour
    {
        public static BossEnemySoundManager instance;

        // Outlet
        AudioSource audioSource;
        public AudioClip normalShootSound;
        public AudioClip chestShootSound;
        public AudioClip laserShootSound;
        public AudioClip movementSoundClip;
        public AudioClip deadSoundClip;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            
        }

        public void PlaySoundNormalShoot()
        {
            audioSource.PlayOneShot(normalShootSound);
        }

        public void PlaySoundChestshoot()
        {
            audioSource.PlayOneShot(chestShootSound);
        }

        public void PlaySoundLaserShoot()
        {
            audioSource.PlayOneShot(laserShootSound);
        }

        public void PlayMovementSound()
        {
            if (!audioSource.isPlaying || audioSource.clip != movementSoundClip)
            {
                audioSource.clip = movementSoundClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        public void StopMovementSound()
        {
            if (audioSource.clip == movementSoundClip)
            {
                audioSource.Stop();
            }
        }

        public void PlaySoundDead()
        {
            audioSource.PlayOneShot(deadSoundClip);
        }

    }
}