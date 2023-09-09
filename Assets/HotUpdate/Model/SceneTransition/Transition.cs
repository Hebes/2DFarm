﻿using Cysharp.Threading.Tasks;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    场景跳转

-----------------------*/

namespace ACFrameworkCore
{
    public class Transition : MonoBehaviour
    {
        [SceneName] public string sceneToGo;        //去往的场景
        public Vector3 positionTpGo;                //去的场景的坐标
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(ConfigTag.TagPlayer)) return;
            ConfigEvent.SceneTransition.EventTriggerUniTask(sceneToGo, positionTpGo).Forget();
        }
    }
}