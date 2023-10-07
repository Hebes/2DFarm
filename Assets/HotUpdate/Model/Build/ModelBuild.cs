using Core;
using Cysharp.Threading.Tasks;
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

namespace Farm2D
{
    public class ModelBuild : IModelInit
    {
        public static ModelBuild Instance;
        private List<BluePrintDetails> bluePrintDataList;                       //建造的图纸数据
        private Transform itemParent;                                           //存放的父物体


        public async UniTask ModelInit()
        {
            Instance = this;
            bluePrintDataList = new List<BluePrintDetails>();
            ChangeData();

            //new BuildInWorld();
            ConfigEvent.BuildFurniture.AddEventListener<string, int, Vector3>(OnBuildFurnitureEvent);

            await UniTask.Yield();
        }

        /// <summary>
        /// 转化数据
        /// </summary>
        private void ChangeData()
        {
            List<BluePrintDetailsData> bluePrintDetailsDataList = this.GetDataList<BluePrintDetailsData>();
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
                bluePrintDetailsTemp.buildPrefab = LoadResExtension.Load<GameObject>(bluePrintDetailsData.buildPrefab);
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
                var itemStock = ModelItem.Instance.GetItem(ConfigEvent.ActionBar, resourceItem.itemID);//从物品获取资源
                if (itemStock.itemAmount >= resourceItem.itemAmount)//需要的资源
                    continue;
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 建造家具的事件
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="mousePos"></param>
        private void OnBuildFurnitureEvent(string BuildKey, int ID, Vector3 mousePos)
        {
            if (itemParent == null)
                itemParent = ModelSceneTransition.Instance.itemParent;
            //获取建造蓝图数据
            BluePrintDetails bluePrint = GetDataOne(ID);
            var buildItem = GameObject.Instantiate(bluePrint.buildPrefab, mousePos, Quaternion.identity, itemParent);
            if (buildItem.GetComponent<Box>())
            {
                buildItem.GetComponent<Box>().boxName = "Box" + ModelItem.Instance.ItemDic.Count.ToString();//设置箱子名称
                buildItem.GetComponent<Box>().InitBox();//初始化箱子
            }
        }

        
    }
}
