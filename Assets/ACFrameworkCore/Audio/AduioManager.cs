using System.Collections;
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
    public class AduioComponent : ICoreComponent
    {
        Dictionary<string, AudioClip> AudioClipDic = new Dictionary<string, AudioClip>();
    }
}
