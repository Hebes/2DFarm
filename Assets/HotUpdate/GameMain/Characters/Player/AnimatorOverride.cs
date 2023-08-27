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
            //添加动画
            List<PlayerAnimators> playerAnimatorsList = this.GetDataListT<PlayerAnimators>();
            for (int i = 0; i < playerAnimatorsList.Count; i++)
            {
                AnimatorType animatorType=new AnimatorType();
                animatorType.ePartType = (EPartType)playerAnimatorsList[i].PartType;
                animatorType.ePartName = (EPartName)playerAnimatorsList[i].PartName;
                animatorType.overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(playerAnimatorsList[i].AnimatorName);
                animatorTypes.Add(animatorType);
            }

            ////基本动画
            ////基本手臂动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.None,
            //    ePartName = EPartName.Arm,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.ArmOverrideController),
            //});
            ////举东西手臂动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.Carry,
            //    ePartName = EPartName.Arm,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.Arm_HoldOverrideController),
            //});

            ////工具动画
            ////基本动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.None,
            //    ePartName = EPartName.Tool,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.ToolOverrideController),
            //});
            ////锄头动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.Hoe,
            //    ePartName = EPartName.Tool,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.Tool_HoeOverrideController),
            //});
            ////斧头动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.Chop,
            //    ePartName = EPartName.Tool,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.Tool_ChopOverrideController),
            //});
            ////十字镐动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.Break,
            //    ePartName = EPartName.Tool,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.Tool_BreakOverrideController),
            //});

            ////采集动画
            ////手臂动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.Collect,
            //    ePartName = EPartName.Arm,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.Arm_CollectOverrideController),
            //});
            ////身体动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.Collect,
            //    ePartName = EPartName.Body,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.Body_CollectOverrideController),
            //});
            ////头部动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.Collect,
            //    ePartName = EPartName.Hair,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.Hair_CollectOverrideController),
            //});

            ////浇水动画
            ////手臂动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.Water,
            //    ePartName = EPartName.Arm,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.Arm_WaterOverrideController),
            //});
            ////身体动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.Water,
            //    ePartName = EPartName.Body,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.Body_WaterOverrideController),
            //});
            ////头部动画
            //animatorTypes.Add(new AnimatorType()
            //{
            //    ePartType = EPartType.Water,
            //    ePartName = EPartName.Hair,
            //    overrideController = await ResourceExtension.LoadAsyncUniTask<AnimatorOverrideController>(ConfigAnimations.Hair_WaterOverrideController),
            //});

            holdItem = transform.GetChildComponent<SpriteRenderer>(childName: "HoldItem");

            ConfigEvent.PlayerHoldUpAnimations.AddEventListener<ItemDetails, bool>(OnItemSelectEvent);
            ConfigEvent.SceneBeforeUnload.AddEventListener(OnBeforeSceneUnloadEvent);
        }

        private void OnBeforeSceneUnloadEvent()
        {
            holdItem.enabled = false;
            SwitchAnimator(EPartType.None);
        }

        /// <summary>物品选中类型播放对应动画</summary>
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
                case EItemType.HoeTool: currentType = EPartType.Hoe; break;
                //TODO 不同的工具返回不同的动画，在这里补全
                case EItemType.Furniture: currentType = EPartType.Hoe; break;
                case EItemType.ChopTool: currentType = EPartType.Chop; break;
                case EItemType.BreakTool: currentType = EPartType.Break; break;
                case EItemType.ReapTool: currentType = EPartType.Reap; break; 
                case EItemType.WaterTool: currentType = EPartType.Water; break;
                case EItemType.CollectTool: currentType = EPartType.Collect; break;
                case EItemType.ReapableSceney:
                default: currentType = EPartType.None; break;
            }
            //idSelect按钮是被点选中
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
                else
                {
                    holdItem.enabled = false;
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
                else if (item.ePartType == EPartType.None)
                    animatorNameDic[item.ePartName.ToString()].runtimeAnimatorController = item.overrideController;
            }
        }
    }
}
