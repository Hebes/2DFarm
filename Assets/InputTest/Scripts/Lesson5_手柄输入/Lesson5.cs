using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson5 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 获取当前手柄
        Gamepad gamePad = Gamepad.current;
        if (gamePad == null)
            return;
        #endregion

        #region 知识点二 手柄摇杆
        //摇杆方向
        //左摇杆
        print(gamePad.leftStick.ReadValue());
        //右摇杆
        print(gamePad.rightStick.ReadValue());

        //摇杆按下
        //右摇杆 按下抬起长按相关
        //gamePad.rightStickButton
        //左摇杆
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

        #region 知识点三 手柄方向键
        //对应手柄上4个方向键 上下左右
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

        #region 知识点四 手柄右侧按键
        //通用
        //Y、△
        //gamePad.buttonNorth
        //A、X
        //gamePad.buttonSouth
        //X、□
        //gamePad.buttonWest
        //B、○
        //gamePad.buttonEast

        //wasPressedThisFrame
        //wasReleasedThisFrame
        //isPressed

        //手柄右侧按钮 x ○ △ □ A B Y 
        //○
        //gamePad.circleButton
        //△
        //gamePad.triangleButton
        //□
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

        #region 知识点五 手柄中央按键
        //中央键
        //gamePad.startButton
        //gamePad.selectButton

        //wasPressedThisFrame
        //wasReleasedThisFrame
        //isPressed

        #endregion

        #region 知识点六 手柄肩部按键
        //左上右上 肩部键位
        //左右前方肩部键
        //gamePad.leftShoulder
        //gamePad.rightShoulder

        //左右后方触发键
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
        //左摇杆
        //print(Gamepad.current.leftStick.ReadValue());
        //右摇杆
        //print(gamePad.rightStick.ReadValue());

        if (Gamepad.current.leftStickButton.wasPressedThisFrame)
        {
            print("左摇杆按下");
        }
        if (Gamepad.current.leftStickButton.wasReleasedThisFrame)
        {
            print("左摇杆抬起");
        }
        if (Gamepad.current.leftStickButton.isPressed)
        {
            print("左摇杆长按");
        }

        if (Gamepad.current.dpad.left.wasPressedThisFrame)
        {
            print("左方向键按下");
        }
        if (Gamepad.current.dpad.left.wasReleasedThisFrame)
        {
            print("左方向键抬起");
        }
        if (Gamepad.current.dpad.left.isPressed)
        {
            print("左方向键长按");
        }

        if(Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            print("上方按键 三角形键(PS)按下");
        }

        if(Gamepad.current.startButton.wasPressedThisFrame)
        {
            print("开始键按下");
        }
        if (Gamepad.current.selectButton.wasPressedThisFrame)
        {
            print("选择键按下");
        }


        if(Gamepad.current.leftShoulder.wasPressedThisFrame)
        {
            print("左侧肩部前方键");
        }


        if (Gamepad.current.leftTrigger.wasPressedThisFrame)
        {
            print("左侧肩部后方键");
        }
    }
}
