﻿using Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = Core.Debug;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    鼠标界面

-----------------------*/

namespace Farm2D
{
    public class UICursorPanel : UIBase
    {
        private ItemDetailsData currentItem;        //当前鼠标指定的数据
        private Sprite currentSprite;           //存储当前鼠标图片

        private Image cursorImage;              //当前鼠标图片
        private Image buildImage;              //建造图片
        private Grid currentGrid;               //当前地板
        private Vector3 mouseWorldPos;          //鼠标是世界位置
        private Vector3Int mouseGridPos;        //鼠标在地图位置
        private bool cursorEnable;              //场景加载之前鼠标不可用
        private bool cursorPositionValid;       //鼠标是否可点按
        private string itemKey;                 //物品管理的Key

        private Camera mainCamera => ModelCommon.Instance.mainCamera;              //主摄像机


        //生命周期
        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Main, EUIMode.Normal, EUILucenyType.Pentrate);

            UIComponent UIComponent = panelGameObject.GetComponent<UIComponent>();
            GameObject T_CursorImage = UIComponent.Get<GameObject>("T_CursorImage");
            GameObject T_BuildImage = UIComponent.Get<GameObject>("T_BuildImage");

            cursorImage = T_CursorImage.GetImage();
            buildImage = T_BuildImage.GetImage();
            buildImage.SetActive(false);
            currentSprite = ChangeMouseType(EMouseType.Normal);
            SetCursorImage(ChangeMouseType(EMouseType.Normal));
            Cursor.visible = false;

            ConfigEvent.ItemSelectedEvent.EventAdd<string, int, bool>(OnItemSelectEvent);
            ConfigEvent.BeforeSceneUnloadEvent.EventAdd(OnBeforeSceneUnloadEvent);
            ConfigEvent.AfterSceneLoadedEvent.EventAdd(OnAfterSceneLoadedEvent);
            Debug.Log("鼠标界面");
        }
        public override void UIUpdate()
        {
            base.UIUpdate();
            cursorImage.transform.position = Input.mousePosition;
            if (!InteractwithUI() && cursorEnable)
            {
                SetCursorImage(currentSprite);
                CheckCursorValid();//检查鼠标是否有效
                CheckPlayerInput();//检查玩家输入
            }
            else
            {
                SetCursorImage(ChangeMouseType(EMouseType.Normal));
                buildImage.SetActive(false);
            }
        }

        /// <summary>
        /// 切换鼠标样式
        /// </summary>
        private Sprite ChangeMouseType(EMouseType mouseType)
        {
            switch (mouseType)
            {
                default:
                case EMouseType.Normal: return ConfigEvent.mouoseSelectedEvent.EventTriggerReturn<Sprite, string>(ConfigSprites.cursor11Png);
                case EMouseType.Tool: return ConfigEvent.mouoseSelectedEvent.EventTriggerReturn<Sprite, string>(ConfigSprites.cursor8Png);
                case EMouseType.Seed: return ConfigEvent.mouoseSelectedEvent.EventTriggerReturn<Sprite, string>(ConfigSprites.cursor7Png);
                case EMouseType.Item: return ConfigEvent.mouoseSelectedEvent.EventTriggerReturn<Sprite, string>(ConfigSprites.cursor3Png);
            }
        }


        //事件监听
        /// <summary>
        /// 设置鼠标对应的图片
        /// </summary>
        /// <param name="itemDatails"></param>
        /// <param name="isSelected">是否是选中状态</param>
        private void OnItemSelectEvent(string itemKey, int itemID, bool isSelected)
        {
            this.itemKey = itemKey;
            ItemDetailsData itemDatails = itemID.GetDataOne<ItemDetailsData>();
            if (!isSelected)//不是选中状态
            {
                currentItem = null;
                cursorEnable = false;
                currentSprite = ChangeMouseType(EMouseType.Normal);
                buildImage.SetActive(false);
                return;
            }

            cursorEnable = true;
            currentItem = itemDatails;
            switch ((EItemType)itemDatails.itemType)
            {
                default: currentSprite = ChangeMouseType(EMouseType.Normal); break;
                case EItemType.Seed: currentSprite = ChangeMouseType(EMouseType.Seed); break;
                case EItemType.Commdity: currentSprite = ChangeMouseType(EMouseType.Item); break;
                case EItemType.HoeTool:
                case EItemType.ChopTool:
                case EItemType.WaterTool:
                case EItemType.ReapTool:
                case EItemType.BreakTool:
                case EItemType.CollectTool:
                case EItemType.ReapableSceney: currentSprite = ChangeMouseType(EMouseType.Tool); break;
                case EItemType.Furniture://家具建造
                    buildImage.SetActive(true);
                    buildImage.sprite = LoadResExtension.LoadOrSub<Sprite>(itemDatails.itemOnWorldPackage, itemDatails.itemOnWorldSprite);
                    buildImage.SetNativeSize();
                    break;
            }
        }

        /// <summary>
        /// 卸载场景前需要做的事情
        /// </summary>
        private void OnBeforeSceneUnloadEvent()
        {
            cursorEnable = false;
        }

        /// <summary>
        /// 场景加载之后需要做的事
        /// </summary>
        private void OnAfterSceneLoadedEvent()
        {
            currentGrid = Object.FindObjectOfType<Grid>();
        }

        /// <summary>
        /// 检查鼠标是否有效
        /// </summary>
        private void CheckCursorValid()
        {
            mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));//屏幕转世界坐标
            mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);//WorldToCell 将世界位置转换为单元格位置。
            //ACDebug.Log("WorldPos:" + mouseWorldPos + "GridPos:" + mouseGridPos);
            //判断在使用范围内
            Vector3Int playerGridPos = currentGrid.WorldToCell(ModelCommon.Instance.playerTransform.position);
            //建造图片跟随移动
            buildImage.rectTransform.position = Input.mousePosition;
            if (Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.itemUseRadiue
                || Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.itemUseRadiue)
            {
                SetCursorInValid();
                return;
            }

            //能否扔东西
            TileDetails currentTile = ModelGridMap.Instance.GetTileDetailsOnMousePosition(mouseGridPos);
            if (currentTile != null)
            {
                CropDetails currentCrop = ModelCrop.Instance.GetCropDetails(currentTile.seedItemID);
                Crop crop = ModelGridMap.Instance.GetCropObject(mouseWorldPos);
                switch ((EItemType)currentItem.itemType)
                {
                    case EItemType.Seed:
                        if (currentTile.daysSinceDug > -1 && currentTile.seedItemID == -1) SetCursorValid(); else SetCursorInValid();
                        break;
                    case EItemType.Commdity:
                        if (currentTile.canDropItem && currentItem.canDropped) SetCursorValid(); else SetCursorInValid();
                        break;
                    case EItemType.Furniture://建造家具
                        buildImage.gameObject.SetActive(true);
                        BluePrintDetails bluePrintDetails = ModelBuild.Instance.GetDataOne(currentItem.itemID);

                        if (currentTile.canPlaceFurniture && ModelBuild.Instance.CheckStock(currentItem.itemID) && !HaveFurnitureInRadius(bluePrintDetails))
                            SetCursorValid();
                        else
                            SetCursorInValid();
                        break;
                    case EItemType.HoeTool:
                        if (currentTile.canDig) SetCursorValid(); else SetCursorInValid();
                        break;
                    case EItemType.ChopTool:
                    case EItemType.BreakTool:
                        if (crop != null)
                        {
                            if (crop.CanHarvest && crop.cropDetails.CheckToolAvailable(currentItem.itemID)) SetCursorValid(); else SetCursorInValid();
                        }
                        else
                        {
                            SetCursorInValid();
                        }
                        break;
                    case EItemType.ReapTool:
                        if (ModelGridMap.Instance.HaveReapableItemsInRadius(mouseWorldPos, currentItem)) SetCursorValid(); else SetCursorInValid();
                        break;
                    case EItemType.WaterTool:
                        if (currentTile.daysSinceDug > -1 && currentTile.daysSinceWatered == -1) SetCursorValid(); else SetCursorInValid();
                        break;
                    case EItemType.CollectTool:
                        if (currentCrop != null)
                        {
                            if (currentCrop.CheckToolAvailable(currentItem.itemID))
                                if (currentTile.growthDays >= currentCrop.TotalGrowthDays) SetCursorValid(); else SetCursorInValid();
                        }
                        else
                        {
                            SetCursorInValid();
                        }
                        break;
                    case EItemType.ReapableSceney:
                        break;
                    default:
                        Debug.Error("没有这个工具的使用方法,工具使用失败");
                        break;
                }
            }
            else
            {
                SetCursorInValid();
            }
        }



        //其他
        /// <summary>
        /// 检查玩家输入
        /// </summary>
        private void CheckPlayerInput()
        {
            //执行方法
            if (Input.GetMouseButtonDown(0) && cursorPositionValid)
                ConfigEvent.PlayerMouseClicked.EventTrigger(itemKey, mouseWorldPos, currentItem.itemID);
        }

        /// <summary>
        /// 是否与UI互动
        /// </summary>
        /// <returns></returns>
        private bool InteractwithUI()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return true;
            return false;
        }

        /// <summary>
        /// 设置鼠标图片
        /// </summary>
        /// <param name="sprite"></param>
        private void SetCursorImage(Sprite sprite)
        {
            cursorImage.sprite = sprite;
            cursorImage.color = new Color(1, 1, 1, 1);
        }

        /// <summary>
        /// 设置鼠标可用
        /// </summary>
        private void SetCursorValid()
        {
            cursorPositionValid = true;
            cursorImage.color = new Color(1, 1, 1, 1);
            buildImage.color = new Color(1, 1, 1, .5f);
        }

        /// <summary>
        /// 设置鼠标不可用
        /// </summary>
        private void SetCursorInValid()
        {
            cursorPositionValid = false;
            cursorImage.color = new Color(1, 0, 0, 0.4f);
            buildImage.color = new Color(1, 0, 0, .5f);
        }

        /// <summary>
        /// 家具是否在半径范围内
        /// </summary>
        /// <param name="bluePrintDetails"></param>
        /// <returns></returns>
        private bool HaveFurnitureInRadius(BluePrintDetails bluePrintDetails)
        {
            var buildItem = bluePrintDetails.buildPrefab;
            Vector2 point = mouseWorldPos;
            var size = buildItem.GetComponent<BoxCollider2D>().size;

            var otherColl = Physics2D.OverlapBox(point, size, 0);
            if (otherColl != null)
                return otherColl.GetComponent<Furniture>();
            return false;
        }
    }
}
