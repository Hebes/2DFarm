using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ACFrameworkCore
{
    public class ManagerScene : SingletonInit<ManagerScene>, ICore
    {
        private ISceneLoad sceneLoad;
        public void ICroeInit()
        {
            sceneLoad = new YooAssetLoadScene();
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="SceneName">场景名称</param>
        /// <param name="unityAction">加载完毕后的回调</param>
        public void LoadSceneAsync(string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            Action<SceneOperationHandle> action = null, bool suspendLoad = false, int priority = 100)
        {
            sceneLoad.LoadSceneAsync(SceneName, loadSceneMode, action, suspendLoad, priority);
        }
        public async UniTask LoadSceneAsync(string SceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool suspendLoad = false, int priority = 100)
        {
            await  sceneLoad.LoadSceneAsync(SceneName, loadSceneMode, suspendLoad, priority);
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
