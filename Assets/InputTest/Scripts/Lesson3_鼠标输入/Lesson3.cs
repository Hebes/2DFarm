using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ ��ȡ��ǰ����豸����Ҫ���������ռ䣩
        Mouse mouse = Mouse.current;
        #endregion

        #region ֪ʶ��� ������λ ���� ̧�� ����
        //������
        //mouse.leftButton
        //����Ҽ�
        //mouse.rightButton
        //����м�
        //mouse.middleButton
        //��� ��ǰ����
        //mouse.forwardButton;
        //mouse.backButton;

        //����
        if(mouse.leftButton.wasPressedThisFrame)
        {

        }
        //̧��
        if(mouse.leftButton.wasReleasedThisFrame)
        {

        }
        //����
        if(mouse.rightButton.isPressed)
        {

        }

        #endregion

        #region ֪ʶ���� ���λ�����
        //��ȡ��ǰ���λ��
        mouse.position.ReadValue();
        //�õ������֮֡���һ��ƫ������ 
        mouse.delta.ReadValue();

        //����м� ���ֵķ�������
        mouse.scroll.ReadValue();
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            print("����������");
        }

        //̧��
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            print("������̧��");
        }
        //����
        if (Mouse.current.rightButton.isPressed)
        {
            print("����Ҽ�����");
        }

        //print(Mouse.current.position.ReadValue());

        //print(Mouse.current.delta.ReadValue());

        print(Mouse.current.scroll.ReadValue());
    }
}
