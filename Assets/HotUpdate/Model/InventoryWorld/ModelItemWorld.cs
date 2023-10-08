using Farm2D;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    世界地图上的物品管理

-----------------------*/

namespace Farm2D
{
    public class ModelItemWorld : IModelInit, ISaveable
    {
        public static ModelItemWorld Instance;
        private Transform playerTransform;                          //玩家
        private Transform itemParent;                               //统一保存的父物体
        public Item bounceItemPrefab;                               //抛投的物品模板
        public Item itemPrefab;
        private string SaveKey = "世界地图上的物品管理";

        private Dictionary<string, List<SceneFurniture>> sceneFurnitureDict;    //记录场景家具
        private Dictionary<string, List<SceneItem>> sceneItemDict;              //世界场景的所有物体

        public async UniTask ModelInit()
        {
            Instance = this;
            sceneItemDict = new Dictionary<string, List<SceneItem>>();
            sceneFurnitureDict = new Dictionary<string, List<SceneFurniture>>();
            //加载预制体
            bounceItemPrefab = YooAssetLoadExpsion.YooaddetLoadSync<GameObject>(ConfigPrefab.prefabBonnceItemBasePreafab).GetComponent<Item>();
            //初始化监听信息
            ConfigEvent.InstantiateItemInScene.EventAdd<int, int, Vector3>(OnInstantiateItemScen);
            ConfigEvent.UIItemDropItem.EventAdd<string, int, Vector3, EItemType, int>(OnDropItemEvent);//扔东西
            ConfigEvent.BeforeSceneUnloadEvent.EventAdd(OnBeforeSceneUnloadEvent);
            ConfigEvent.AfterSceneLoadedEvent.EventAdd(OnAfterSceneLoadedEvent);
            ConfigEvent.StartNewGameEvent.EventAdd<int>(OnStartNewGameEvent);
            LoadInit().Forget();

            //注册保存事件
            ISaveable saveable = this;
            saveable.RegisterSaveable();

            await UniTask.Yield();
        }


        public async UniTaskVoid LoadInit()
        {
            GameObject itemPrefabGo = await LoadResExtension.LoadAsync<GameObject>(ConfigPrefab.prefabItemBasePreafab);
            itemPrefab = itemPrefabGo.GetComponent<Item>();

            await UniTask.Yield();
        }



        //事件监听
        private void OnStartNewGameEvent(int obj)
        {
            sceneItemDict.Clear();
            sceneFurnitureDict.Clear();
        }
        private void OnDropItemEvent(string itemKey, int itemID, Vector3 mousePos, EItemType itemType, int removeAmount)
        {
            if (itemType == EItemType.Seed)
            {
                //UIDragPanel uIDragPanel1 = UIManagerExpansion.GetUIPanl<UIDragPanel>(ConfigUIPanel.UIDragPanel);
                ModelItem.Instance.RemoveItem(itemKey, itemID, removeAmount);
                return;
            }

            Item item = GameObject.Instantiate(bounceItemPrefab, playerTransform.position, Quaternion.identity, itemParent);
            //抛出方向
            var dir = (mousePos - playerTransform.position).normalized;
            item.GetComponent<ItemBounce>().InitBounceItem(mousePos, dir);
            item.Init(itemID, removeAmount);
            //获取数据
            //UIDragPanel uIDragPanel = UIManagerExpansion.GetUIPanl<UIDragPanel>(ConfigUIPanel.UIDragPanel);
            //item.itemID = itemID;
            //item.itemAmount = removeAmount;
            //if (removeAmount > item.itemAmount)
            //    item.itemAmount = uIDragPanel.itemAmount;
            //移除物品
            ModelItem.Instance.RemoveItem(itemKey, itemID, removeAmount);
        }
        private void OnAfterSceneLoadedEvent()
        {
            playerTransform = ModelCommon.Instance.playerTransform;// GameObject.FindGameObjectWithTag(ConfigTag.TagPlayer).transform;
            itemParent = ModelSceneTransition.Instance.itemParent;
            RecreateAllItems();
            RebuildFurniture();
        }
        private void OnInstantiateItemScen(int itemID, int itemAmount, Vector3 pos)
        {
            Item item = GameObject.Instantiate(bounceItemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = itemID;
            item.itemAmount = itemAmount;
            item.GetComponent<ItemBounce>().InitBounceItem(pos, Vector3.up);
        }
        private void OnBeforeSceneUnloadEvent()
        {
            GetAllSceneItems();
            GetAllSceneFurniture();
        }



        //保存场景数据
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

                if (sceneItemDict.ContainsKey(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name))
                    sceneItemDict[UnityEngine.SceneManagement.SceneManager.GetActiveScene().name] = currentSceneItems;//找剄数据就更新tem数据列表
                else
                    sceneItemDict.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, currentSceneItems);//如果是新场景
            }
        }
        private void RecreateAllItems()
        {
            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out List<SceneItem> currentSceneItems))
            {
                if (currentSceneItems == null) return;
                //清场
                foreach (var item in Object.FindObjectsOfType<Item>())
                    GameObject.Destroy(item.gameObject);
                //重新创建   
                foreach (SceneItem sceneItem in currentSceneItems)
                {
                    Item newItem = GameObject.Instantiate(itemPrefab, sceneItem.position.ToVector3(), Quaternion.identity, itemParent);
                    newItem.Init(sceneItem.itemID, sceneItem.ItemAmount);
                }
            }
        }
        //获取场景家具
        private void GetAllSceneFurniture()
        {
            List<SceneFurniture> currentSceneFurniture = new List<SceneFurniture>();
            Furniture[] furnitures = GameObject.FindObjectsOfType<Furniture>();
            foreach (Furniture item in furnitures)
            {
                SceneFurniture sceneFurniture = new SceneFurniture
                {
                    itemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };

                if (item.GetComponent<Box>())
                    sceneFurniture.name = item.GetComponent<Box>().boxName;

                currentSceneFurniture.Add(sceneFurniture);
                GameObject.Destroy(item.gameObject);//删除物体
            }

            if (sceneFurnitureDict.ContainsKey(SceneManager.GetActiveScene().name))
                sceneFurnitureDict[SceneManager.GetActiveScene().name] = currentSceneFurniture;//找到数据就更新item数据列表
            else
                sceneFurnitureDict.Add(SceneManager.GetActiveScene().name, currentSceneFurniture);//如果是新场景
        }
        //刷新家具
        private void RebuildFurniture()
        {
            if (sceneFurnitureDict.TryGetValue(SceneManager.GetActiveScene().name, out List<SceneFurniture> currentSceneFurniture))
            {
                if (currentSceneFurniture == null) return;
                Furniture[] furnitures = GameObject.FindObjectsOfType<Furniture>();
                foreach (Furniture item in furnitures)
                    GameObject.Destroy(item.gameObject);//删除物体
                //生成
                foreach (SceneFurniture sceneFurniture in currentSceneFurniture)
                {
                    BluePrintDetails bluePrint = ModelBuild.Instance.GetDataOne(sceneFurniture.itemID);
                    var buildItem = GameObject.Instantiate(bluePrint.buildPrefab, sceneFurniture.position.ToVector3(), Quaternion.identity, itemParent);
                    if (buildItem.GetComponent<Box>())
                    {
                        buildItem.GetComponent<Box>().boxName = sceneFurniture.name;
                        buildItem.GetComponent<Box>().InitBox();
                    }
                }
            }
        }



        //保存数据
        public string GUID => SaveKey;
        public GameSaveData GenerateSaveData()
        {
            GetAllSceneItems();
            GetAllSceneFurniture();

            GameSaveData saveData = new GameSaveData();
            saveData.sceneItemDict = sceneItemDict;
            saveData.sceneFurnitureDict = sceneFurnitureDict;

            return saveData;
        }
        public void RestoreData(GameSaveData saveData)
        {
            sceneItemDict = saveData.sceneItemDict;
            sceneFurnitureDict = saveData.sceneFurnitureDict;

            RecreateAllItems();
            RebuildFurniture();
        }


    }
}
