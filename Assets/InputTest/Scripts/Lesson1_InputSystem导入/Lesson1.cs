using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ InputSystem������
        //packageManager�е���Input System
        #endregion

        #region ֪ʶ��� ѡ������һ������ģʽ
        //ѡ��InputSystem�� ��InputManager���������
        //  File����>Build Setting����>Player Setting����>Other����>Active Input Handling
        //  ����ͬʱ����Ҳ����ֻ��������֮һ,ÿ�����ú������Unity
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            print("123");
        }
    }
}
