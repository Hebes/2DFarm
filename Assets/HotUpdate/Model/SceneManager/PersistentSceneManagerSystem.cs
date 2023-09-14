using System.Collections.Generic;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	PersistentScene场景管理系统

-----------------------*/

namespace ACFarm
{
    public class PersistentSceneManagerSystem : MonoBehaviour
    {
        public static PersistentSceneManagerSystem instance;
        public List<GameObject> CutsceneList;   //过场动画管理

        private void Awake()
        {
            instance = this;
        }
        private void OnDestroy()
        {
            instance = null;
        }

        //公开接口
        /// <summary>
        /// 启动过场动画
        /// </summary>
        /// <param name="SceneName">过场动画的物体名称</param>
        public GameObject GetCutscene(string SceneName)
        {
            GameObject gameObject = CutsceneList.Find(x => x.name == SceneName);
            gameObject?.SetActive(true);
            return gameObject;
        }

        //静态调用方法
        public static GameObject StaticGetCutscene(string SceneName)
        {
            return instance.GetCutscene(SceneName);
        }
    }
}
