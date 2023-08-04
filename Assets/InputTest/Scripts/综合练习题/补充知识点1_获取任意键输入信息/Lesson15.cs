using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Lesson15 : MonoBehaviour
{
    public InputAction input;
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ �ع�ѧ���Ļ�ȡ���������ķ���
        //1.�������������
        //input.Enable();
        //input.performed += (context) =>
        //{
        //    print("123");
        //    print(context.control.name);
        //    print(context.control.path);
        //};
        ////2.��������������ַ�
        //Keyboard.current.onTextInput += (c) =>
        //{
        //    print(c);
        //};
        #endregion

        #region ֪ʶ��� InputSystem��ר��������������µķ���
        //�����Call �����̻ᱨ�� ����Ҳ������ִ��
        //��CallOnce ֻ��ִ��һ�� ���ǲ��ᱨ��
        InputSystem.onAnyButtonPress.CallOnce((control) =>
        {
            print(control.path);
            print(control.name);
        });
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
