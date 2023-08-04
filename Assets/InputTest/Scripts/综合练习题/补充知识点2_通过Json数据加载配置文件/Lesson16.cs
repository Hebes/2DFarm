using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson16 : MonoBehaviour
{
    public PlayerInput input;
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 分析PlayerInput文件中的输入配置文件

        #endregion

        #region 知识点二 通过Json手动加载输入配置文件
        string json = Resources.Load<TextAsset>("PlayerInputTest").text;
        InputActionAsset asset = InputActionAsset.FromJson(json);

        input.actions = asset;

        input.onActionTriggered += (context) =>
        {
            if(context.phase == InputActionPhase.Performed)
            {
                switch (context.action.name)
                {
                    case "Move":
                        print("移动");
                        break;
                    case "Look":
                        print("看向");
                        break;
                    case "Fire":
                        print("开火");
                        break;
                }
            }
        };
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
