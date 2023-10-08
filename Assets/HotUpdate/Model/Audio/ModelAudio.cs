using Core;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = Core.Debug;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	音乐管理系统

-----------------------*/

namespace Farm2D
{
    public class ModelAudio : IModelInit
    {
        public static ModelAudio Instance;

        private GameObject soundObj;                 //音效依附对象
        private float soundValue = 1;                       //音效大小
        private Dictionary<string, AudioSource> soundDic;
        private List<AudioSource> soundList;                //音效列表//暂时没用的
        public List<SceneSoundItem> sceneSoundDataList;   //场景音乐数据
        public List<SoundDetails> soundDetailsDataList;       //音乐数据

        public async UniTask ModelInit()
        {
            Instance = this;
            sceneSoundDataList = new List<SceneSoundItem>();
            soundDetailsDataList = new List<SoundDetails>();
            soundList = new List<AudioSource>();
            soundDic = new Dictionary<string, AudioSource>();

            soundObj = new GameObject("AudioManagerSystem");
            GameObject.DontDestroyOnLoad(soundObj);

            //初始化数据
            //场景音乐配置表
            List<SceneSoundItemDetailsData> sceneSoundItemDetailsDataTemp = this.GetDataList<SceneSoundItemDetailsData>();
            foreach (SceneSoundItemDetailsData sceneSoundItemDetailsData in sceneSoundItemDetailsDataTemp)
            {
                SceneSoundItem sceneSoundItem = new SceneSoundItem();
                sceneSoundItem.sceneName = sceneSoundItemDetailsData.sceneName;
                sceneSoundItem.ambient = sceneSoundItemDetailsData.ambient;
                sceneSoundItem.music = sceneSoundItemDetailsData.music;
                sceneSoundDataList.Add(sceneSoundItem);
            }
            //音乐数据
            List<SoundDetailsData> soundDetailsDataTemp = this.GetDataList<SoundDetailsData>();
            foreach (SoundDetailsData soundDetailsData in soundDetailsDataTemp)
            {
                SoundDetails soundDetails = new SoundDetails();
                soundDetails.soundName = soundDetailsData.soundName;
                soundDetails.soundClip = LoadResExtension.Load<AudioClip>(soundDetailsData.soundName);
                soundDetails.soundPitchMin = soundDetailsData.soundPitchMin;
                soundDetails.soundPitchMax = soundDetailsData.soundPitchMax;
                soundDetails.soundVolume = soundDetailsData.soundVolume;
                soundDetailsDataList.Add(soundDetails);
            }

            ConfigEvent.BeforeSceneUnloadEvent.EventAdd(BeforeSceneUnloadEvent);
            ConfigEvent.AfterSceneLoadedEvent.EventAdd(OnAfterSceneLoadedEvent);
            ConfigEvent.PlaySoundEvent.EventAdd<ESoundName>(OnPlaySoundEvent);
            ConfigEvent.EndGameEvent.EventAdd(OnEndGameEvent);

            await UniTask.Yield();
        }

        //事件监听
        public  void OnAfterSceneLoadedEvent()
        {
            string currentScene = SceneManager.GetActiveScene().name;

            SceneSoundItem sceneSound = GetSceneSoundData(currentScene);
            if (sceneSound == null)
            {
                Debug.Error($"音乐{currentScene}没有找到");
                return;
            }

            SoundDetails ambient = GetSoundDetailsData(sceneSound.ambient);
            SoundDetails music = GetSoundDetailsData(sceneSound.music);

            PlaySound(ambient.soundClip, true);
            PlaySound(music.soundClip, true);
        }
        private void BeforeSceneUnloadEvent()
        {
            OnEndGameEvent();
        }
        private void OnEndGameEvent()
        {
            StopAllSound();
        }
        private void OnPlaySoundEvent(ESoundName soundName)
        {
            string soundNameTemp = string.Empty;
            switch (soundName)
            {
                case ESoundName.none:
                    break;
                case ESoundName.FootStepSoft:
                    soundNameTemp = ConfigSound.footstepSound;
                    break;
                case ESoundName.FootStepHard:
                    soundNameTemp = ConfigSound.footstepSound;
                    break;
                case ESoundName.Axe:
                    soundNameTemp = ConfigSound.AxeSound;
                    break;
                case ESoundName.Pickaxe:
                    soundNameTemp = ConfigSound.pickup_popSound;
                    break;
                case ESoundName.Hoe:
                    soundNameTemp = ConfigSound.HoeSound;
                    break;
                case ESoundName.Reap:
                    soundNameTemp = ConfigSound.pluckSound;
                    break;
                case ESoundName.Water:
                    soundNameTemp = ConfigSound.WateringCanSound;
                    break;
                case ESoundName.Basket:
                    soundNameTemp = ConfigSound.BasketSound;
                    break;
                case ESoundName.Pickup:
                    soundNameTemp = ConfigSound.pickup_popSound;
                    break;
                case ESoundName.Plant:
                    soundNameTemp = ConfigSound.plant_seedSound;
                    break;
                case ESoundName.TreeFalling:
                    soundNameTemp = ConfigSound.tree_fallingSound;
                    break;
                case ESoundName.Rustle:
                    soundNameTemp = ConfigSound.rustleSound;
                    break;
                case ESoundName.AmbientCountryside1:
                    soundNameTemp = ConfigSound.countryside1Sound;
                    break;
                case ESoundName.AmbientCountryside2:
                    soundNameTemp = ConfigSound.countryside2Sound;
                    break;
                case ESoundName.MusicCalm1:
                    soundNameTemp = ConfigSound.Calm1_APlaceICallHomeSound;
                    break;
                case ESoundName.MusicCalm2:
                    soundNameTemp = ConfigSound.Calm2_ChildhoodFriendsSound;
                    break;
                case ESoundName.MusicCalm3:
                    soundNameTemp = ConfigSound.Calm3_PeacefulDaysSound;
                    break;
                case ESoundName.MusicCalm4:
                    soundNameTemp = ConfigSound.Calm4_SandCastlesSound;
                    break;
                case ESoundName.MusicCalm5:
                    soundNameTemp = ConfigSound.Calm5_SummerMemoriesSound;
                    break;
                case ESoundName.MusicCalm6:
                    soundNameTemp = ConfigSound.Calm6_InnocenceSound;
                    break;
                case ESoundName.AmbientIndoor1:
                    soundNameTemp = ConfigSound.indoors1Sound;
                    break;
            }
            if (string.IsNullOrEmpty(soundNameTemp))
            {
                Debug.Error($"音效是空的请添加音效{soundNameTemp}");
                return;
            }
            SoundDetails soundDetails = GetSoundDetailsData(soundNameTemp);
            if (soundDetails != null)
                PlaySound(soundDetails.soundClip, false);
        }

        private SceneSoundItem GetSceneSoundData(string sceneName)
        {
            return sceneSoundDataList.Find(p => { return p.sceneName == sceneName; });
        }
        private SoundDetails GetSoundDetailsData(string soundName)
        {
            return soundDetailsDataList.Find(p => { return p.soundName == soundName; });
        }

        /// <summary>
        /// 检查音效是否播放结束
        /// </summary>
        public void ChackSoundOpenOver()
        {
            for (int i = soundList.Count - 1; i >= 0; --i)
            {
                if (!soundList[i].isPlaying)
                {
                    soundList.Remove(soundList[i]);
                    GameObject.Destroy(soundList[i]);
                }
            }
        }
        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isLoop"></param>
        /// <param name="clip"></param>
        public void PlaySound(AudioClip clip, bool isLoop = false)
        {
            AudioSource source = null;
            if (soundDic.ContainsKey(clip.name))
            {
                source = soundDic[clip.name];
            }
            else
            {
                source = soundObj.AddComponent<AudioSource>();
                soundDic.Add(clip.name, source);
            }
            source.enabled = true;
            source.clip = clip;
            source.loop = isLoop;
            source.volume = soundValue;
            //source.pitch = Random.Range(0.8f, 1f);
            source.Play();
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        public void StopAllSound()
        {
            foreach (KeyValuePair<string, AudioSource> item in soundDic)
                item.Value.enabled = false;
        }

       
    }
}
