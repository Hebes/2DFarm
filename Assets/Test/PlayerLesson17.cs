using ACFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*--------�ű�����-----------
				
�������䣺
	1607388033@qq.com
����:
	����
����:
    �������

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
        //                print("����");
        //                break;
        //            case "Jump":
        //                print("��Ծ");
        //                break;
        //            case "Move":
        //                print("�ƶ�");
        //                break;
        //        }
        //    }
        //};
    }
}
