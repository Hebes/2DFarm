using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
        //public SceneRouteDataList_SO sceneRouteDate;

        //private Dictionary<string, SceneRoute> sceneRouteDict = new Dictionary<string, SceneRoute>();


        protected void Awake()
        {
            //初始化NPC列表
            npcPositionList = new List<NPCPosition>();
            GameObject[] NPCS = GameObject.FindGameObjectsWithTag(ConfigTag.TagNPC);
            foreach (GameObject NPC in NPCS)
            {
                NPCPosition nPCPosition = new NPCPosition();
                nPCPosition.npc = NPC.transform;
                nPCPosition.position = new Vector3(-2f, -1.4f,0);
                nPCPosition.startScene = ConfigScenes.FieldScenes;
                npcPositionList.Add(nPCPosition);
            }

            //InitSceneRouteDict();
        }
        private void OnEnable()
        {
            ConfigEvent.StartNewGame.AddEventListener<int>(OnStartNewGameEvent);
        }
        private void OnDisable()
        {
            ConfigEvent.StartNewGame.RemoveEventListener<int>(OnStartNewGameEvent);
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
        /// 初始化路径字典
        /// </summary>
        //private void InitSceneRouteDict()
        //{
        //    if (sceneRouteDate.sceneRouteList.Count > 0)
        //    {
        //        foreach (SceneRoute route in sceneRouteDate.sceneRouteList)
        //        {
        //            var key = route.fromSceneName + route.gotoSceneName;

        //            if (sceneRouteDict.ContainsKey(key))
        //                continue;

        //            sceneRouteDict.Add(key, route);
        //        }
        //    }
        //}

        /// <summary>
        /// 获得两个场景间的路径
        /// </summary>
        /// <param name="fromSceneName">起始场景</param>
        /// <param name="gotoSceneName">目标场景</param>
        /// <returns></returns>
        //public SceneRoute GetSceneRoute(string fromSceneName, string gotoSceneName)
        //{
        //    return sceneRouteDict[fromSceneName + gotoSceneName];
        //}
    }
}
