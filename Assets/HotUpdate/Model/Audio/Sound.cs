using Farm2D;
using UnityEngine;

namespace Farm2D
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
