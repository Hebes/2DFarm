using Cysharp.Threading.Tasks;
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


namespace Core
{
    /// <summary> 音乐类型 </summary>
    public enum EAudioSourceType
    {
        /// <summary> 背景音乐 </summary>
        BGM,
        /// <summary> 音效 </summary>
        SFX,
    }

    public class CoreAduio : ICore
    {
        public static CoreAduio Instance;
        private Dictionary<string, AudioClip> audioClipDic; //音效列表
        private AudioSource sfx;
        private AudioSource bgm;

        public void ICroeInit()
        {
            Instance = this;
            audioClipDic = new Dictionary<string, AudioClip>();
            GameObject AudioManagerGo = new GameObject("AudioManager");
            GameObject.DontDestroyOnLoad(AudioManagerGo);
            this.sfx = AudioManagerGo.AddComponent<AudioSource>();
            this.bgm = AudioManagerGo.AddComponent<AudioSource>();
            Debug.Log("音频模块初始化成功!");
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClipName">音效名称</param>
        /// <param name="audioSourceType">音效的类型</param>
        /// <param name="isLoop">是否循环</param>
        /// <returns></returns>
        public async UniTask PlayAudioSource(string audioClipName, EAudioSourceType audioSourceType, bool isLoop = false)
        {
            AudioClip audioClip = null;
            if (audioClipDic.TryGetValue(audioClipName, out audioClip))
            {
                Play(audioClip, audioSourceType, isLoop);
                return;
            }
            audioClip = await LoadResExtension.LoadAsync<AudioClip>(audioClipName);
            Play(audioClip, audioSourceType, isLoop);
            audioClipDic.Add(audioClipName, audioClip);
        }

        /// <summary>
        /// 暂停播放
        /// </summary>
        /// <param name="audioSourceType"></param>
        public void StopAudioSource(EAudioSourceType audioSourceType)
        {
            switch (audioSourceType)
            {
                case EAudioSourceType.BGM: bgm.Stop(); break;
                case EAudioSourceType.SFX: sfx.Stop(); break;
            }
        }

        /// <summary>
        /// 改变音量
        /// </summary>
        /// <param name="v"></param>
        public void ChangeAudioSourceValue(EAudioSourceType audioSourceType, float v)
        {
            switch (audioSourceType)
            {
                case EAudioSourceType.BGM: bgm.volume = v; break;
                case EAudioSourceType.SFX: sfx.volume = v; break;
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="audioSourceType"></param>
        /// <param name="isLoop"></param>
        private void Play(AudioClip audioClip, EAudioSourceType audioSourceType, bool isLoop = false)
        {
            AudioSource audioSource = null;
            switch (audioSourceType)
            {
                case EAudioSourceType.BGM: audioSource = bgm; break;
                case EAudioSourceType.SFX: audioSource = sfx; break;
            }

            audioSource.clip = audioClip;
            audioSource.loop = isLoop;
            audioSource.volume = Random.Range(.85f, 1.1f);
            audioSource.Play();
        }
    }
}
