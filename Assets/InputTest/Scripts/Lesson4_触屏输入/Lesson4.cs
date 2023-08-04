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
        #region 知识点一 获取当前触屏设备
        Touchscreen ts = Touchscreen.current;
        //由于触屏相关都是在移动平台或提供触屏的设备上使用
        //所以在使用时最好做一次判空
        if (ts == null)
            return;
        #endregion

        #region 知识点二 得到触屏手指信息
        //得到触屏手指数量
        print(ts.touches.Count);
        //得到单个触屏手指
        //ts.touches[1]
        //得到所有触屏手指
        foreach (var item in ts.touches)
        {
            
        }
        #endregion

        #region 知识点三 手指按下 抬起 长按 点击
        //获取指定索引手指
        TouchControl tc = ts.touches[0];
        //按下 抬起
        if(tc.press.wasPressedThisFrame)
        {

        }
        if(tc.press.wasReleasedThisFrame)
        {

        }
        //长按
        if(tc.press.isPressed)
        {

        }

        //点击手势
        if(tc.tap.isPressed)
        {

        }

        //连续点击次数
        print(tc.tapCount);

        #endregion

        #region 知识点四 手指位置等相关信息
        //位置
        print(tc.position.ReadValue());
        //第一次接触时位置
        print(tc.startPosition.ReadValue());
        //接触区域大小
        tc.radius.ReadValue();
        //偏移位置
        tc.delta.ReadValue();

        //得到当前手指的 状态（阶段）
        UnityEngine.InputSystem.TouchPhase tp = tc.phase.ReadValue();
        switch (tp)
        {
            //无
            case UnityEngine.InputSystem.TouchPhase.None:
                break;
            //开始接触
            case UnityEngine.InputSystem.TouchPhase.Began:
                break;
            //移动
            case UnityEngine.InputSystem.TouchPhase.Moved:
                break;
            //结束
            case UnityEngine.InputSystem.TouchPhase.Ended:
                break;
            //取消
            case UnityEngine.InputSystem.TouchPhase.Canceled:
                break;
            //静止
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
