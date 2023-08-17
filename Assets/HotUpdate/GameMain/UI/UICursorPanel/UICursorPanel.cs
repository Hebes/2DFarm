using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ACFrameworkCore
{
    public class UICursorPanel : UIBase
    {
        public GameObject T_CursorImage;

        public Sprite normal;                   //默认图标
        public Sprite tool;                     //工具图标
        public Sprite seed;                     //种子
        public Sprite item;                     //物品图标
        private ItemDetails currentItem;        //当前鼠标图标
        private Sprite currentSprite;           //存储当前鼠标图片
        private Image cursorImage;              //当前鼠标图片
        private RectTransform cursorCanvas;     //当前渲染
        private Grid currentGrid;               //当前地板
        private Vector3 mouseWorldPos;          //鼠标是世界位置
        private Vector3Int mouseGridPos;        //鼠标在地图位置
        private bool cursorEnable;              //场景加载之前鼠标不可用
        private bool cursorPositionValid;       //鼠标是否可点按

        private Camera mainCamera => Camera.main;                                           //主摄像机
        private Transform playerTransform => Object.FindObjectOfType<Player>().transform;   //玩家


        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fade, EUIMode.Normal, EUILucenyType.Pentrate);
            //获取组件
            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            T_CursorImage = UIComponent.Get<GameObject>("T_CursorImage");
            //加载UI图标
            normal = ResourceExtension.Load<Sprite>(ConfigSprites.Spritescursor11);
            tool = ResourceExtension.Load<Sprite>(ConfigSprites.Spritescursor8);
            seed = ResourceExtension.Load<Sprite>(ConfigSprites.Spritescursor7);
            item = ResourceExtension.Load<Sprite>(ConfigSprites.Spritescursor3);

            //cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
            cursorImage = T_CursorImage.GetComponent<Image>();
            currentSprite = normal;
            SetCursorImage(normal);
        }
        public override void UIOnEnable()
        {
            base.UIOnEnable();
            ConfigEvent.CursorItemSelect.AddEventListener<ItemDetails, bool>(OnItemSelectEvent);
            ConfigEvent.SceneBeforeUnload.AddEventListener(OnBeforeSceneUnloadEvent);
            ConfigEvent.SceneAfterLoaded.AddEventListener(OnAfterSceneLoadedEvent);
        }
        public override void UIOnDisable()
        {
            base.UIOnDisable();
            ConfigEvent.CursorItemSelect.RemoveEventListener<ItemDetails, bool>(OnItemSelectEvent);
            ConfigEvent.SceneBeforeUnload.RemoveEventListener(OnBeforeSceneUnloadEvent);
            ConfigEvent.SceneAfterLoaded.RemoveEventListener(OnAfterSceneLoadedEvent);
        }

        public override void UIUpdate()
        {
            base.UIUpdate();
            //if (cursorCanvas == null) return;
            cursorImage.transform.position = Input.mousePosition;
            ACDebug.Log($"当前bool:{!InteractwithUI()}{cursorEnable}");
            if (!InteractwithUI() && cursorEnable)
            {
                SetCursorImage(currentSprite);
                CheckCursorValid();//检查鼠标是否有效
                //CheckPlayerInput();//检查玩家输入
            }
            else
            {
                SetCursorImage(normal);
            }
        }

        //检查玩家输入
        private void CheckPlayerInput()
        {
            //执行方法
            if (Input.GetMouseButtonDown(0) && cursorPositionValid)
                ConfigEvent.CursorClicked.EventTrigger(mouseWorldPos, currentItem);
        }

        /// <summary>
        /// 设置鼠标对应的图片
        /// </summary>
        /// <param name="itemDatails"></param>
        /// <param name="isSelected"></param>
        private void OnItemSelectEvent(ItemDetails itemDatails, bool isSelected)
        {
            if (!isSelected)//如果不是选中的话
            {
                currentSprite = normal;
            }
            else
            {
                switch ((EItemType)itemDatails.itemType)
                {
                    case EItemType.Seed:
                        currentSprite = seed;
                        break;
                    case EItemType.Commdity:
                        currentSprite = item;
                        break;
                    case EItemType.Furniture:
                    case EItemType.HoeTool:
                    case EItemType.ChopTool:
                    case EItemType.WaterTool:
                    case EItemType.ReapTool:
                    case EItemType.BreakTool:
                    case EItemType.ClooectTool:
                    case EItemType.ReapableSceney:
                        currentSprite = tool;
                        break;
                    default:
                        currentSprite = normal;
                        break;
                }
            }

            if (!isSelected)
            {
                currentItem = null;
                cursorEnable = false;
                currentSprite = normal;
            }
            else
            {
                cursorEnable = true;
                currentItem = itemDatails;
            }

        }

        private void OnAfterSceneLoadedEvent() 
        {
            currentGrid = Object.FindObjectOfType<Grid>();
            cursorEnable = true;
        }
        private void OnBeforeSceneUnloadEvent()
        {
            cursorEnable = false;
        }

        private void SetCursorImage(Sprite sprite)
        {
            cursorImage.sprite = sprite;
            cursorImage.color = new Color(1, 1, 1, 1);
        }//设置鼠标图片
        private void SetCursorValid()
        {
            cursorPositionValid = true;
            cursorImage.color = new Color(1, 1, 1, 1);
        }//设置鼠标可用
        private void SetCursorInValid()
        {
            cursorPositionValid = false;
            cursorImage.color = new Color(1, 0, 0, 0.4f);
        }//设置鼠标不可用


        /// <summary>
        /// 是否与UI互动
        /// </summary>
        /// <returns></returns>
        private bool InteractwithUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        /// <summary>
        /// 检查鼠标是否有效
        /// </summary>
        private void CheckCursorValid()
        {
            mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));//屏幕转世界坐标
            mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);//WorldToCell 将世界位置转换为单元格位置。
            Debug.Log("WorldPos:" + mouseWorldPos + "GridPos:" + mouseGridPos);

            //判断在使用范围内
            //var playerGridPos = currentGrid.WorldToCell(playerTransform.position);
            //if (Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.itemUseRadiue
            //    || Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.itemUseRadiue)
            //{
            //    SetCursorInValid();
            //    return;
            //}

            ////能否扔东西
            //TileDetails currentTile = GridMapSystem.Instance.GetTileDetailsOnMousePosition(mouseGridPos);
            //if (currentTile != null)
            //{
            //    switch ((EItemType)currentItem.itemType)
            //    {
            //        case EItemType.Commdity://商品的话
            //            if (currentTile.canDropItem && currentItem.canDropped) SetCursorValid(); else SetCursorInValid();
            //            break;
            //        case EItemType.HoeTool:
            //            if (currentTile.canDig) SetCursorValid(); else SetCursorInValid();
            //            break;
            //        case EItemType.WaterTool:
            //            if (currentTile.daysSinceDug > -1 && currentTile.daysSinceWatered == -1) SetCursorValid(); else SetCursorInValid();
            //            break;
            //    }
            //}
            //else
            //{
            //    SetCursorInValid();
            //}
        }
    }
}
