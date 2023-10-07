using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    播放轨道

-----------------------*/

namespace Farm2D
{
    public class DialogueClip : PlayableAsset, ITimelineClipAsset
    {
        public ClipCaps clipCaps => ClipCaps.None;
        public DialogueBehaviour dailogue = new DialogueBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<DialogueBehaviour> playable = ScriptPlayable<DialogueBehaviour>.Create(graph, dailogue);//创建可以编辑的片段
            return playable;
        }
    }
}
