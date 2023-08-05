using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player :MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator[] animators;
    private float inputX;
    private float inputY;
    private Vector2 movementInput;
    private bool isMoving;

    public float speed = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }

    private void Update()
    {
        PlayerInput();
    }


    private void PlayerInput()
    {
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
}
