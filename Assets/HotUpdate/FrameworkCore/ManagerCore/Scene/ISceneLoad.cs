/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    场景加载接口

-----------------------*/

using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using YooAsset;

namespace Core
{
    public interface ISceneLoad
    {
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="SceneName">场景名称</param>
        /// <param name="action"></param>
        /// <param name="loadSceneMode">场景加载模式</param>
        /// <param name="suspendLoad">场景加载到90%自动挂起</param>
        /// <param name="priority">优先级</param>
        /// <returns></returns>
        public UniTask<SceneOperationHandle> LoadSceneAsync(string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            bool suspendLoad = false, int priority = 100);

        public object GetManagerDic();
    }
}
