using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson5 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ ��ȡ��ǰ�ֱ�
        Gamepad gamePad = Gamepad.current;
        if (gamePad == null)
            return;
        #endregion

        #region ֪ʶ��� �ֱ�ҡ��
        //ҡ�˷���
        //��ҡ��
        print(gamePad.leftStick.ReadValue());
        //��ҡ��
        print(gamePad.rightStick.ReadValue());

        //ҡ�˰���
        //��ҡ�� ����̧�𳤰����
        //gamePad.rightStickButton
        //��ҡ��
        if(gamePad.leftStickButton.wasPressedThisFrame)
        {

        }
        if(gamePad.leftStickButton.wasReleasedThisFrame)
        {

        }
        if(gamePad.leftStickButton.isPressed)
        {

        }
        #endregion

        #region ֪ʶ���� �ֱ������
        //��Ӧ�ֱ���4������� ��������
        //gamePad.dpad.left
        //gamePad.dpad.right
        //gamePad.dpad.up
        //gamePad.dpad.down
        if(gamePad.dpad.left.wasPressedThisFrame)
        {

        }
        if(gamePad.dpad.left.wasReleasedThisFrame)
        {

        }
        if(gamePad.dpad.left.isPressed)
        {

        }
        #endregion

        #region ֪ʶ���� �ֱ��Ҳఴ��
        //ͨ��
        //Y����
        //gamePad.buttonNorth
        //A��X
        //gamePad.buttonSouth
        //X����
        //gamePad.buttonWest
        //B����
        //gamePad.buttonEast

        //wasPressedThisFrame
        //wasReleasedThisFrame
        //isPressed

        //�ֱ��Ҳఴť x �� �� �� A B Y 
        //��
        //gamePad.circleButton
        //��
        //gamePad.triangleButton
        //��
        //gamePad.squareButton
        //X
        //gamePad.crossButton
        //x
        //gamePad.xButton
        //a
        //gamePad.aButton
        //b
        //gamePad.bButton
        //Y
        //gamePad.yButton
        #endregion

        #region ֪ʶ���� �ֱ����밴��
        //�����
        //gamePad.startButton
        //gamePad.selectButton

        //wasPressedThisFrame
        //wasReleasedThisFrame
        //isPressed

        #endregion

        #region ֪ʶ���� �ֱ��粿����
        //�������� �粿��λ
        //����ǰ���粿��
        //gamePad.leftShoulder
        //gamePad.rightShoulder

        //���Һ󷽴�����
        //gamePad.leftTrigger
        //gamePad.rightTrigger

        //wasPressedThisFrame
        //wasReleasedThisFrame
        //isPressed

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        //��ҡ��
        //print(Gamepad.current.leftStick.ReadValue());
        //��ҡ��
        //print(gamePad.rightStick.ReadValue());

        if (Gamepad.current.leftStickButton.wasPressedThisFrame)
        {
            print("��ҡ�˰���");
        }
        if (Gamepad.current.leftStickButton.wasReleasedThisFrame)
        {
            print("��ҡ��̧��");
        }
        if (Gamepad.current.leftStickButton.isPressed)
        {
            print("��ҡ�˳���");
        }

        if (Gamepad.current.dpad.left.wasPressedThisFrame)
        {
            print("���������");
        }
        if (Gamepad.current.dpad.left.wasReleasedThisFrame)
        {
            print("�����̧��");
        }
        if (Gamepad.current.dpad.left.isPressed)
        {
            print("���������");
        }

        if(Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            print("�Ϸ����� �����μ�(PS)����");
        }

        if(Gamepad.current.startButton.wasPressedThisFrame)
        {
            print("��ʼ������");
        }
        if (Gamepad.current.selectButton.wasPressedThisFrame)
        {
            print("ѡ�������");
        }


        if(Gamepad.current.leftShoulder.wasPressedThisFrame)
        {
            print("���粿ǰ����");
        }


        if (Gamepad.current.leftTrigger.wasPressedThisFrame)
        {
            print("���粿�󷽼�");
        }
    }
}
