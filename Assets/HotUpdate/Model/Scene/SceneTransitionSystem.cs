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
            currentceneName = ConfigScenes.Field;
            ConfigEvent.SceneTransition.AddEventListenerUniTask<string, Vector3>(SceneTransition);
        }

        public async UniTask CreatScene()
        {
            await ConfigScenes.PersistentScene.LoadSceneAsyncUnitask(LoadSceneMode.Single);
            await currentceneName.LoadSceneAsyncUnitask(LoadSceneMode.Additive);
            currentceneName.SetActivateScene();//设置为激活场景
            ConfigEvent.SwichConfinerShape.EventTrigger();//切换场景边界
            ConfigEvent.SceneAfterLoaded.EventTrigger();                    //加载场景之后需要做的事情
        }

        //切换场景
        private async UniTask SceneTransition(string targetScene, Vector3 targetPosition)
        {
            if (!isFade)//如果是切换场景的情况下
            {
                isFade = true;
                ConfigEvent.SceneBeforeUnload.EventTrigger();
                await ConfigEvent.UIFade.EventTriggerUniTask((float)1);
                SceneOperationHandle sceneOperationHandle = await targetScene.LoadSceneAsyncUnitask(LoadSceneMode.Additive);//加载新的场景
                sceneOperationHandle.ActivateScene();                           //设置场景激活
                currentceneName.UnloadAsync();                                  //卸载原来的场景
                currentceneName = targetScene;                                  //变换当前场景的名称
                ConfigEvent.PlayerMoveToPosition.EventTrigger(targetPosition);  //移动人物坐标
                ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮
                await UniTask.DelayFrame(40);
                ConfigEvent.SwichConfinerShape.EventTrigger();                  //切换场景边界
                ConfigEvent.SceneAfterLoaded.EventTrigger();                    //加载场景之后需要做的事情
                await ConfigEvent.UIFade.EventTriggerUniTask((float)0);
                isFade = false;
            }
        }

        /// <summary>
        /// 黑幕淡入
        /// </summary>
        /// <param name="self"></param>
        /// <param name="curtain"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        //public static IEnumerator Fadein(this UIMateLoadingComponent self, GameObject curtain, float speed)
        //{
        //    curtain.SetActive(true);
        //    Image image;
        //    image = curtain.GetComponent<Image>();
        //    while (image.color.a >= 0.1f)
        //    {
        //        image.color = Color.Lerp(image.color, Color.clear, speed * Time.deltaTime);
        //        yield return null;
        //    }
        //    curtain.SetActive(false);
        //}

        /// <summary>
        /// 黑幕淡出
        /// </summary>
        /// <param name="self"></param>
        /// <param name="curtain"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        //public static IEnumerator Fadeout(this UIMateLoadingComponent self, float speed)
        //{
        //    self.Curtain.SetActive(true);
        //    Image image;
        //    image = self.Curtain.GetComponent<Image>();
        //    while (image.color.a <= 0.999f)
        //    {
        //        image.color = Color.Lerp(image.color, Color.black, speed * Time.deltaTime);
        //        yield return null;
        //    }
        //}
    }
}
