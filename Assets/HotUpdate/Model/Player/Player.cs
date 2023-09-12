using ACFrameworkCore;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

/*--------�ű�����-----------

�������䣺
    1607388033@qq.com
����:
    ����
����:
    ��ҿ��ƽű�

-----------------------*/

namespace ACFarm
{
    public class Player : MonoBehaviour, ISaveable
    {
        private Rigidbody2D rb;                 //��ҵ���ײ��
        private Animator[] animators;           //��ҵĶ������
        private float inputX;                   //����X
        private float inputY;                   //����Y
        private Vector2 movementInput;          //�ƶ�������
        private bool isMoving;                  //�Ƿ����ƶ�
        private bool inputDisable;              //��Ҳ��ܲ���
        public float speed = 10f;               //�ƶ��ٶ�
        private float mouseX;                   //ʹ�ù��ߵĶ���X
        private float mouseY;                   //ʹ�ù��ߵĶ���Y
        private bool UseTool;                   //�Ƿ�ʹ�ù���


        //��������
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
            //ע�ᱣ���¼�
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



        //�¼�����
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
            //����ƶ�
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
        /// ʹ�ù��ߵĶ���
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
                //������泯����
                anim.SetFloat("InputX", mouseX);
                anim.SetFloat("InputY", mouseY);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.45f), ignoreTimeScale: false);
            ConfigEvent.ExecuteActionAfterAnimation.EventTrigger(itemKey, itemDetails.itemID, mouseWorldPos);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), ignoreTimeScale: false);

            //�ȴ���������
            UseTool = false;
            inputDisable = false;
        }
        /// <summary>
        /// �������
        /// </summary>
        private void PlayerInput()
        {
            //����ֻ�ܺ����߻���������
            //if (inputY == 0)
            //    inputX = Input.GetAxisRaw("Horizontal");
            //if (inputX == 0)
            //    inputY = Input.GetAxisRaw("Vertical");

            //����б����
            inputX = Input.GetAxisRaw("Horizontal");
            inputY = Input.GetAxisRaw("Vertical");

            if (inputX != 0 && inputY != 0)//��ֹ���ϵ��ƶ���ʱ�򳬹�1
            {
                inputX = inputX * 0.6f;
                inputY = inputY * 0.6f;
            }

            //��·�ٶ��½�
            if (Input.GetKey(KeyCode.LeftShift))
            {
                inputX = inputX * 0.5f;
                inputY = inputY * 0.5f;
            }

            movementInput = new Vector2(inputX, inputY);

            isMoving = movementInput != Vector2.zero;//�ж��Ƿ����ƶ�
        }
        /// <summary>
        /// ����ƶ�
        /// </summary>
        private void Movement()
        {
            rb.MovePosition(rb.position + (speed * Time.fixedDeltaTime * movementInput));
        }
        /// <summary>
        /// ���Ŷ���
        /// </summary>
        private void SwitchAnimation()
        {
            foreach (var anim in animators)
            {
                anim.SetBool("IsMoving", isMoving);//ע��IsMovingҪ�Ͷ����Ǳߵ�һ��
                anim.SetFloat("MouseX", mouseX);
                anim.SetFloat("MouseY", mouseY);
                if (isMoving)
                {
                    anim.SetFloat("InputX", inputX);
                    anim.SetFloat("InputY", inputY);
                }
            }
        }



        //��������
        private string SaveKey = "Player���ݱ���";        //�����Key,һ�㶼������
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
