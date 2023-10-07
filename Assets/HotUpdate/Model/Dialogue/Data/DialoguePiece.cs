using UnityEngine;
using UnityEngine.Events;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    对话数据

-----------------------*/

namespace Farm2D
{
    [System.Serializable]
    public class DialoguePiece
    {
        [Header("对话详情")]
        public Sprite faceImage;
        public bool onLeft;
        public string name;
        [TextArea]
        public string dialogueText;
        public bool hasToPause;
        [HideInInspector] public bool isDone;//是否结束
        public UnityEvent OnFinishEvent;    //每个对话完成后触发的事件
        public int nextDialogue;//下一个对话的ID
    }
}
