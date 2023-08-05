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
    private bool InputDisable;              //��Ҳ��ܲ���

    public float speed = 10f;               //�ƶ��ٶ�

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
            if (isMoving)
            {
                anim.SetFloat("InputX", inputX);
                anim.SetFloat("InputY", inputY);
            }
        }
    }
}
