using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        private List<BluePrintDetails> bluePrintDataList;                       //建造的图纸数据
        private Transform itemParent;                                           //存放的父物体

        public void ICroeInit()
        {
            Instance = this;
            bluePrintDataList = new List<BluePrintDetails>();
            new BuildInWorld();
            ChangeData();
            ConfigEvent.BuildFurniture.AddEventListener<int, Vector3>(OnBuildFurnitureEvent);
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
        /// 获取建造蓝图的一条数据
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
                var itemStock = InventoryAllSystem.Instance.GetData(ConfigEvent.PalayerBag, resourceItem.itemID);
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

        /// <summary>
        /// 建造家具的事件
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="mousePos"></param>
        private void OnBuildFurnitureEvent(int ID, Vector3 mousePos)
        {
            if (itemParent == null)
                itemParent = itemParent = SceneTransitionSystem.Instance.itemParent;
            //获取建造蓝图数据
            BluePrintDetails bluePrint = GetDataOne(ID);
            var buildItem = GameObject.Instantiate(bluePrint.buildPrefab, mousePos, Quaternion.identity, itemParent);
            //if (buildItem.GetComponent<Box>())
            //{
            //    buildItem.GetComponent<Box>().index = InventoryAllSystem.Instance.BoxDataAmount;
            //    buildItem.GetComponent<Box>().InitBox(buildItem.GetComponent<Box>().index);
            //}
        }
    }
}
