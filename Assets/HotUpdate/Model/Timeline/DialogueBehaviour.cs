using Core;
using Farm2D;
using UnityEngine;
using UnityEngine.Playables;
using Debug = Core.Debug;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	玩家剧情对话

-----------------------*/

namespace Farm2D
{
    [System.Serializable]
    public class DialogueBehaviour : PlayableBehaviour
    {
        private PlayableDirector director;  //Timeline控制器
        public DialoguePiece dialoguePiece; //播放对象

        public override void OnPlayableCreate(Playable playable)
        {
            director = (playable.GetGraph().GetResolver() as PlayableDirector);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            //呼叫启动UI
            ConfigEvent.ShowDialogueEvent.EventTrigger(dialoguePiece);
            if (Application.isPlaying)//如果动画正在播放的时候
            {
                if (dialoguePiece.hasToPause)//如果可以暂停的话
                    ModelTimeline.Instance.PauseTimeline(director);//暂停timeline
                else
                    ConfigEvent.ShowDialogueEvent.EventTrigger<DialoguePiece>(null);
            }
        }

        //在Timeline播放期间每帧执行
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (Application.isPlaying)
            {
                ModelTimeline.Instance.isDone = dialoguePiece.isDone;//每帧检测是否播放完成
                ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Pause);
            }
        }
        //当前片段结束后暂停
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            ConfigEvent.ShowDialogueEvent.EventTrigger<DialoguePiece>(null);
        }
        public override void OnGraphStart(Playable playable)
        {
            Debug.Log($"游戏暂停了");
            ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Pause);
        }
        //整条时间线播放完成后才会关闭
        public override void OnGraphStop(Playable playable)
        {
            Debug.Log($"游戏运行了");
            ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Gameplay);
            ModelAudio.Instance.OnAfterSceneLoadedEvent();
        }
    }
}
