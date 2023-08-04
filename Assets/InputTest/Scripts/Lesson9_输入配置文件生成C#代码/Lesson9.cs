using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson9 : MonoBehaviour
{
    Lesson9Input input;
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 根据配置文件生成C#代码
        //1.选择InputActions文件
        //2.在Inspector窗口设置生成路径，类名，命名空间
        //3.应用后生成代码
        #endregion

        #region 知识点二 使用C#代码进行监听
        //1.创建生成的代码对象
        input = new Lesson9Input();
        //2.激活输入
        input.Enable();
        //3.事件监听
        input.Action1.Fire.performed += (context) =>
        {
            print("开火");
        };

        input.Action2.Space.performed += (context) =>
        {
            print("跳跃");
        };
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        print(input.Action1.Move.ReadValue<Vector2>());
    }
}
