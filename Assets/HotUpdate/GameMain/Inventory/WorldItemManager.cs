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
    public class WorldItemManager : MonoBehaviour
    {
        public Item itemPrefab;
        public Item bounceItemPrefab;//抛投的物品模板
        private Transform itemParent;
        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();//

        private Transform playerTransform
        {
            get
            {
                return FindObjectOfType<Player>().transform;
            }
        }

        private void OnEnable()
        {
            ConfigEvent.CreatItemScene.AddEventListener<int, Vector3>(OnInstantiateItemScen);
            ConfigEvent.DropItem.AddEventListener<int, Vector3>(OnDropItemEvent);
            ConfigEvent.BeforeSceneUnload.AddEventListener(OnBeforeSceneUnloadEvent);
            ConfigEvent.BeforeSceneUnload.AddEventListener(OnAfterSceneLoadedEvent);
        }
        private void OnDisable()
        {
            ConfigEvent.CreatItemScene.RemoveEventListener<int, Vector3>(OnInstantiateItemScen);
            ConfigEvent.DropItem.RemoveEventListener<int, Vector3>(OnDropItemEvent);
            ConfigEvent.BeforeSceneUnload.RemoveEventListener(OnBeforeSceneUnloadEvent);
            ConfigEvent.BeforeSceneUnload.RemoveEventListener(OnAfterSceneLoadedEvent);
        }

        /// <summary>
        /// 扔东西
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="pos"></param>
        private void OnDropItemEvent(int ID, Vector3 mousePos)
        {
            Item item = Instantiate(bounceItemPrefab, playerTransform.position, Quaternion.identity, itemParent);
            item.itemID = ID;
            //抛出方向
            var dir = (mousePos - playerTransform.position).normalized;
            item.GetComponent<ItemBounce>().InitBounceItem(mousePos, dir);

        }

        /// <summary>
        /// 查找相机 限定范围的组件  场景加载之后
        /// </summary>
        private void OnAfterSceneLoadedEvent()
        {
            itemParent = GameObject.FindGameObjectWithTag("ItemParent").transform;
            RecreateAllItems();
        }

        /// <summary>
        /// 在世界地图生成物品
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnInstantiateItemScen(int ID, Vector3 pos)
        {
            Item item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = ID;
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
            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };
                currentSceneItems.Add(sceneItem);

                if (sceneItemDict.ContainsKey(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name))
                {
                    //找剄数据就更新tem数据列表
                    sceneItemDict[UnityEngine.SceneManagement.SceneManager.GetActiveScene().name] = currentSceneItems;
                }
                else
                {   //如果是新场景
                    sceneItemDict.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, currentSceneItems);
                }
            }
        }

        /// <summary>
        /// 刷新重建当前场景物品 切换场景结束的时候
        /// </summary>
        private void RecreateAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();
            if (sceneItemDict.TryGetValue(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, out currentSceneItems))
            {
                //清场
                foreach (var item in FindObjectsOfType<Item>())
                    Destroy(item.gameObject);
                //重新创建   
                foreach (var item in currentSceneItems)
                {
                    Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
                    newItem.Init(item.itemID);
                }

                ////清场
                //for (int i = 0; i < FindObjectsOfType<Item>()?.Length; i++)
                //    Destroy(FindObjectsOfType<Item>()[i].gameObject);
                // //重新创建   
                //for (int i = 0; i < currentSceneItems?.Count; i++)
                //{
                //    Item newItem = Instantiate(itemPrefab, currentSceneItems[i].position.ToVector3(), Quaternion.identity, itemParent);
                //    newItem.Init(currentSceneItems[i].itemID);
                //}
            }
        }
    }
}
