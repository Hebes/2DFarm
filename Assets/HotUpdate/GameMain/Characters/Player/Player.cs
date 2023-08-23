using ACFrameworkCore;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;                 //��ҵ���ײ��
    private Animator[] animators;           //��ҵĶ������
    private float inputX;                   //����X
    private float inputY;                   //����Y
    private Vector2 movementInput;          //�ƶ�������
    private bool isMoving;                  //�Ƿ����ƶ�
    private bool InputDisable;              //��Ҳ��ܲ���
    public float speed = 10f;               //�ƶ��ٶ�
    private float mouseX;                   //ʹ�ù��ߵĶ���X
    private float mouseY;                   //ʹ�ù��ߵĶ���Y
    private bool UseTool;                  //�Ƿ�ʹ�ù���

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
        ConfigEvent.PlayerMoveToPosition.AddEventListener<Vector3>(OnMoveToPosition);
        ConfigEvent.SceneBeforeUnload.AddEventListener(OnBeforeSceneUnloadEvent);
        ConfigEvent.SceneAfterLoaded.AddEventListener(OnAfterSceneLoadedEvent);
        ConfigEvent.PlayerMouseClicked.AddEventListener<Vector3, ItemDetails>((pos, itemDetails) => { OnMouseClickedEvent(pos, itemDetails).Forget(); });
    }
    private void Update()
    {
        if (InputDisable == false)
            PlayerInput();
        else
            isMoving = false;
        SwitchAnimation();
    }
    private void FixedUpdate()
    {
        if (!InputDisable)
            Movement();
    }

    private void OnBeforeSceneUnloadEvent()
    {
        InputDisable = true;
    }
    private void OnAfterSceneLoadedEvent()
    {
        InputDisable = false;
    }
    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }
    private async UniTaskVoid OnMouseClickedEvent(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        switch ((EItemType)itemDetails.itemType)
        {
            case EItemType.Seed:
            case EItemType.Commdity:
            case EItemType.Furniture:
                ConfigEvent.ExecuteActionAfterAnimation.EventTrigger(mouseWorldPos, itemDetails);
                break;
            case EItemType.HoeTool:
            case EItemType.ChopTool:
            case EItemType.BreakTool:
            case EItemType.ReapTool:
            case EItemType.WaterTool:
            case EItemType.CollectTool:
            case EItemType.ReapableSceney:
                mouseX = mouseWorldPos.x - transform.position.x;
                mouseY = mouseWorldPos.y - transform.position.y;
                if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                    mouseY = 0;
                else
                    mouseX = 0;
                await UseToolRoutine(mouseWorldPos, itemDetails);
                break;
        }
        
    }

    private async UniTask UseToolRoutine(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        UseTool = true;
        InputDisable = true;
        await UniTask.Yield();
        foreach (var anim in animators)
        {
            anim.SetTrigger("useTool");
            //������泯����
            anim.SetFloat("InputX", mouseX);
            anim.SetFloat("InputY", mouseY);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(0.45f), ignoreTimeScale: false);
        ConfigEvent.ExecuteActionAfterAnimation.EventTrigger(mouseWorldPos, itemDetails);
        await UniTask.Delay(TimeSpan.FromSeconds(0.25f), ignoreTimeScale: false);

        //�ȴ���������
        UseTool = false;
        InputDisable = false;
    }


    private void PlayerInput()//�������
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
    private void Movement()//����ƶ�
    {
        rb.MovePosition(rb.position + (speed * Time.fixedDeltaTime * movementInput));
    }
    private void SwitchAnimation()//���Ŷ���
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
}
