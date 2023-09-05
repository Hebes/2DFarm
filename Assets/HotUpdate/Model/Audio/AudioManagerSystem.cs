using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;
using ACFrameworkCore;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	音乐管理系统

-----------------------*/

namespace ACFarm
{
    public class AudioManagerSystem : ICore
    {
        public static AudioManagerSystem Instance;
        public List<SceneSoundItem> sceneSoundDataList;   //场景音乐数据
        public List<SoundDetails> soundDetailsDataList;       //音乐数据

        public AudioSource ambientSource;       //环境音乐
        public AudioSource gameSource;          //游戏音乐

        private Coroutine soundRoutine;

        [Header("Audio Mixer")]
        public AudioMixer audioMixer;

        [Header("Snapshots")]
        public AudioMixerSnapshot normalSnapShot;
        public AudioMixerSnapshot ambientSnapShot;
        public AudioMixerSnapshot muteSnapShot;
        private float musicTransitionSecond = 8f;

        public float MusicStartSecond
        {
            get
            {
                return Random.Range(5f, 15f);
            }
        }


        public void ICroeInit()
        {
            Instance = this;
            GameObject gameObject = new GameObject("AudioManagerSystem");
            ambientSource = gameObject.AddComponent<AudioSource>();
            gameSource = gameObject.AddComponent<AudioSource>();
            sceneSoundDataList = new List<SceneSoundItem>();
            soundDetailsDataList = new List<SoundDetails>();

            //初始化数据
            List<SceneSoundItemDetailsData> sceneSoundItemDetailsDataTemp = this.GetDataListThis<SceneSoundItemDetailsData>();
            foreach (SceneSoundItemDetailsData sceneSoundItemDetailsData in sceneSoundItemDetailsDataTemp)
            {
                SceneSoundItem sceneSoundItem = new SceneSoundItem();
                sceneSoundItem.sceneName = sceneSoundItemDetailsData.sceneName;
                sceneSoundItem.ambient = sceneSoundItemDetailsData.ambient;
                sceneSoundItem.music = sceneSoundItemDetailsData.music;
                sceneSoundDataList.Add(sceneSoundItem);
            }
            List<SoundDetailsData> soundDetailsDataTemp = this.GetDataListThis<SoundDetailsData>();
            foreach (SoundDetailsData soundDetailsData in soundDetailsDataTemp)
            {
                SoundDetails soundDetails = new SoundDetails();
                soundDetails.soundName = soundDetailsData.soundName;
                soundDetails.soundClip = ResourceExtension.Load<AudioClip>(soundDetailsData.soundName);
                soundDetails.soundPitchMin = soundDetailsData.soundPitchMin;
                soundDetails.soundPitchMax = soundDetailsData.soundPitchMax;
                soundDetails.soundVolume = soundDetailsData.soundVolume;
                soundDetailsDataList.Add(soundDetails);
            }


            ConfigEvent.SceneAfterLoadedEvent.AddEventListener(OnAfterSceneLoadedEvent);
            //ConfigEvent.PlaySoundEvent.AddEventListener(OnAfterSceneLoadedEvent);
            //ConfigEvent.InitSoundEffect.AddEventListener(InitSoundEffect);
            ConfigEvent.EndGameEvent.AddEventListener(OnEndGameEvent);

            OnAfterSceneLoadedEvent();
        }


        private SceneSoundItem GetSceneSoundData(string sceneName)
        {
            return sceneSoundDataList.Find(p => { return p.sceneName == sceneName; });
        }
        private SoundDetails GetSoundDetailsData(string soundName)
        {
            return soundDetailsDataList.Find(p => { return p.soundName == soundName; });
        }
        private float ConvertSoundVolume(float amount)
        {
            return (amount * 100 - 80);
        }
        public void SetMasterVolume(float value)
        {
            audioMixer.SetFloat("MasterVolume", (value * 100 - 80));
        }


        private void InitSoundEffect(SoundDetails soundDetails)
        {
            GameObject obj = new GameObject("music");
            obj.AddComponent<Sound>().SetSound(soundDetails);
            MonoManager.Instance.StartCoroutine(DisableSound(obj, soundDetails.soundClip.length));
        }

        private IEnumerator DisableSound(GameObject obj, float duration)
        {
            yield return new WaitForSeconds(duration);
            GameObject.Destroy(obj);
        }


        private void OnEndGameEvent()
        {
            if (soundRoutine != null)
                MonoManager.Instance.MonoStopCoroutine(soundRoutine);
            muteSnapShot.TransitionTo(timeToReach: 1f);
        }
        private void OnPlaySoundEvent(string soundName)
        {
            SoundDetails soundDetails = GetSoundDetailsData(soundName);
            if (soundDetails != null)
                ConfigEvent.InitSoundEffect.EventTrigger(soundDetails);
        }
        private void OnAfterSceneLoadedEvent()
        {
            string currentScene = SceneManager.GetActiveScene().name;

            SceneSoundItem sceneSound = GetSceneSoundData(currentScene);
            if (sceneSound == null)
                return;

            SoundDetails ambient = GetSoundDetailsData(sceneSound.ambient);
            SoundDetails music = GetSoundDetailsData(sceneSound.music);

            if (soundRoutine != null)
                MonoManager.Instance.MonoStopCoroutine(soundRoutine);
            soundRoutine = MonoManager.Instance.StartCoroutine(PlaySoundRoutine(music, ambient));
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
    }
}
