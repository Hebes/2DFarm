using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;
using YooAsset;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    场景加载拓展类

-----------------------*/

namespace ACFrameworkCore
{
    public static class SceneExpansion
    {
        public static void LoadSceneAsync(this string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            Action<SceneOperationHandle> action = null, bool suspendLoad = false, int priority = 100)
        {
            ManagerScene.Instance.LoadSceneAsync(SceneName, loadSceneMode, action, suspendLoad, priority);
        }

        public static async UniTask LoadSceneAsyncUnitask(this string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool suspendLoad = false, int priority = 100)
        {
            await ManagerScene.Instance.LoadSceneAsync(SceneName, loadSceneMode, suspendLoad, priority);
        }
    }
}
