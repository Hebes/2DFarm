using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ACFrameworkCore;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    对话控制器

-----------------------*/

namespace ACFarm
{
    [RequireComponent(typeof(NPCMovement))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class DialogueController : MonoBehaviour
    {
        private NPCMovement npc;            //NPC移动组件
        public UnityEvent OnFinishEvent;    //对话完成后触发的事件
        public List<DialoguePiece> dialogueList;
        private Stack<DialoguePiece> dailogueStack;//后进先出
        private bool canTalk;   //能否对话
        private bool isTalking; //正在说话
        private GameObject uiSign;//交互组件
        public int dialogueStartNumber = 1;//对话开始的序列号



        private void Awake()
        {
            npc = GetComponent<NPCMovement>();
            uiSign = transform.GetChildComponent<Transform>("uiSign").gameObject;
            ConfigEvent.OnFinishEvent.AddEventListener<UnityEvent>(AddOnFinishEvent);
            DialogueManagerSystem.Instance.GetDialogueData(dialogueStartNumber, ref dialogueList);
            FillDialogueStack();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(ConfigTag.TagPlayer))
                canTalk = !npc.isMoving && npc.interactable;
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(ConfigTag.TagPlayer))
                canTalk = false;
        }
        private async void Update()
        {
            uiSign.SetActive(canTalk);

            if (canTalk & Input.GetKeyDown(KeyCode.Space) && !isTalking)
                await DailogueRoutine();
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="unityEvent"></param>
        public void AddOnFinishEvent(UnityEvent unityEvent)
        {
            OnFinishEvent = unityEvent;
        }

        /// <summary>
        /// 构建对话堆栈
        /// </summary>
        private void FillDialogueStack()
        {
            dailogueStack = new Stack<DialoguePiece>();
            for (int i = dialogueList.Count - 1; i > -1; i--)
            {
                dialogueList[i].isDone = false;
                dailogueStack.Push(dialogueList[i]);
            }
        }

        private IEnumerator DailogueRoutine()
        {
            isTalking = true;
            if (dailogueStack.TryPop(out DialoguePiece result))
            {
                //传到UI显示对话
                ConfigEvent.ShowDialogueEvent.EventTrigger(result);
                ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Pause);
                yield return new WaitUntil(() => result.isDone);
                isTalking = false;
            }
            else
            {
                ConfigEvent.UpdateGameStateEvent.EventTrigger(EGameState.Gameplay);
                ConfigEvent.ShowDialogueEvent.EventTrigger<DialoguePiece>(null);
                FillDialogueStack();
                isTalking = false;

                if (OnFinishEvent != null)
                {
                    OnFinishEvent?.Invoke();
                    canTalk = false;
                }
            }
        }
    }
}
