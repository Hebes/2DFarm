using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	建造系统管理类

-----------------------*/

namespace ACFrameworkCore
{
    public class BuildManagerSystem : ICore
    {
        public static BuildManagerSystem Instance;
        private List<BluePrintDetails> bluePrintDataList;//建造数据

        public void ICroeInit()
        {
            Instance = this;
            bluePrintDataList = new List<BluePrintDetails>();
            ChangeData();
        }

        /// <summary>
        /// 转化数据
        /// </summary>
        private void ChangeData()
        {
            List<BluePrintDetailsData> bluePrintDetailsDataList = this.GetDataListThis<BluePrintDetailsData>();
            foreach (BluePrintDetailsData bluePrintDetailsData in bluePrintDetailsDataList)
            {
                BluePrintDetails bluePrintDetailsTemp = new BluePrintDetails();
                bluePrintDetailsTemp.ID = bluePrintDetailsData.ItemID;
                bluePrintDetailsTemp.resourceItem = new InventoryItem[bluePrintDetailsData.InventoryItemID.Count];
                for (int i = 0; i < bluePrintDetailsData.InventoryItemID.Count; i++)
                {
                    InventoryItem inventoryItemTemp = new InventoryItem();
                    inventoryItemTemp.itemID = bluePrintDetailsData.InventoryItemID[i];
                    inventoryItemTemp.itemAmount = bluePrintDetailsData.InventoryItemCount[i];
                    bluePrintDetailsTemp.resourceItem[i] = inventoryItemTemp;
                }
                bluePrintDetailsTemp.buildPrefab = ResourceExtension.Load<GameObject>(bluePrintDetailsData.buildPrefab);
                //获取图片名称
                //string imageName= DataExpansion.GetDataOne(bluePrintDetailsData.
                //bluePrintDetailsTemp.icon = ResourceExtension.LoadOrSub<Sprite>(bluePrintDetailsData.);
                bluePrintDataList.Add(bluePrintDetailsTemp);
            }
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public BluePrintDetails GetDataOne(int id)
        {
            BluePrintDetails bluePrintDetails = bluePrintDataList.Find((BluePrintDetails data) => { return data.ID == id; });
            return bluePrintDetails;
        }

        /// <summary>
        /// 检查建造资源物品库存
        /// </summary>
        /// <param name="ID">图纸ID</param>
        /// <returns></returns>
        public bool CheckStock(int ID)
        {
            var bluePrintDetails = GetDataOne(ID);

            foreach (var resourceItem in bluePrintDetails.resourceItem)
            {
                var itemStock = InventoryAllSystem.Instance.GetData(ConfigInventory.PalayerBag, resourceItem.itemID);
                if (itemStock.itemAmount >= resourceItem.itemAmount)
                {
                    continue;
                }
                else
                {
                    ACDebug.Error($"库存数量资源数量不过无法建造");
                    return false;
                }
            }
            return true;
        }
    }
}
