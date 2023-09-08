using Cysharp.Threading.Tasks;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            Vector3 positionTpGo = Vector3.zero;
            //更具场景切换传送下个场景玩家需要去往的位置
            switch (sceneToGo)
            {
                case ConfigScenes.Field: positionTpGo = new Vector3(11.78f, 22f, 0); break;
                case ConfigScenes.Home: positionTpGo = new Vector3(-14.02f, -6.77f, 0); break;
                case ConfigScenes.Market: positionTpGo = new Vector3(9.6f, 30f, 0); break;
            }
            if (!other.CompareTag(ConfigTag.TagPlayer)) return;
            ConfigEvent.SceneTransition.EventTriggerUniTask(sceneToGo, positionTpGo).Forget();
        }
    }
}
