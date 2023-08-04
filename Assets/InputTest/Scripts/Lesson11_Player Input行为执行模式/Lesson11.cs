using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson11 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ Send Messages
        //���Զ���ű���
        //������Ϊ "On+��Ϊ��" �ĺ���
        //û�в��� ���� ��������ΪInputValue
        //�����Զ���ű����ص�PlayerInput�����Ķ�����
        //��������Ӧ����ʱ ���Զ����ú���

        //���һ���Ĭ�ϵ�3�����豸��صĺ������Ե���
        //�豸ע��(�����������豸��ʧ�лָ����ٴ�����ʱ�ᴥ��)��OnDeviceRegained(PlayerInput input)
        //�豸��ʧ�����ʧȥ�˷���������豸֮һ�����磬�������豸�ľ����ʱ����OnDeviceLost(PlayerInput input)
        //�������л���OnControlsChanged(PlayerInput input)
        #endregion

        #region ֪ʶ��� Broadcast Messages	
        //������SendMessage����һ��
        //Ψһ�������ǣ��Զ���ű��������Թ�����PlayerInput�����Ķ�����
        //�����Թ��������Ӷ�����
        #endregion

        #region ֪ʶ���� Invoke Unity Events
        //��ģʽ������������Inspector������ͨ����ק����ʽ������Ӧ����
        //����ע�⣺��Ӧ�����Ĳ������� ��Ҫ��Ϊ InputAction.CallbackContext
        #endregion

        #region ֪ʶ���� Invoke C Sharp Events
        //1.��ȡPlayerInput���
        PlayerInput input = this.GetComponent<PlayerInput>();
        //2.��ȡ��Ӧ�¼�����ί�к������
        input.onDeviceLost += OnDeviceLost;
        input.onDeviceRegained += OnDeviceRegained;
        input.onControlsChanged += OnControlsChanged;
        input.onActionTriggered += OnActionTrigger;

        //input.currentActionMap["Move"].ReadValue<Vector2>()
        //3.����������ʱ���Զ������¼����ö�Ӧ����
        #endregion

        #region ֪ʶ���� �ؼ�����InputValue��InputAction.CallbackContext
        //InputValue
        //�Ƿ���

        //�õ����巵��ֵ
        //value.Get<>
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Send Message / Broadcast Messages	
    public void OnMove(InputValue value)
    {
        print("Move");
        print(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        print("Look");
        print(value.Get<Vector2>());
    }
    public void OnFire(InputValue value)
    {
        print("Fire");
        if(value.isPressed)
        {
            print("����");
        }
    }

    public void OnDeviceLost( PlayerInput input)
    {
        print("�豸��ʧ");
    }

    public void OnDeviceRegained(PlayerInput input)
    {
        print("�豸ע��");
    }

    public void OnControlsChanged(PlayerInput input)
    {
        print("�������л�");
    }
    #endregion

    #region Invoke Unity Events
    public void MyFire(InputAction.CallbackContext context)
    {
        print("����1");
    }

    public void MyMove(InputAction.CallbackContext context)
    {
        print("�ƶ�1");
    }

    public void MyLook(InputAction.CallbackContext context)
    {
        print("Look1");
    }

    #endregion

    public void OnActionTrigger(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Fire":
                //����׶ε��ж� �����׶� ��ȥ���߼�
                if(context.phase == InputActionPhase.Performed)
                    print("����");
                break;
            case "Look":
                print("����");
                print(context.ReadValue<Vector2>());
                break;
            case "Move":
                print("�ƶ�");
                print(context.ReadValue<Vector2>());
                break;
        }
    }
}
