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
        }

        //切换场景
        private async UniTaskVoid SceneTransition(string targetScene, Vector3 targetPosition)
        {
            if (!isFade)//如果是切换场景的情况下
            {
                isFade = true;
                ConfigEvent.SceneBeforeUnload.EventTrigger();
                await ConfigUIPanel.UIFadePanel.GetUIPanl<UIFadePanel>().Fade(1);
                SceneOperationHandle sceneOperationHandle = await targetScene.LoadSceneAsyncUnitask(LoadSceneMode.Additive);//加载新的场景
                sceneOperationHandle.ActivateScene();//设置场景激活
                currentceneName.UnloadAsync();//卸载原来的场景
                currentceneName = targetScene;//变换当前场景的名称
                ConfigEvent.PlayerMoveToPosition.EventTrigger(targetPosition);  //移动人物坐标
                ConfigEvent.SceneAfterLoaded.EventTrigger();                    //加载场景之后需要做的事情
                ConfigEvent.SwichConfinerShape.EventTrigger();                  //切换场景边界
                ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮
                await UniTask.DelayFrame(20);
                await ConfigUIPanel.UIFadePanel.GetUIPanl<UIFadePanel>().Fade(0);
                isFade = false;
            }
        }
    }
}
