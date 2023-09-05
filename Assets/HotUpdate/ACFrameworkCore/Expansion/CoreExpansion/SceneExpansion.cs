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
        
        public static async UniTask<SceneOperationHandle> LoadSceneAsyncUnitask(this string SceneName, 
            LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool suspendLoad = false, int priority = 100)
        {
            return await ACSceneManager.Instance.LoadSceneAsync(SceneName, loadSceneMode, suspendLoad, priority);
        }
        public static  void UnloadAsync(this string scnenName)
        {
            ACSceneManager.Instance.UnloadAsync(scnenName);
        }
        public static void SetActivateScene(this string scnenName)
        {
            ACSceneManager.Instance.SetActivateScene(scnenName);
        }
    }
}
