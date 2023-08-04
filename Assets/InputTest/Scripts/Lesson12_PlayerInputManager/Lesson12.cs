using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson12 : MonoBehaviour
{
    private Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 PlayerInputManager的作用
        //PlayerInputManager 组件主要是用于管理本地多人输入的输入管理器
        //它主要管理玩家加入和离开
        #endregion

        #region 知识点二 组件添加及参数相关
        #endregion

        #region 知识点三 PlayerInputManager使用
        //获取PlayerInputManager
        //PlayerInputManager.instance
        //玩家加入时
        PlayerInputManager.instance.onPlayerJoined += (playerInput) =>
        {
            print("创建了一个玩家");
        };
        //玩家离开时
        PlayerInputManager.instance.onPlayerLeft += (playerInput) =>
        {
            print("离开了一个玩家");
        };

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(dir * 10 * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
        dir.z = dir.y;
        dir.y = 0;
    }
}
