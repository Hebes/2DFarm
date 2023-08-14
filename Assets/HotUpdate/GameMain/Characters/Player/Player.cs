using ACFrameworkCore;
using UnityEngine;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
        ConfigEvent.PlayerMoveToPosition.AddEventListener<Vector3>(OnMoveToPosition);
        ConfigEvent.SceneBeforeUnload.AddEventListener(OnBeforeSceneUnloadEvent);
        ConfigEvent.SceneAfterLoaded.AddEventListener(OnAfterSceneLoadedEvent);
        ConfigEvent.PlayerMouseClicked.AddEventListener<Vector3, ItemDetails>(OnMouseClickedEvent);
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
    private void OnMouseClickedEvent(Vector3 pos, ItemDetails itemDetails)
    {
        ConfigEvent.PlayerExecuteActionAfterAnimation.EventTrigger(pos, itemDetails);
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
            if (isMoving)
            {
                anim.SetFloat("InputX", inputX);
                anim.SetFloat("InputY", inputY);
            }
        }
    }
}
