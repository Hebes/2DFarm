using ACFrameworkCore;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	动画控制管理系统

-----------------------*/

namespace ACFarm
{
    public class TimelineManagerSystem : ICore
    {
        public static TimelineManagerSystem Instance;

        public PlayableDirector startDirector;              //游戏初始化的那个，不是每个场景的
        private PlayableDirector currentDirector;           //每个场景的动画
        public bool isDone;                         //是否播放完毕
        private bool isPause;                       //是否暂停



        //生命周期
        public void ICroeInit()
        {
            Instance = this;
            //必须先初始化过场动画UI界面
            //ConfigUIPanel.UIIntro.ShwoUIPanel<UIIntroPanel>();               //显示TimeLine
            //startDirector = UIManagerExpansion.GetUIPanl<UIIntroPanel>(ConfigUIPanel.UIIntro).panelGameObject.transform.GetChildComponent<PlayableDirector>("UIIntro1");
            //currentDirector = startDirector;

            //startDirector.played += OnPlayed;
            //startDirector.stopped += OnStopped;                                     //恢复运行
            ConfigEvent.AfterSceneLoadedEvent.AddEventListener(OnAfterSceneLoadedEvent);
            ConfigEvent.StartNewGameEvent.AddEventListener<int>(OnStartNewGameEvent);
            MonoManager.Instance.OnAddUpdateEvent(Update);
        }
        private void Update()
        {
            if (isPause && Input.GetKeyDown(KeyCode.Space) && isDone)
            {
                isPause = false;
                currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
            }
        }

        public void Test()
        {
            //startDirector.played += OnPlayed;//如果动画正在执行的话
            //startDirector.stopped += OnStopped;                                     //恢复运行
            //ConfigEvent.AfterSceneLoadedEvent.AddEventListener(OnAfterSceneLoadedEvent);
            //ConfigEvent.StartNewGameEvent.AddEventListener<int>(OnStartNewGameEvent);
            //MonoManager.Instance.OnAddUpdateEvent(Update);
        }

        //事件监听
        /// <summary>
        /// 新游戏
        /// </summary>
        /// <param name="obj"></param>
        private void OnStartNewGameEvent(int obj)
        {
            startDirector?.Play();
        }
        /// <summary>
        /// 场景加载完毕
        /// </summary>
        private void OnAfterSceneLoadedEvent()
        {
            //这个场景是否是第一次启动
            string currentSceneName = SceneManager.GetActiveScene().name;
            bool isScenFirst = GridMapManagerSystem.Instance.GetSceneFirstLoad(currentSceneName);
            if (isScenFirst == false) return;
            startDirector = PersistentSceneManagerSystem.StaticGetCutscene(currentSceneName)?.GetComponent<PlayableDirector>();//获取当前场景的过场动画组件
            currentDirector = startDirector;
            if (currentDirector != null)
            {
                AudioManagerSystem.Instance.StopAllSound();
                currentDirector.Play();
            }
            if (startDirector != null && !startDirector.isActiveAndEnabled)
                ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Gameplay);
        }
        /// <summary>
        /// 在停止中
        /// </summary>
        /// <param name="obj"></param>
        private void OnStopped(PlayableDirector obj)
        {
            ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Gameplay);
        }
        /// <summary>
        /// 再播放中
        /// </summary>
        /// <param name="obj"></param>
        private void OnPlayed(PlayableDirector obj)
        {
            ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Pause);
        }
        /// <summary>
        /// 暂停时间线
        /// </summary>
        /// <param name="director"></param>
        public void PauseTimeline(PlayableDirector director)
        {
            currentDirector = director;

            currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
            isPause = true;
        }
    }
}
