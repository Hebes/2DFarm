using System;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    单个节点

-----------------------*/

namespace ACFrameworkCore
{
    public class Node : IComparable<Node>
    {
        public Vector2Int gridPosition;     //网格坐标
        public int gCost = 0;               //距离Start格子的距离
        public int hCost = 0;               //距离Target格子的距离
        public bool isObstacle = false;     //当前格子是否是障碍
        public Node parentNode;             //父节点
        public int FCost => gCost + hCost;  //当前格子的值

        public Node(Vector2Int pos)
        {
            gridPosition = pos;
            parentNode = null;
        }

        /// <summary>
        /// 比较方法 大于返回1 等于返回0 小于返回-1
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Node other)
        {
            //比较选出最低的F值，返回-1，0，1
            int result = FCost.CompareTo(other.FCost);
            if (result == 0)
            {
                result = hCost.CompareTo(other.hCost);
            }
            return result;
        }
    }
}
