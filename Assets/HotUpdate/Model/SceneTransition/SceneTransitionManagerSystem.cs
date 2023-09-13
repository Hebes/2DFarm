using ACFrameworkCore;
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
    场景过渡管理系统

-----------------------*/

namespace ACFarm
{
    public class SceneTransitionManagerSystem : ICore, ISaveable
    {
        public static SceneTransitionManagerSystem Instance;
        public string currentceneName = string.Empty;//当前的场景,如果从记录中加载的场景可以在这里设置
        private bool isFade;//是否切换场景

        public Transform cropParent;       //庄家的父物体，都放在这个下面不会看上去太乱
        public Transform itemParent;

        public void ICroeInit()
        {
            Instance = this;

            ConfigEvent.SceneTransition.AddEventListenerUniTask<string, Vector3>(SceneTransition);
            ConfigEvent.StartNewGameEvent.AddEventListener<int>(P => { StartNewGameEvent(P).Forget(); });
            ConfigScenes.PersistentScene.LoadSceneAsyncUnitask(LoadSceneMode.Single).Forget();
            ConfigEvent.EndGameEvent.AddEventListener(OnEndGameEvent);
            //注册保存事件
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }

        //创建场景
        public async UniTask CreatScene()
        {
            await currentceneName.LoadSceneAsyncUnitask(LoadSceneMode.Additive);
            currentceneName.SetActivateScene();//设置为激活场景
            ConfigEvent.SwichConfinerShape.EventTrigger();//切换场景边界
            ConfigEvent.AfterSceneLoadedEvent.EventTrigger();                    //加载场景之后需要做的事情
        }


        //事件监听
        private async UniTask StartNewGameEvent(int obj)
        {
            currentceneName = ConfigScenes.Field;
            await SceneTransition(currentceneName, Vector3.zero);

            //测试创建拾取的物体
            GameObject gameObject = ResourceExtension.Load<GameObject>(ConfigPrefab.ItemBasePreafab);

            GameObject.Instantiate(gameObject).GetComponent<Item>().Init(1001, 1);
            GameObject.Instantiate(gameObject).GetComponent<Item>().Init(1002, 1);
            GameObject.Instantiate(gameObject).GetComponent<Item>().Init(1003, 1);
            GameObject.Instantiate(gameObject).GetComponent<Item>().Init(1004, 1);
            GameObject.Instantiate(gameObject).GetComponent<Item>().Init(1005, 1);
            GameObject.Instantiate(gameObject).GetComponent<Item>().Init(1006, 1);
            GameObject.Instantiate(gameObject).GetComponent<Item>().Init(1007, 10);
            GameObject.Instantiate(gameObject).GetComponent<Item>().Init(1018, 10);
            GameObject.Instantiate(gameObject).GetComponent<Item>().Init(1015, 119);
            GameObject.Instantiate(gameObject).GetComponent<Item>().Init(1032, 10);
            GameObject.Instantiate(gameObject).GetComponent<Item>().Init(1033, 10);
        }
        private async UniTask SceneTransition(string targetScene, Vector3 targetPosition)
        {
            if (!isFade)//如果是切换场景的情况下
            {
                isFade = true;
                ConfigEvent.BeforeSceneUnloadEvent.EventTrigger();
                await ConfigEvent.UIFade.EventTriggerUniTask((float)1);
                if (!string.IsNullOrEmpty(currentceneName))
                    currentceneName.UnloadAsync();                                  //卸载原来的场景
                SceneOperationHandle sceneOperationHandle = await targetScene.LoadSceneAsyncUnitask(LoadSceneMode.Additive);//加载新的场景
                sceneOperationHandle.ActivateScene();                           //设置场景激活
                currentceneName = targetScene;                                  //变换当前场景的名称
                ConfigEvent.PlayerMoveToPosition.EventTrigger(targetPosition);  //移动人物坐标
                ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮
                await UniTask.DelayFrame(40);
                //创建每个场景必要的物体
                cropParent = new GameObject(ConfigTag.TagCropParent).transform;
                itemParent = new GameObject(ConfigTag.TagItemParent).transform;

                ConfigEvent.SwichConfinerShape.EventTrigger();                  //切换场景边界
                ConfigEvent.AfterSceneLoadedEvent.EventTrigger();                    //加载场景之后需要做的事情
                await ConfigEvent.UIFade.EventTriggerUniTask((float)0);
                isFade = false;
            }
        }
        private void OnEndGameEvent()
        {
            UnloadScene().Forget();
        }
        private async UniTask UnloadScene()
        {
            ConfigEvent.BeforeSceneUnloadEvent.EventTrigger();
            await ConfigEvent.UIFade.EventTriggerUniTask((float)1);
            SceneManager.GetActiveScene().name.UnloadAsync();// SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            await ConfigEvent.UIFade.EventTriggerUniTask((float)0);
        }


        //保存数据
        private string SaveKey = "场景过渡管理系统";
        public string GUID => SaveKey;
        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.dataSceneName = SceneManager.GetActiveScene().name;
            return saveData;
        }
        public void RestoreData(GameSaveData saveData)
        {
            SceneTransition(saveData.dataSceneName, Vector3.zero).Forget();
        }
    }
}
