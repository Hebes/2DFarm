using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson13 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ InputSystem��UI��֧��
        //1.������ϵͳInputSystem��֧��IMGUI��GUI��ע�⣺�༭�����벻��Ӱ��
        //  �����ǰ�������InputSystem����ôOnGUI�е������ж�������ݲ��ᱻ����
        //  �����Ҫѡ��Both����ֻ����������ϵͳInputManager������OnGUI����������

        //2.������ϵͳ֧��UGUI��������Ҫʹ��������ϵͳ����ģ�飨Input System UI Input Module��
        #endregion

        #region ֪ʶ��� UGUI�е�������ϵͳ����ģ��������

        #endregion

        #region ֪ʶ���� VR�����ʹ��������ϵͳע������
        //�������VR��Ŀ��ʹ��������ϵͳ���UGUIʹ��
        //��Ҫ��Canvas���������Tracked Device Raycaster���
        #endregion

        #region ֪ʶ���� ������Ϸʹ�ö���UI
        //���ͬһ�豸�ϵĶ�����Ϸ��ÿ������Ҫʹ���Լ���һ�׶���UI
        //��Ҫ��EventSystem�е�EventSystem����滻ΪMultiplayer Event System���

        //��EventSystem�����ͬ�������ڳ�����ͬʱ������MultiplayerEventSystem��
        //�������������ж����ң�ÿ����Ҷ����Լ���InputSystemUIInputModule��MultiplayerEventSystem���
        //ÿ����Ҷ��������Լ���һ������������Լ���UIʵ����
        //���������ʹ��PlayerInput���������������PlayerInput���Զ�������ҵ�InputSystemUIInputModule��ʹ����ҵĲ���
        //MultilayerEventSystem������������¼�ϵͳ�е�������ͬ
        //���⣬MultiplayerEventSystem����������һ��playerRoot���ԣ������Խ�������Ϊһ����Ϸ����
        //����Ϸ����������¼�ϵͳӦ�����νṹ�д��������UI��ѡ����
        #endregion

        #region ֪ʶ���� On-Screen������
        //On-Screen�������ģ��UI���û������Ľ���
        //1.On-Screen Button����ť����
        //2.On-Screen Stick��ҡ�˽���
        #endregion

        #region ֪ʶ���� ��������
        //�ɲ鿴�ٷ��ĵ��˽����������ϵͳ��UI���ʹ�õ��������
        //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.2/manual/UISupport.html
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
