using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    场景过渡系统

-----------------------*/

namespace ACFrameworkCore
{
    public class SceneTransitionSystem : ICore
    {
        public static SceneTransitionSystem Instance;
        public string currentceneName = string.Empty;//当前的场景,如果从记录中加载的场景可以在这里设置
        private bool isFade;//是否切换场景

        public void ICroeInit()
        {
            Instance = this;
            currentceneName = ConfigScenes.FieldScenes;
            ConfigEvent.SceneTransition.AddEventListener<string, Vector3>((arg2, pos) => { SceneTransition(arg2, pos).Forget(); });
            CreatScene().Forget();
        }

        private async UniTaskVoid CreatScene()
        {
            await ConfigScenes.PersistentSceneScenes.LoadSceneAsyncUnitask(LoadSceneMode.Single);
            await currentceneName.LoadSceneAsyncUnitask(LoadSceneMode.Additive);
            currentceneName.SetActivateScene();//设置为激活场景
            ConfigEvent.SwichConfinerShape.EventTrigger();//切换场景边界

            ////测试创建拾取的物体
            //GameObject gameObject = await ResourceExtension.LoadAsyncUniTask<GameObject>(ConfigPrefab.ItemBasePrefab);

            //GameObject go1 = GameObject.Instantiate(gameObject);
            //Item item = go1.GetComponent<Item>();
            //item.itemID = 1007;
            //item.itemAmount = 3;

            //GameObject go2 = GameObject.Instantiate(gameObject);
            //Item item2 = go2.GetComponent<Item>();
            //item2.itemID = 1008;
            //item2.itemAmount = 6;

            //GameObject go3 = GameObject.Instantiate(gameObject);
            //Item item3 = go3.GetComponent<Item>();
            //item3.itemID = 1015;
            //item3.itemAmount = 119;
        }

        //切换场景
        private async UniTaskVoid SceneTransition(string targetScene, Vector3 targetPosition)
        {
            if (!isFade)//如果是切换场景的情况下
            {
                ConfigEvent.SceneBeforeUnload.EventTrigger();
                await Fade(1);
                SceneOperationHandle sceneOperationHandle = await targetScene.LoadSceneAsyncUnitask(LoadSceneMode.Additive);//加载新的场景
                sceneOperationHandle.ActivateScene();//设置场景激活
                currentceneName.UnloadAsync();//卸载原来的场景
                currentceneName = targetScene;//变换当前场景的名称
                
                ConfigEvent.PlayerMoveToPosition.EventTrigger(targetPosition);  //移动人物坐标
                ConfigEvent.SceneAfterLoaded.EventTrigger();
                ConfigEvent.SwichConfinerShape.EventTrigger();                  //切换场景边界
                await Fade(0);
            }
        }
        /// <summary>loading画面的显示与隐藏 淡入淡出场景</summary>
        /// <param name="targetAlpha">1是黑 0是透明</param>
        /// <returns></returns>
        private async UniTask Fade(float targetAlpha)
        {
            await UniTask.Yield();
            //isFade = true;
            //fadeCanvasGroup.blocksRaycasts = true;
            //float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuretion;
            //while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))//Approximately 判断是否大概相似
            //{
            //    fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            //    await UniTask.Yield();
            //}
            //fadeCanvasGroup.blocksRaycasts = false;
            //isFade = false;
        }
    }
}
