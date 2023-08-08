using ACFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    控制玩家

-----------------------*/

public class PlayerLesson17 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = this.GetComponent<PlayerInput>();
        InputSystemManager inputSystemManager = InputSystemManager.Instance;
        inputSystemManager.InitPlayerInput(playerInput);
        inputSystemManager.jsonStr = Resources.Load<TextAsset>("Lesson17").text;

        //playerInput.actions = inputSystemManager.GetActionAsset();
        //playerInput.actions.Enable();

        //playerInput.onActionTriggered += (context) =>
        //{
        //    if(context.phase == InputActionPhase.Performed)
        //    {
        //        switch (context.action.name)
        //        {
        //            case "Fire":
        //                print("开火");
        //                break;
        //            case "Jump":
        //                print("跳跃");
        //                break;
        //            case "Move":
        //                print("移动");
        //                break;
        //        }
        //    }
        //};
    }
}
