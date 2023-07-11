using System.Collections;
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


namespace ACFrameworkCore
{
    public class AduioComponent : ICoreComponent
    {
        public static AduioComponent Instance { get; private set; }
        Dictionary<string, AudioClip> AudioClipDic { get; set; }

        public void OnCroeComponentInit()
        {

            Instance = this;
            AudioClipDic = new Dictionary<string, AudioClip>();
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        public void OnAudioClipDicInit()
        {

        }
    }
}
