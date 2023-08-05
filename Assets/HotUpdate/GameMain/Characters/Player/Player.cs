using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator[] animators;
    private float inputX;
    private float inputY;
    private Vector2 movementInput;
    private bool isMoving;
    private bool InputDisable;              //玩家不能操作

    public float speed = 10f;               //移动速度

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }
    private void Update()
    {
        if (!InputDisable)
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
            if (isMoving)
            {
                anim.SetFloat("InputX", inputX);
                anim.SetFloat("InputY", inputY);
            }
        }
    }
}
