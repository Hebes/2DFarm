/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    音频拓展

-----------------------*/

using Cysharp.Threading.Tasks;

namespace Core
{
    public static class AudioExpansion
    {
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClipName"></param>
        /// <param name="audioSourceType"></param>
        /// <param name="isLoop"></param>
        /// <returns></returns>
        public static async UniTask PlayAudioSource(this EAudioSourceType audioSourceType, string audioClipName, bool isLoop = false)
        {
            await CoreAduio.Instance.PlayAudioSource(audioClipName, audioSourceType, isLoop);
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="audioSourceType"></param>
        public static void StopAudioSource(this EAudioSourceType audioSourceType)
        {
            CoreAduio.Instance.StopAudioSource(audioSourceType);
        }

        /// <summary>
        /// 改变音量
        /// </summary>
        /// <param name="audioSourceType"></param>
        /// <param name="v"></param>
        public static void ChangeAudioSourceValue(this EAudioSourceType audioSourceType, float v)
        {
            CoreAduio.Instance.ChangeAudioSourceValue(audioSourceType, v);
        }
    }
}
