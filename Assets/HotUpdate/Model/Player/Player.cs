using ACFrameworkCore;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    玩家控制脚本

-----------------------*/

namespace ACFarm
{
    public class Player : MonoBehaviour, ISaveable
    {
        private Rigidbody2D rb;                 //玩家的碰撞体
        private Animator[] animators;           //玩家的动画组件
        private float inputX;                   //输入X
        private float inputY;                   //输入Y
        private Vector2 movementInput;          //移动的输入
        private bool isMoving;                  //是否在移动
        private bool inputDisable;              //玩家不能操作
        public float speed = 10f;               //移动速度
        private float mouseX;                   //使用工具的动画X
        private float mouseY;                   //使用工具的动画Y
        private bool UseTool;                   //是否使用工具


        //生命周期
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animators = GetComponentsInChildren<Animator>();
            ConfigEvent.BeforeSceneUnloadEvent.AddEventListener(OnBeforeSceneUnloadEvent);
            ConfigEvent.AfterSceneLoadedEvent.AddEventListener(OnAfterSceneLoadedEvent);
            ConfigEvent.PlayerMoveToPosition.AddEventListener<Vector3>(OnMoveToPosition);
            ConfigEvent.PlayerMouseClicked.AddEventListener<string, Vector3, int>(OnMouseClickedEvent);
            ConfigEvent.UpdateGameStateEvent.AddEventListener<EGameState>(OnUpdateGameStateEvent);
            ConfigEvent.StartNewGameEvent.AddEventListener<int>(OnStartNewGameEvent);
            ConfigEvent.EndGameEvent.AddEventListener(OnEndGameEvent);
            //注册保存事件
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }
        private void Update()
        {
            if (!inputDisable)
                PlayerInput();
            else
                isMoving = false;
            SwitchAnimation();
        }
        private void FixedUpdate()
        {
            if (!inputDisable)
                Movement();
        }



        //事件监听
        private void OnBeforeSceneUnloadEvent()
        {
            inputDisable = true;
        }
        private void OnAfterSceneLoadedEvent()
        {
            inputDisable = false;
        }
        private void OnMoveToPosition(Vector3 targetPosition)
        {
            if (targetPosition == Vector3.zero) return;
            transform.position = targetPosition;
        }
        private void OnMouseClickedEvent(string itemKey, Vector3 mouseWorldPos, int itemID)
        {
            ItemDetailsData itemDetails = itemID.GetDataOne<ItemDetailsData>();
            if (UseTool) return;
            switch ((EItemType)itemDetails.itemType)
            {
                case EItemType.Seed:
                case EItemType.Commdity:
                case EItemType.Furniture:
                    ConfigEvent.ExecuteActionAfterAnimation.EventTrigger(itemKey, itemDetails.itemID, mouseWorldPos );
                    break;
                case EItemType.HoeTool:
                case EItemType.ChopTool:
                case EItemType.BreakTool:
                case EItemType.ReapTool:
                case EItemType.WaterTool:
                case EItemType.CollectTool:
                case EItemType.ReapableSceney:
                    mouseX = mouseWorldPos.x - transform.position.x;
                    mouseY = mouseWorldPos.y - (transform.position.y + 0.85f);
                    if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                        mouseY = 0;
                    else
                        mouseX = 0;
                    UseToolRoutine(itemKey, mouseWorldPos, itemDetails).Forget();
                    break;
            }
        }
        private void OnUpdateGameStateEvent(EGameState gameState)
        {
            //玩家移动
            switch (gameState)
            {
                case EGameState.Gameplay:
                    inputDisable = false;
                    break;
                case EGameState.Pause:
                    inputDisable = true;
                    break;
            }
        }
        private void OnStartNewGameEvent(int obj)
        {
            inputDisable = false;
            transform.position = ConfigSettings.playerStartPos;
        }
        private void OnEndGameEvent()
        {
            inputDisable = true;
        }



        /// <summary>
        /// 使用工具的动作
        /// </summary>
        /// <param name="mouseWorldPos"></param>
        /// <param name="itemDetails"></param>
        /// <returns></returns>
        private async UniTask UseToolRoutine(string itemKey, Vector3 mouseWorldPos, ItemDetailsData itemDetails)
        {
            UseTool = true;
            inputDisable = true;
            await UniTask.Yield();
            foreach (var anim in animators)
            {
                anim.SetTrigger("useTool");
                //人物的面朝方向
                anim.SetFloat("InputX", mouseX);
                anim.SetFloat("InputY", mouseY);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.45f), ignoreTimeScale: false);
            ConfigEvent.ExecuteActionAfterAnimation.EventTrigger(itemKey, itemDetails.itemID, mouseWorldPos);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), ignoreTimeScale: false);

            //等待动画结束
            UseTool = false;
            inputDisable = false;
        }
        /// <summary>
        /// 玩家输入
        /// </summary>
        private void PlayerInput()
        {
            //用于只能横着走或者竖着走
            //if (inputY == 0)
            //    inputX = Input.GetAxisRaw("Horizontal");
            //if (inputX == 0)
            //    inputY = Input.GetAxisRaw("Vertical");

            //可以斜射走
            inputX = Input.GetAxisRaw("Horizontal");
            inputY = Input.GetAxisRaw("Vertical");

            if (inputX != 0 && inputY != 0)//防止左上等移动的时候超过1
            {
                inputX = inputX * 0.6f;
                inputY = inputY * 0.6f;
            }

            //走路速度下降
            if (Input.GetKey(KeyCode.LeftShift))
            {
                inputX = inputX * 0.5f;
                inputY = inputY * 0.5f;
            }

            movementInput = new Vector2(inputX, inputY);

            isMoving = movementInput != Vector2.zero;//判断是否在移动
        }
        /// <summary>
        /// 玩家移动
        /// </summary>
        private void Movement()
        {
            rb.MovePosition(rb.position + (speed * Time.fixedDeltaTime * movementInput));
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        private void SwitchAnimation()
        {
            foreach (var anim in animators)
            {
                anim.SetBool("IsMoving", isMoving);//注意IsMoving要和动画那边的一样
                anim.SetFloat("MouseX", mouseX);
                anim.SetFloat("MouseY", mouseY);
                if (isMoving)
                {
                    anim.SetFloat("InputX", inputX);
                    anim.SetFloat("InputY", inputY);
                }
            }
        }



        //保存数据
        private string SaveKey = "Player数据保存";        //保存的Key,一般都是名字
        public string GUID => SaveKey;
        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.characterPosDict = new Dictionary<string, SerializableVector3>();
            saveData.characterPosDict.Add(this.name, new SerializableVector3(transform.position));
            return saveData;
        }
        public void RestoreData(GameSaveData saveData)
        {
            var targetPosition = saveData.characterPosDict[this.name].ToVector3();
            transform.position = targetPosition;
        }
    }
}
