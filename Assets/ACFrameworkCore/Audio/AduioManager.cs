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
        Dictionary<string, AudioClip> AudioClipDic { get; set; }

        public void OnCroeComponentInit()
        {
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
