using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Lesson4 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ ��ȡ��ǰ�����豸
        Touchscreen ts = Touchscreen.current;
        //���ڴ�����ض������ƶ�ƽ̨���ṩ�������豸��ʹ��
        //������ʹ��ʱ�����һ���п�
        if (ts == null)
            return;
        #endregion

        #region ֪ʶ��� �õ�������ָ��Ϣ
        //�õ�������ָ����
        print(ts.touches.Count);
        //�õ�����������ָ
        //ts.touches[1]
        //�õ����д�����ָ
        foreach (var item in ts.touches)
        {
            
        }
        #endregion

        #region ֪ʶ���� ��ָ���� ̧�� ���� ���
        //��ȡָ��������ָ
        TouchControl tc = ts.touches[0];
        //���� ̧��
        if(tc.press.wasPressedThisFrame)
        {

        }
        if(tc.press.wasReleasedThisFrame)
        {

        }
        //����
        if(tc.press.isPressed)
        {

        }

        //�������
        if(tc.tap.isPressed)
        {

        }

        //�����������
        print(tc.tapCount);

        #endregion

        #region ֪ʶ���� ��ָλ�õ������Ϣ
        //λ��
        print(tc.position.ReadValue());
        //��һ�νӴ�ʱλ��
        print(tc.startPosition.ReadValue());
        //�Ӵ������С
        tc.radius.ReadValue();
        //ƫ��λ��
        tc.delta.ReadValue();

        //�õ���ǰ��ָ�� ״̬���׶Σ�
        UnityEngine.InputSystem.TouchPhase tp = tc.phase.ReadValue();
        switch (tp)
        {
            //��
            case UnityEngine.InputSystem.TouchPhase.None:
                break;
            //��ʼ�Ӵ�
            case UnityEngine.InputSystem.TouchPhase.Began:
                break;
            //�ƶ�
            case UnityEngine.InputSystem.TouchPhase.Moved:
                break;
            //����
            case UnityEngine.InputSystem.TouchPhase.Ended:
                break;
            //ȡ��
            case UnityEngine.InputSystem.TouchPhase.Canceled:
                break;
            //��ֹ
            case UnityEngine.InputSystem.TouchPhase.Stationary:
                break;
            default:
                break;
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
