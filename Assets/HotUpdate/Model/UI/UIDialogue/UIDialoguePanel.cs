using Core;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	对话功能UI

-----------------------*/

namespace Farm2D
{
    public class UIDialoguePanel : UIBase
    {

        public Text dailogueText;
        public Image faceRight, faceLeft;
        public Text nameRight, nameLeft;
        public GameObject continueBox;

        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Normal, EUIMode.Normal, EUILucenyType.Pentrate);

            UIComponent UIComponent = panelGameObject.GetComponent<UIComponent>();

            GameObject T_ContinueBox = UIComponent.Get<GameObject>("T_ContinueBox");
            GameObject T_DialoguePanel = UIComponent.Get<GameObject>("T_DialoguePanel");
            GameObject T_FaceRightText = UIComponent.Get<GameObject>("T_FaceRightText");
            GameObject T_FaceRight = UIComponent.Get<GameObject>("T_FaceRight");
            GameObject T_FaceLeftText = UIComponent.Get<GameObject>("T_FaceLeftText");
            GameObject T_FaceLeft = UIComponent.Get<GameObject>("T_FaceLeft");
            GameObject T_TalkBoxText = UIComponent.Get<GameObject>("T_TalkBoxText");

            dailogueText = T_TalkBoxText.GetComponent<Text>();
            faceRight = T_FaceRight.GetComponent<Image>();
            faceLeft = T_FaceLeft.GetComponent<Image>();
            nameRight = T_FaceRightText.GetComponent<Text>();
            nameLeft = T_FaceLeftText.GetComponent<Text>();
            continueBox = T_ContinueBox;

            continueBox.SetActive(false);
            ConfigEvent.ShowDialogueEvent.EventAdd<DialoguePiece>(OnShowDailogueEvent);
        }

        /// <summary>
        /// 在展示对话事件
        /// </summary>
        /// <param name="piece"></param>
        private void OnShowDailogueEvent(DialoguePiece piece)
        {
            MonoController.Instance.AddCoroutine("显示对话框",ShowDialogue(piece));
        }

        private IEnumerator ShowDialogue(DialoguePiece piece)
        {
            if (piece != null)
            {
                piece.isDone = false;

                OpenUIForm<UIDialoguePanel>(ConfigUIPanel.UIDialogue);
                continueBox.SetActive(false);

                dailogueText.text = string.Empty;

                if (piece.name != string.Empty)
                {
                    if (piece.onLeft)
                    {
                        faceRight.gameObject.SetActive(false);
                        faceLeft.gameObject.SetActive(true);
                        faceLeft.sprite = piece.faceImage;
                        nameLeft.text = piece.name;
                    }
                    else
                    {
                        faceRight.gameObject.SetActive(true);
                        faceLeft.gameObject.SetActive(false);
                        faceRight.sprite = piece.faceImage;
                        nameRight.text = piece.name;
                    }
                }
                else
                {
                    faceLeft.gameObject.SetActive(false);
                    faceRight.gameObject.SetActive(false);
                    nameLeft.gameObject.SetActive(false);
                    nameRight.gameObject.SetActive(false);
                }
                yield return dailogueText.DOText(piece.dialogueText, 1f).WaitForCompletion();

                piece.isDone = true;

                if (piece.hasToPause && piece.isDone)
                    continueBox.SetActive(true);
            }
            else
            {
                CloseUIForm();
                yield break;
            }
        }
    }
}
