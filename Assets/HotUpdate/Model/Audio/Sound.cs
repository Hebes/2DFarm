﻿using ACFrameworkCore;
using UnityEngine;

namespace ACFarm
{
    [RequireComponent(typeof(AudioSource))]
    public class Sound : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void SetSound(SoundDetails soundDetails)
        {
            audioSource.clip = soundDetails.soundClip;
            audioSource.volume = soundDetails.soundVolume;
            audioSource.pitch = Random.Range(soundDetails.soundPitchMin, soundDetails.soundPitchMax);
        }
    }
}
