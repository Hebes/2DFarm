using Core;
using System.Collections.Generic;
using UnityEngine;
using Debug = Core.Debug;

/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    NPC管理系统

-----------------------*/

namespace Farm2D
{
    public class NPCManagerSystem : SingletonNewMono<NPCManagerSystem>
    {
        public List<NPCPosition> npcPositionList;       //NPC列表
        private Dictionary<string, SceneRoute> sceneRouteDict = new Dictionary<string, SceneRoute>();

        protected void Awake()
        {
            //初始化NPC列表
            npcPositionList = new List<NPCPosition>();
            GameObject[] NPCS = GameObject.FindGameObjectsWithTag(ConfigTag.TagNPC);
            foreach (GameObject NPC in NPCS)
            {
                NPCPosition nPCPosition = new NPCPosition();
                nPCPosition.npc = NPC.transform;
                nPCPosition.position = new Vector3(-2f, -1.4f, 0);
                nPCPosition.startScene = NPC.GetComponent<NPCMovement>().currentScene; //ConfigScenes.Field;
                npcPositionList.Add(nPCPosition);
            }
            //初始化路径字典
            List<SceneRouteDetailsData> sceneRouteDetailsDataList = this.GetDataList<SceneRouteDetailsData>();
            if (sceneRouteDetailsDataList.Count == 0)
                return;
            foreach (SceneRouteDetailsData route in sceneRouteDetailsDataList)
            {
                var key = route.fromSceneName + route.gotoSceneName;
                if (sceneRouteDict.ContainsKey(key))
                    continue;
                SceneRoute sceneRoute = new SceneRoute();
                sceneRoute.fromSceneName = route.fromSceneName;
                sceneRoute.gotoSceneName = route.gotoSceneName;
                sceneRoute.scenePathList = new List<ScenePath>();
                for (int i = 0; i < route.gotoGridCellX.Count; i++)
                {
                    ScenePath scenePath = new ScenePath();
                    scenePath.sceneName = route.sceneName[i];
                    scenePath.fromGridCell = new Vector2Int(route.fromGridCellX[i], route.fromGridCellY[i]);
                    scenePath.gotoGridCell = new Vector2Int(route.gotoGridCellX[i], route.gotoGridCellY[i]);
                    sceneRoute.scenePathList.Add(scenePath);
                }
                sceneRouteDict.Add(key, sceneRoute);
            }

            //事件监听
            ConfigEvent.StartNewGameEvent.AddEventListener<int>(OnStartNewGameEvent);
        }

        private void OnStartNewGameEvent(int obj)
        {
            foreach (NPCPosition character in npcPositionList)
            {
                character.npc.position = character.position;
                character.npc.GetComponent<NPCMovement>().currentScene = character.startScene;
            }
        }

        /// <summary>
        /// 获得两个场景间的路径
        /// </summary>
        /// <param name="fromSceneName">起始场景</param>
        /// <param name="gotoSceneName">目标场景</param>
        /// <returns></returns>
        public SceneRoute GetSceneRoute(string fromSceneName, string gotoSceneName)
        {
            if (!sceneRouteDict.ContainsKey(fromSceneName + gotoSceneName))
                Debug.Error($"配置文件SceneRouteDetailsData未配置{fromSceneName}{gotoSceneName}");
            return sceneRouteDict[fromSceneName + gotoSceneName];
        }
    }
}
