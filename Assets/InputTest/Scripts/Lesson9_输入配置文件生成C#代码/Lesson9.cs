using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson9 : MonoBehaviour
{
    Lesson9Input input;
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ ���������ļ�����C#����
        //1.ѡ��InputActions�ļ�
        //2.��Inspector������������·���������������ռ�
        //3.Ӧ�ú����ɴ���
        #endregion

        #region ֪ʶ��� ʹ��C#������м���
        //1.�������ɵĴ������
        input = new Lesson9Input();
        //2.��������
        input.Enable();
        //3.�¼�����
        input.Action1.Fire.performed += (context) =>
        {
            print("����");
        };

        input.Action2.Space.performed += (context) =>
        {
            print("��Ծ");
        };
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        print(input.Action1.Move.ReadValue<Vector2>());
    }
}
