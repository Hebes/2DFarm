using System;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    音乐数据

-----------------------*/

namespace ACFrameworkCore
{
    [System.Serializable]
    public class SoundDetails
    {
        public string soundName;
        public AudioClip soundClip;
        [Range(0.1f, 1.5f)]
        public float soundPitchMin;
        [Range(0.1f, 1.5f)]
        public float soundPitchMax;
        [Range(0.1f, 1f)]
        public float soundVolume;
    }
}
