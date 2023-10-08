using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

/*--------�ű�����-----------
				
�������䣺
	1607388033@qq.com
����:
	����
����:
    ��Ƶģ��

-----------------------*/


namespace Core
{
    /// <summary> �������� </summary>
    public enum EAudioSourceType
    {
        /// <summary> �������� </summary>
        BGM,
        /// <summary> ��Ч </summary>
        SFX,
    }

    public class CoreAduio : ICore
    {
        public static CoreAduio Instance;
        private Dictionary<string, AudioClip> audioClipDic; //��Ч�б�
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
            Debug.Log("��Ƶģ���ʼ���ɹ�!");
        }

        /// <summary>
        /// ������Ч
        /// </summary>
        /// <param name="audioClipName">��Ч����</param>
        /// <param name="audioSourceType">��Ч������</param>
        /// <param name="isLoop">�Ƿ�ѭ��</param>
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
        /// ��ͣ����
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
        /// �ı�����
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
        /// ������Ч
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
