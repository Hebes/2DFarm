using ACFrameworkCore;
using UnityEngine;
using UnityEngine.Playables;

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
            //ConfigEvent.StartNewGameEvent.AddEventListener<int>(OnStartNewGameEvent);
            //MonoManager.Instance.OnAddUpdateEvent(Update);
        }

        public void Test()
        {

            currentDirector = startDirector;

            //startDirector.played += OnPlayed;//如果动画正在执行的话
            //startDirector.stopped += OnStopped;                                     //恢复运行
            //ConfigEvent.AfterSceneLoadedEvent.AddEventListener(OnAfterSceneLoadedEvent);
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


        //事件监听
        //新游戏
        private void OnStartNewGameEvent(int obj)
        {
            if (startDirector != null)
                startDirector.Play();
        }
        //场景加载完毕
        private void OnAfterSceneLoadedEvent()
        {
            startDirector = GameObject.FindObjectOfType<PlayableDirector>();
            currentDirector = GameObject.FindObjectOfType<PlayableDirector>();

            Test();
            if (currentDirector != null)
                currentDirector.Play();
            if (!startDirector.isActiveAndEnabled)
                ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Gameplay);
        }
        //在停止中
        private void OnStopped(PlayableDirector obj)
        {
            ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Gameplay);
        }
        //再播放中
        private void OnPlayed(PlayableDirector obj)
        {
            ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Pause);
        }
        //暂停时间线
        public void PauseTimeline(PlayableDirector director)
        {
            currentDirector = director;

            currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
            isPause = true;
        }
    }
}
