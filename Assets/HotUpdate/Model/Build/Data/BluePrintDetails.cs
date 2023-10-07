using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	建造的数据

-----------------------*/

namespace Farm2D
{
    [System.Serializable]
    public class BluePrintDetails
    {
        /// <summary> 建造物品的id </summary>
        public int ID;
        /// <summary> 建造物品需要的材料 </summary>
        public InventoryItem[] resourceItem;
        /// <summary> 建造的成功的预制体 </summary>
        public GameObject buildPrefab;
    }
}
