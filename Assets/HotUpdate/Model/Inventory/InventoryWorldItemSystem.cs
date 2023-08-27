using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    世界地图上的物品管理

-----------------------*/

namespace ACFrameworkCore
{
    public class InventoryWorldItemSystem : ICore
    {
        public static InventoryWorldItemSystem Instance;
        private Dictionary<string, List<SceneItem>> sceneItemDict;  //世界场景的所有物体
        private Transform playerTransform;                          //玩家
        private Transform itemParent;                               //统一保存的父物体

        public Item bounceItemPrefab;                               //抛投的物品模板
        public Item itemPrefab;

        public void ICroeInit()
        {
            Instance = this;
            sceneItemDict = new Dictionary<string, List<SceneItem>>();
            //加载预制体
            bounceItemPrefab = YooAssetLoadExpsion.YooaddetLoadSync<GameObject>(ConfigPrefab.BonnceItemBasePreafab).GetComponent<Item>();
            //初始化监听信息
            ConfigEvent.InstantiateItemInScene.AddEventListener<int, int, Vector3>(OnInstantiateItemScen);
            ConfigEvent.UIItemDropItem.AddEventListener<int, Vector3, EItemType, int>(OnDropItemEvent);//扔东西
            ConfigEvent.SceneBeforeUnload.AddEventListener(OnBeforeSceneUnloadEvent);
            ConfigEvent.SceneAfterLoaded.AddEventListener(OnAfterSceneLoadedEvent);
            LoadInit().Forget();
        }

        public async UniTaskVoid LoadInit()
        {
            GameObject itemPrefabGo = await ResourceExtension.LoadAsyncUniTask<GameObject>(ConfigPrefab.ItemBasePreafab);
            itemPrefab = itemPrefabGo.GetComponent<Item>();
        }

        /// <summary>
        /// 扔东西
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="mousePos"></param>
        /// <param name="itemType"></param>
        /// <param name="removeAmount"></param>
        private void OnDropItemEvent(int itemID, Vector3 mousePos, EItemType itemType, int removeAmount)
        {
            if (itemType == EItemType.Seed)
            {
                UIDragPanel uIDragPanel1 = UIManagerExpansion.GetUIPanl<UIDragPanel>(ConfigUIPanel.UIDragPanelPrefab);
                InventoryAllSystem.Instance.RemoveItemDicArray(uIDragPanel1.key, itemID, removeAmount);
                return;
            }

            Item item = GameObject.Instantiate(bounceItemPrefab, playerTransform.position, Quaternion.identity, itemParent);
            //抛出方向
            var dir = (mousePos - playerTransform.position).normalized;
            item.GetComponent<ItemBounce>().InitBounceItem(mousePos, dir);
            //获取数据
            UIDragPanel uIDragPanel = UIManagerExpansion.GetUIPanl<UIDragPanel>(ConfigUIPanel.UIDragPanelPrefab);
            //设置数据
            item.itemID = itemID;
            item.itemAmount = removeAmount;
            if (removeAmount > item.itemAmount)
                item.itemAmount = uIDragPanel.itemAmount;
            //移除物品
            InventoryAllSystem.Instance.RemoveItemDicArray(uIDragPanel.key, itemID, item.itemAmount);
        }

        /// <summary>
        /// 查找玩家限定范围的组件场景加载之后
        /// </summary>
        private void OnAfterSceneLoadedEvent()
        {
            playerTransform = GameObject.FindGameObjectWithTag(ConfigTag.TagPlayer).transform;
            itemParent = GameObject.FindGameObjectWithTag(ConfigTag.TagItemParent).transform;
            RecreateAllItems();
        }

        /// <summary>
        /// 在世界地图生成物品
        /// </summary>
        /// <param name="itemID">物品ID</param>
        /// <param name="itemAmount">物品数量</param>
        /// <param name="pos"></param>
        private void OnInstantiateItemScen(int itemID, int itemAmount, Vector3 pos)
        {
            Item item = GameObject.Instantiate(bounceItemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = itemID;
            item.itemAmount = itemAmount;
            item.GetComponent<ItemBounce>().InitBounceItem(pos, Vector3.up);
        }

        /// <summary>
        /// 保存场景item
        /// </summary>
        private void OnBeforeSceneUnloadEvent()
        {
            GetAllSceneItems();
        }

        /// <summary>
        /// 获取当前场景里面的所有的物品
        /// </summary>
        private void GetAllSceneItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();
            foreach (var item in Object.FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.itemID,
                    ItemAmount = item.itemAmount,
                    position = new SerializableVector3(item.transform.position)
                };
                currentSceneItems.Add(sceneItem);

                if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
                    sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;//找剄数据就更新tem数据列表
                else
                    sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItems);//如果是新场景
            }
        }

        /// <summary>
        /// 刷新重建当前场景物品 切换场景结束的时候
        /// </summary>
        private void RecreateAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();
            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItems))
            {
                if (currentSceneItems == null) return;
                //清场
                foreach (var item in Object.FindObjectsOfType<Item>())
                    GameObject.Destroy(item.gameObject);
                //重新创建   
                foreach (SceneItem sceneItem in currentSceneItems)
                {
                    Item newItem = GameObject.Instantiate(itemPrefab, sceneItem.position.ToVector3(), Quaternion.identity, itemParent);
                    newItem.Init(sceneItem.itemID, sceneItem.ItemAmount).Forget();
                }
            }
        }

    }
}
