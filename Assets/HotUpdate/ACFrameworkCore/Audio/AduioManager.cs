using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    音频模块

-----------------------*/


namespace ACFrameworkCore
{
    public class AduioManager : SingletonInit<AduioManager>, ICore
    {
        public void ICroeInit()
        {
            AudioClipDic = new Dictionary<string, AudioClip>();
            soundList = new List<AudioSource>();

            if (soundObj != null)
            {
                soundObj = new GameObject();
                soundObj.name = "Aduio";
                bkMusic = soundObj.AddComponent<AudioSource>();
            }
            ACDebug.Log("音频模块初始化成功!");
        }

        private Dictionary<string, AudioClip> AudioClipDic;//音效列表
        private GameObject soundObj = null;//音效依附对象
        private float bkValue = 1;//背景音乐大小
        public AudioSource bkMusic;//背景音乐组件
        private float soundValue = 1;//音效大小
        private List<AudioSource> soundList; //音效列表

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void OnAudioClipDicInit(float bkValue, float soundValue,
           AudioSource bkMusic, AudioSource soundMusic)
        {
            this.bkValue = bkValue;
            this.soundValue = soundValue;
            //PlayBkMusic()
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
                    GameObject.Destroy(soundList[i]);
                    soundList.RemoveAt(i);
                }
            }
        }

        //***********************背景音乐***********************
        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="name"></param>
        public void PlayBkMusic(string name, AudioClip clip)
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkValue;
            bkMusic.Play();
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public void PauseBKMusic()
        {
            bkMusic?.Pause();
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopBKMusic()
        {
            bkMusic?.Stop();
        }

        /// <summary>
        /// 改变背景音乐 音量大小
        /// </summary>
        /// <param name="v"></param>
        public void ChangeBKValue(float v)
        {
            bkValue = v;
            if (bkMusic == null)
                return;
            bkMusic.volume = bkValue;
        }


        //***********************音效音乐***********************
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isLoop"></param>
        /// <param name="clip"></param>
        public void PlaySound(string name, bool isLoop, AudioClip clip)
        {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = isLoop;
            source.volume = soundValue;
            source.Play();
            soundList.Add(source);
        }

        /// <summary>
        /// 改变音效声音大小
        /// </summary>
        /// <param name="value"></param>
        public void ChangeSoundValue(float value)
        {
            soundValue = value;
            for (int i = 0; i < soundList.Count; ++i)
                soundList[i].volume = value;
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        public void StopSound(AudioSource source)
        {
            if (soundList.Contains(source))
            {
                soundList.Remove(source);
                source.Stop();
                GameObject.Destroy(source);
            }
        }
    }
}
