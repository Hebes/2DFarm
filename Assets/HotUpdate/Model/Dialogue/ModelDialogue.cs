using Core;
using Cysharp.Threading.Tasks;
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

namespace Farm2D
{
    public class ModelDialogue : IModelInit
    {
        public static ModelDialogue Instance;
        public List<DialoguePiece> allDialogueList;                    //所有对话内容

        public async UniTask ModelInit()
        {
            Instance = this;
            InitDialogueData();

            await UniTask.Yield();
        }

        /// <summary>
        /// 初始化对话数据
        /// </summary>
        private void InitDialogueData()
        {
            allDialogueList = new List<DialoguePiece>();
            List<DialogueDetailsData> dialogueDetailsDataList = this.GetDataList<DialogueDetailsData>();
            foreach (DialogueDetailsData dialogueDetailsData in dialogueDetailsDataList)
            {
                DialoguePiece dialoguePieceTemp = new DialoguePiece();
                dialoguePieceTemp.faceImage = LoadResExtension.Load<Sprite>(dialogueDetailsData.faceImage);//加载图片
                dialoguePieceTemp.onLeft = dialogueDetailsData.onLeft;
                dialoguePieceTemp.name = dialogueDetailsData.name;
                dialoguePieceTemp.dialogueText = dialogueDetailsData.dialogueText;
                dialoguePieceTemp.hasToPause = dialogueDetailsData.hasToPause;
                dialoguePieceTemp.isDone = dialogueDetailsData.isDone;
                dialoguePieceTemp.nextDialogue = dialogueDetailsData.nextDialogue;
                allDialogueList.Add(dialoguePieceTemp);
            }
        }

        /// <summary>
        /// 获取需要对话的数据
        /// </summary>
        /// <param name="startNumber"></param>
        public void GetDialogueData(int startNumber,ref List<DialoguePiece> dialoguePieces)
        {
            //添加对话内容
            dialoguePieces.Add(allDialogueList[startNumber]);
            int nextDialogue = allDialogueList[startNumber].nextDialogue;
            if (nextDialogue != -1)
                GetDialogueData(nextDialogue, ref dialoguePieces);
        }
    }
}
