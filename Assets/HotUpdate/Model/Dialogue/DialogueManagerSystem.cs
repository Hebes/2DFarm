using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	对话管理系统

-----------------------*/

namespace ACFrameworkCore
{
    public class DialogueManagerSystem : ICore
    {
        public static DialogueManagerSystem Instance;
        public List<DialoguePiece> dialogueList;                    //对话内容
        private Stack<DialoguePiece> dailogueStack;                 //对话堆栈

        public void ICroeInit()
        {
            Instance = this;
            dialogueList = new List<DialoguePiece>();
            List<DialogueDetailsData> dialogueDetailsDataList = DataExpansion.GetDataList<DialogueDetailsData>();
            foreach (DialogueDetailsData dialogueDetailsData in dialogueDetailsDataList)
            {
                DialoguePiece dialoguePieceTemp = new DialoguePiece();
                dialoguePieceTemp.faceImage = ResourceExtension.Load<Sprite>(dialogueDetailsData.faceImage);//加载图片
                dialoguePieceTemp.onLeft = dialogueDetailsData.onLeft;
                dialoguePieceTemp.name = dialogueDetailsData.name;
                dialoguePieceTemp.dialogueText = dialogueDetailsData.dialogueText;
                dialoguePieceTemp.hasToPause = dialogueDetailsData.hasToPause;
                dialoguePieceTemp.isDone = dialogueDetailsData.isDone;
                dialogueList.Add(dialoguePieceTemp);
            }
        }
    }
}
