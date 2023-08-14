using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    玩家动作

-----------------------*/

namespace ACFrameworkCore
{
    public class AnimatorOverride : MonoBehaviour
    {
        private Animator[] animators;
        public SpriteRenderer holdItem;//被举起的物品的图片
        public List<AnimatorType> animatorTypes;//各部分动画列表
        private Dictionary<string, Animator> animatorNameDic;

        private async void Awake()
        {
            animatorTypes = new List<AnimatorType>();
            animatorNameDic = new Dictionary<string, Animator>();
            animators = GetComponentsInChildren<Animator>();
            foreach (var anim in animators)
                animatorNameDic.Add(anim.name, anim);
            animatorTypes.Add(new AnimatorType()
            {
                ePartType = EPartType.None,
                ePartName = EPartName.Arm,
                overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.AnimationsArm),
            });
            animatorTypes.Add(new AnimatorType()
            {
                ePartType = EPartType.Carry,
                ePartName = EPartName.Arm,
                overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.AnimationsArm_Hold),
            });
            holdItem = transform.GetChildComponent<SpriteRenderer>("HoldItem");

            ConfigEvent.PlayerHoldUpAnimations.AddEventListener<ItemDetails, bool>(OnItemSelectEvent);
            ConfigEvent.SceneBeforeUnload.AddEventListener(OnBeforeSceneUnloadEvent);
        }

        private void OnBeforeSceneUnloadEvent()
        {
            holdItem.enabled = false;
            SwitchAnimator(EPartType.None);
        }

        /// <summary>物品选中类型，播放对应动画</summary>
        /// <param name="itemDatails"></param><param name="idSelect">是否选中</param>
        private void OnItemSelectEvent(ItemDetails itemDatails, bool idSelect)
        {
            //WORKFLOW:不同的工具返回不同的动画，在这里补全
            EPartType currentType;
            EItemType itemType = (EItemType)itemDatails.itemType;
            switch (itemType)
            {
                case EItemType.Seed: currentType = EPartType.Carry; break;
                case EItemType.Commdity: currentType = EPartType.Carry; break;
                //TODO 不同的工具返回不同的动画，在这里补全
                case EItemType.Furniture:
                case EItemType.HoeTool:
                case EItemType.ChopTool:
                case EItemType.BreakTool:
                case EItemType.ReapTool:
                case EItemType.WaterTool:
                case EItemType.ClooectTool:
                case EItemType.ReapableSceney:
                default: currentType = EPartType.None; break;
            }
            //idSelect按钮是都被点选中
            if (idSelect == false)
            {
                currentType = EPartType.None;//如果没有选中物品的话，就切换普通动画状态
                holdItem.enabled = false;//是否启动头顶的物品
            }
            else
            {
                if (currentType == EPartType.Carry)
                {
                    holdItem.enabled = true;
                    holdItem.sprite = ResourceExtension.Load<Sprite>(itemDatails.itemOnWorldSprite);
                }
            }

            SwitchAnimator(currentType);
        }

        /// <summary>根据物体类型播放对应动画</summary>
        private void SwitchAnimator(EPartType ePartType)
        {
            foreach (var item in animatorTypes)
            {
                if (item.ePartType == ePartType)
                    animatorNameDic[item.ePartName.ToString()].runtimeAnimatorController = item.overrideController;
            }
        }
    }
}
