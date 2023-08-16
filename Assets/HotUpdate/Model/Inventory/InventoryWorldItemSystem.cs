﻿using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

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
        public Item bounceItemPrefab;//抛投的物品模板
        private Dictionary<string, List<SceneItem>> sceneItemDict;//世界场景的所有物体
        private Transform playerTransform;//玩家
        private Transform itemParent;//统一保存的父物体

        public Item itemPrefab;

        public void ICroeInit()
        {
            Instance = this;
            sceneItemDict = new Dictionary<string, List<SceneItem>>();
            //初始化监听信息
            ConfigEvent.UIItemCreatOnWorld.AddEventListener<SlotUI, Vector3>(OnInstantiateItemScen);
            ConfigEvent.UIItemDropItem.AddEventListener<int, Vector3>(OnDropItemEvent);
            ConfigEvent.SceneBeforeUnload.AddEventListener(OnBeforeSceneUnloadEvent);
            ConfigEvent.SceneAfterLoaded.AddEventListener(OnAfterSceneLoadedEvent);
            LoadInit().Forget();
        }

        public async UniTaskVoid LoadInit()
        {
            GameObject itemPrefabGo = await ResourceExtension.LoadAsyncUniTask<GameObject>(ConfigPrefab.ItemBasePrefab);
            itemPrefab = itemPrefabGo.GetComponent<Item>();
        }

        /// <summary>
        /// 扔东西
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="pos"></param>
        private void OnDropItemEvent(int itemID, Vector3 mousePos)
        {
            Item item = GameObject.Instantiate(bounceItemPrefab, playerTransform.position, Quaternion.identity, itemParent);
            item.itemID = itemID;
            //抛出方向
            var dir = (mousePos - playerTransform.position).normalized;
            item.GetComponent<ItemBounce>().InitBounceItem(mousePos, dir);

            //if (slotUI.itemDatails.canDropped == false)
            //{
            //    ACDebug.Log($"{slotUI.itemDatails.name}是不能被扔掉的");
            //    return;
            //}
            ////屏幕坐标转成世界坐标 鼠标对应的世界坐标
            //var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            //ConfigEvent.UIItemCreatOnWorld.EventTrigger(slotUI, pos);//id 数量,坐标//创建世界物体
            //InventoryAllSystem.Instance.RemoveItemDicArray(slotUI.configInventoryKey, slotUI.itemDatails.itemID, slotUI.itemAmount);//删除原先的
            //slotUI.UpdateEmptySlot();
        }

        /// <summary>
        /// 查找相机 限定范围的组件  场景加载之后
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
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnInstantiateItemScen(SlotUI slotUI, Vector3 pos)
        {
            Item item = GameObject.Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = slotUI.itemDatails.itemID;
            item.itemAmount = slotUI.itemAmount;

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
            //List<SceneItem> currentSceneItems = new List<SceneItem>();
            //foreach (var item in FindObjectsOfType<Item>())
            //{
            //    SceneItem sceneItem = new SceneItem
            //    {
            //        itemID = item.itemID,
            //        position = new SerializableVector3(item.transform.position)
            //    };
            //    currentSceneItems.Add(sceneItem);

            //    if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
            //    {
            //        //找剄数据就更新tem数据列表
            //        sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
            //    }
            //    else
            //    {   //如果是新场景
            //        sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItems);
            //    }
            //}
        }

        /// <summary>
        /// 刷新重建当前场景物品 切换场景结束的时候
        /// </summary>
        private void RecreateAllItems()
        {
            //List<SceneItem> currentSceneItems = new List<SceneItem>();
            //if (sceneItemDict.TryGetValue(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, out currentSceneItems))
            //{
            //    //清场
            //    foreach (var item in FindObjectsOfType<Item>())
            //        Destroy(item.gameObject);
            //    //重新创建   
            //    foreach (var item in currentSceneItems)
            //    {
            //        Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
            //        newItem.Init(item.itemID).Forget();
            //    }

            //    ////清场
            //    //for (int i = 0; i < FindObjectsOfType<Item>()?.Length; i++)
            //    //    Destroy(FindObjectsOfType<Item>()[i].gameObject);
            //    // //重新创建   
            //    //for (int i = 0; i < currentSceneItems?.Count; i++)
            //    //{
            //    //    Item newItem = Instantiate(itemPrefab, currentSceneItems[i].position.ToVector3(), Quaternion.identity, itemParent);
            //    //    newItem.Init(currentSceneItems[i].itemID);
            //    //}
            //}
        }
    }
}