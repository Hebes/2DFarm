using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	音乐管理系统

-----------------------*/

namespace ACFrameworkCore
{
    public class AudioManagerSystem : ICore
    {
        public static AudioManagerSystem Instance;
        public List<SceneSoundItem> soundDetailsData; //音乐数据库
        public List<SoundDetails> sceneSoundData; //音乐数据库

        public void ICroeInit()
        {
            Instance = this;
            soundDetailsData = new List<SceneSoundItem>();
            sceneSoundData = new List<SoundDetails>();

            //初始化数据


            ConfigEvent.SceneAfterLoadedEvent.AddEventListener(OnAfterSceneLoadedEvent);
            ConfigEvent.PlaySoundEvent.AddEventListener(OnAfterSceneLoadedEvent);
            ConfigEvent.EndGameEvent.AddEventListener(OnEndGameEvent);
        }

        [Header("Audio Source")]
        public AudioSource ambientSource;
        public AudioSource gameSource;

        private Coroutine soundRoutine;

        [Header("Audio Mixer")]
        public AudioMixer audioMixer;

        [Header("Snapshots")]
        public AudioMixerSnapshot normalSnapShot;
        public AudioMixerSnapshot ambientSnapShot;
        public AudioMixerSnapshot muteSnapShot;
        private float musicTransitionSecond = 8f;


        public float MusicStartSecond => Random.Range(5f, 15f);


        private void OnEndGameEvent()
        {
            if (soundRoutine != null)
                StopCoroutine(soundRoutine);
            muteSnapShot.TransitionTo(1f);
        }

        private void OnPlaySoundEvent(SoundName soundName)
        {
            var soundDetails = soundDetailsData.GetSoundDetails(soundName);
            if (soundDetails != null)
                EventHandler.CallInitSoundEffect(soundDetails);
        }

        private void OnAfterSceneLoadedEvent()
        {
            string currentScene = SceneManager.GetActiveScene().name;

            SceneSoundItem sceneSound = sceneSoundData.GetSceneSoundItem(currentScene);
            if (sceneSound == null)
                return;

            SoundDetails ambient = soundDetailsData.GetSoundDetails(sceneSound.ambient);
            SoundDetails music = soundDetailsData.GetSoundDetails(sceneSound.music);

            if (soundRoutine != null)
                StopCoroutine(soundRoutine);
            soundRoutine = StartCoroutine(PlaySoundRoutine(music, ambient));
        }


        private IEnumerator PlaySoundRoutine(SoundDetails music, SoundDetails ambient)
        {
            if (music != null && ambient != null)
            {
                PlayAmbientClip(ambient, 1f);
                yield return new WaitForSeconds(MusicStartSecond);
                PlayMusicClip(music, musicTransitionSecond);
            }
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="soundDetails"></param>
        private void PlayMusicClip(SoundDetails soundDetails, float transitionTime)
        {
            audioMixer.SetFloat("MusicVolume", ConvertSoundVolume(soundDetails.soundVolume));
            gameSource.clip = soundDetails.soundClip;
            if (gameSource.isActiveAndEnabled)
                gameSource.Play();

            normalSnapShot.TransitionTo(transitionTime);
        }


        /// <summary>
        /// 播放环境音效
        /// </summary>
        /// <param name="soundDetails"></param>
        private void PlayAmbientClip(SoundDetails soundDetails, float transitionTime)
        {
            audioMixer.SetFloat("AmbientVolume", ConvertSoundVolume(soundDetails.soundVolume));
            ambientSource.clip = soundDetails.soundClip;
            if (ambientSource.isActiveAndEnabled)
                ambientSource.Play();

            ambientSnapShot.TransitionTo(transitionTime);
        }


        private float ConvertSoundVolume(float amount)
        {
            return (amount * 100 - 80);
        }

        public void SetMasterVolume(float value)
        {
            audioMixer.SetFloat("MasterVolume", (value * 100 - 80));
        }
    }
}
