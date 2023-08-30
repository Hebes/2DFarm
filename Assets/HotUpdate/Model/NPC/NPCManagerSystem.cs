using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    NPC管理系统

-----------------------*/

namespace ACFrameworkCore
{
    public class NPCManagerSystem : SingletonNewMono<NPCManagerSystem>
    {
        public List<NPCPosition> npcPositionList;       //NPC列表
        private Dictionary<string, SceneRoute> sceneRouteDict = new Dictionary<string, SceneRoute>();


        protected void Awake()
        {
            InitNPCPositionList();
            InitSceneRouteDict();
            ConfigEvent.StartNewGame.AddEventListener<int>(OnStartNewGameEvent);
        }

        private void OnStartNewGameEvent(int obj)
        {
            foreach (var character in npcPositionList)
            {
                character.npc.position = character.position;
                character.npc.GetComponent<NPCMovement>().currentScene = character.startScene;
            }
        }

        /// <summary>
        /// 初始化NPC列表
        /// </summary>
        private void InitNPCPositionList()
        {
            //初始化NPC列表
            npcPositionList = new List<NPCPosition>();
            GameObject[] NPCS = GameObject.FindGameObjectsWithTag(ConfigTag.TagNPC);
            foreach (GameObject NPC in NPCS)
            {
                NPCPosition nPCPosition = new NPCPosition();
                nPCPosition.npc = NPC.transform;
                nPCPosition.position = new Vector3(-2f, -1.4f, 0);
                nPCPosition.startScene = ConfigScenes.Field;
                npcPositionList.Add(nPCPosition);
            }
        }

        /// <summary>
        /// 初始化路径字典
        /// </summary>
        private void InitSceneRouteDict()
        {
            List<SceneRouteDetailsData> sceneRouteDetailsDataList = this.GetDataListT<SceneRouteDetailsData>();
            if (sceneRouteDetailsDataList.Count == 0)
                return;
            foreach (SceneRouteDetailsData route in sceneRouteDetailsDataList)
            {
                var key = route.fromSceneName + route.gotoSceneName;
                if (sceneRouteDict.ContainsKey(key))
                    continue;
                SceneRoute sceneRoute = new SceneRoute();
                sceneRoute.fromSceneName= route.fromSceneName;
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
        }

        /// <summary>
        /// 获得两个场景间的路径
        /// </summary>
        /// <param name="fromSceneName">起始场景</param>
        /// <param name="gotoSceneName">目标场景</param>
        /// <returns></returns>
        public SceneRoute GetSceneRoute(string fromSceneName, string gotoSceneName)
        {
            return sceneRouteDict[fromSceneName + gotoSceneName];
        }
    }
}
