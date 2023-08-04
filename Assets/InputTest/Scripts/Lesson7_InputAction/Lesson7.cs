using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson7 : MonoBehaviour
{
    [Header("Binding")]
    public InputAction move;
    [Header("1D Axis")]
    public InputAction axis;
    [Header("2D Vector")]
    public InputAction vector2D;
    [Header("3D Vector")]
    public InputAction vector3D;

    [Header("Button With One")]
    public InputAction btnOne;


    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ InputAction��ʲô��
        //����˼�壬InputAction��InputSystem�������Ƿ�װ�����붯����
        //������Ҫ���ã��ǲ���Ҫ����ͨ��д�������ʽ����������
        //����ֱ����Inspector���ڱ༭��Ҫ�������������
        //�����봥��ʱ������ֻ��Ҫ�Ѿ����������봥������߼�������

        //��������Ҫ���ڴ������붯�������� 
        //������Ӧ��InputAction���͵ĳ�Ա������ע�⣺��Ҫ���������ռ�UnityEngine.InputSystem��
        #endregion

        #region ֪ʶ��� InputAction�������

        #endregion

        #region ֪ʶ���� InputAction��ʹ��
        //1.����������
        move.Enable();

        //2.�����������
        //��ʼ����
        move.started += TestFun;

        //��������
        move.performed += (context) =>
        {
            print("�����¼�����");
            //��ǰ״̬
            //û������ Disabled
            //�ȴ� Waiting
            //��ʼ Started
            //���� Performed
            //���� Canceled
            //context.phase
            print(context.phase);

            //������Ϊ��Ϣ 
            print(context.action.name);

            //�ؼ�(�豸)��Ϣ
            print(context.control.name);

            //��ȡֵ
            //context.ReadValue<float>

            //����ʱ��
            print(context.duration);

            //��ʼʱ��
            print(context.startTime);
        };

        //��������
        move.canceled += (context) =>
        {
            print("�����¼�����");
        };

        //3.�ؼ����� CallbackContext
        //��ǰ״̬

        //������Ϊ��Ϣ 

        //�ؼ���Ϣ

        //��ȡֵ

        //����ʱ��

        //��ʼʱ��


        axis.Enable();
        vector2D.Enable();
        vector3D.Enable();

        btnOne.Enable();
        btnOne.performed += (context) =>
        {
            print("��ϼ�����");
        };

        #endregion

        #region ֪ʶ���� ������������
        //1.Input System �������ã�һЩĬ��ֵ���ã�
        //2.���������������
        #endregion
    }

    private void TestFun(InputAction.CallbackContext context)
    {
        print("��ʼ�¼�����");
    }

    // Update is called once per frame
    void Update()
    {
        //print(axis.ReadValue<float>());

        //rint(vector2D.ReadValue<Vector2>());

        print(vector3D.ReadValue<Vector3>());
    }
}
