using System;
using UnityEngine;
/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    场景物体的描述

-----------------------*/

namespace ACFrameworkCore
{
    [Serializable]
    public class SceneItem
    {
        public int itemID;                  //物品ID
        public int ItemAmount;              //物品数量
        public SerializableVector3 position;//物品位置
    }
}
