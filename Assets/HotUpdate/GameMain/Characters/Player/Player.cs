using ACFrameworkCore;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;                 //玩家的碰撞体
    private Animator[] animators;           //玩家的动画组件
    private float inputX;                   //输入X
    private float inputY;                   //输入Y
    private Vector2 movementInput;          //移动的输入
    private bool isMoving;                  //是否在移动
    private bool InputDisable;              //玩家不能操作
    public float speed = 10f;               //移动速度

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
        ConfigEvent.ExecuteActionAfterAnimation.EventTrigger(pos, itemDetails);
    }

    private void PlayerInput()//玩家输入
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
    private void Movement()//玩家移动
    {
        rb.MovePosition(rb.position + (speed * Time.fixedDeltaTime * movementInput));
    }
    private void SwitchAnimation()//播放动画
    {
        foreach (var anim in animators)
        {
            anim.SetBool("IsMoving", isMoving);//注意IsMoving要和动画那边的一样
            if (isMoving)
            {
                anim.SetFloat("InputX", inputX);
                anim.SetFloat("InputY", inputY);
            }
        }
    }
}
