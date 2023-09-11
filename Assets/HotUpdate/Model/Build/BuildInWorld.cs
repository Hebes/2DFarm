using ACFrameworkCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	在世界建造的数据

-----------------------*/

namespace ACFarm
{
    public class BuildInWorld
    {
        public static BuildInWorld Instance;
        private Dictionary<string, List<SceneFurniture>> sceneFurnitureDict;    //记录场景家具
        private Transform itemParent;
        public BuildInWorld()
        {
            Instance = this;
            sceneFurnitureDict = new Dictionary<string, List<SceneFurniture>>();
            ConfigEvent.BeforeSceneUnloadEvent.AddEventListener(SceneBeforeUnload);
            ConfigEvent.AfterSceneLoadedEvent.AddEventListener(SceneAfterLoaded);
        }

        private void SceneBeforeUnload()
        {
            GetAllSceneFurniture();
        }
        private void SceneAfterLoaded()
        {
            itemParent = SceneTransitionManagerSystem.Instance.itemParent;
            RebuildFurniture();
        }


        /// <summary>
        /// 获得场景所有家具
        /// </summary>
        private void GetAllSceneFurniture()
        {
            List<SceneFurniture> currentSceneFurniture = new List<SceneFurniture>();

            foreach (var item in GameObject.FindObjectsOfType<Furniture>())
            {
                SceneFurniture sceneFurniture = new SceneFurniture
                {
                    itemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };
                //if (item.GetComponent<Box>())
                //    sceneFurniture.boxIndex = item.GetComponent<Box>().index;

                currentSceneFurniture.Add(sceneFurniture);
            }

            if (sceneFurnitureDict.ContainsKey(SceneManager.GetActiveScene().name))
            {

                //找到数据就更新item数据列表
                sceneFurnitureDict[SceneManager.GetActiveScene().name] = currentSceneFurniture;
            }
            else    //如果是新场景
            {
                sceneFurnitureDict.Add(SceneManager.GetActiveScene().name, currentSceneFurniture);
            }
        }

        /// <summary>
        /// 重建当前场景家具
        /// </summary>
        private void RebuildFurniture()
        {
            List<SceneFurniture> currentSceneFurniture = new List<SceneFurniture>();

            if (sceneFurnitureDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneFurniture))
            {
                if (currentSceneFurniture != null)
                {
                    foreach (SceneFurniture sceneFurniture in currentSceneFurniture)
                    {
                        BluePrintDetails bluePrint = BuildManagerSystem.Instance.GetDataOne(sceneFurniture.itemID);
                        var buildItem = GameObject.Instantiate(bluePrint.buildPrefab, sceneFurniture.position.ToVector3(), Quaternion.identity, itemParent);
                        //if (buildItem.GetComponent<Box>())
                        //{
                        //    buildItem.GetComponent<Box>().InitBox(sceneFurniture.boxIndex);
                        //}
                    }
                }
            }
        }
    }
}
