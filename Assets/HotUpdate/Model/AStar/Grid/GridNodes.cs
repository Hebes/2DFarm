using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	网格节点

-----------------------*/

namespace ACFrameworkCore
{
    public class GridNodes
    {
        private int width;          //宽
        private int height;         //高
        private Node[,] gridNode;   //网格节点

        /// <summary>
        /// 构造函数初始化节点范围数组
        /// </summary>
        /// <param name="width">地图宽度</param>
        /// <param name="height">地图高度</param>
        public GridNodes(int width, int height)
        {
            this.width = width;
            this.height = height;

            gridNode = new Node[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gridNode[x, y] = new Node(new Vector2Int(x, y));
                }
            }
        }

        /// <summary>
        /// 获取网格节点
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <returns></returns>
        public Node GetGridNode(int xPos, int yPos)
        {
            if (xPos < width && yPos < height)
            {
                return gridNode[xPos, yPos];
            }
            Debug.LogError("超出网格范围");
            return null;
        }
    }
}
