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
        public int itemID;
        public SerializableVector3 position;
    }

    /// <summary>
    /// 序列化物品的坐标存储类
    /// </summary>
    [Serializable]
    public class SerializableVector3
    {
        public float x, y, z;

        /// <summary>
        /// 转成数值
        /// </summary>
        /// <param name="pos"></param>
        public SerializableVector3(Vector3 pos)
        {
            this.x = pos.x;
            this.y = pos.y;
            this.z = pos.z;
        }

        /// <summary>
        /// 转Vector3
        /// </summary>
        /// <returns></returns>
        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// 转Vector2
        /// </summary>
        /// <returns></returns>
        public Vector2Int ToVector2Int()
        {
            return new Vector2Int((int)x, (int)y);
        }
    }
}
