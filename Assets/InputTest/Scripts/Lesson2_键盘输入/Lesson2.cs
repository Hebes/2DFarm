using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ ��ȡ��ǰ�����豸(��Ҫ���������ռ�)
        //������ϵͳ �ṩ�˶�Ӧ�������豸�� �������Ƕ�ĳһ���豸������м��
        Keyboard keyBoard = Keyboard.current;
        #endregion

        #region ֪ʶ��� �������� ����̧�𳤰�
        //����Ҫ�õ�ĳһ������ ͨ����������� ��� ���ְ��� ����ȡ
        //keyBoard.aKey
        //����
        if(keyBoard.enterKey.wasPressedThisFrame)
        {

        }
        //̧��
        if(keyBoard.dKey.wasReleasedThisFrame)
        {

        }

        //����
        if(keyBoard.spaceKey.isPressed)
        {

        }

        #endregion

        #region ֪ʶ���� ͨ���¼�������������
        //ͨ����keyboard�����е� �ı������¼� ���ί�к���
        //����Ի��ÿ�����������
        keyBoard.onTextInput += (c) =>
        {
            print("ͨ��lambda���ʽ" + c);
        };
        keyBoard.onTextInput += TextInput;
        keyBoard.onTextInput -= TextInput;
        #endregion

        #region ֪ʶ���� ��������¼���
        //���Դ��� ����� ���� ̧�� ���� ��ص��߼�
        //keyBoard.anyKey
        //wasPressedThisFrame
        //wasReleasedThisFrame
        //isPressed
        #endregion
    }

    private void TextInput(char c)
    {
        print("ͨ�����������¼�����" + c);
    }

    // Update is called once per frame
    void Update()
    {
        //�ո�� ��ǰ֡ �Ƿ���
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            print("�ո������");
        }

        //�ж�D���Ƿ��ͷ�
        if (Keyboard.current.dKey.wasReleasedThisFrame)
        {
            print("D��̧��");
        }

        //�жϿո��Ƿ�һֱ���ڰ���״̬
        if (Keyboard.current.spaceKey.isPressed)
        {
            print("�ո���״̬");
        }


        //�����������
        if(Keyboard.current.anyKey.wasPressedThisFrame)
        {
            print("���������");
        }
    }
}
