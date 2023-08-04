using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLesson17 : MonoBehaviour
{
    private PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();

        playerInput.actions = DataManager.Instance.GetActionAsset();
        playerInput.actions.Enable();

        playerInput.onActionTriggered += (context) =>
        {
            if(context.phase == InputActionPhase.Performed)
            {
                switch (context.action.name)
                {
                    case "Fire":
                        print("开火");
                        break;
                    case "Jump":
                        print("跳跃");
                        break;
                    case "Move":
                        print("移动");
                        break;
                }
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //让玩家产生改建效果
    public void ChangeInput()
    {
        //改建就是改变 我们PlayerInput上关联的输入配置信息嘛
        playerInput.actions = DataManager.Instance.GetActionAsset();
        playerInput.actions.Enable();
    }
}
