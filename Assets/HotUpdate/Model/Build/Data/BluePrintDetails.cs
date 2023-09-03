using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	建造的数据

-----------------------*/

namespace ACFrameworkCore
{
    [System.Serializable]
    public class BluePrintDetails
    {
        public int ID;
        public InventoryItem[] resourceItem;
        public GameObject buildPrefab;
    }
}
