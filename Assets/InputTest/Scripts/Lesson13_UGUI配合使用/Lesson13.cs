using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson13 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 InputSystem对UI的支持
        //1.新输入系统InputSystem不支持IMGUI（GUI）注意：编辑器代码不受影响
        //  如果当前激活的是InputSystem，那么OnGUI中的输入判断相关内容不会被触发
        //  你必须要选择Both或者只激活老输入系统InputManager才能让OnGUI中内容有用

        //2.新输入系统支持UGUI，但是需要使用新输入系统输入模块（Input System UI Input Module）
        #endregion

        #region 知识点二 UGUI中的新输入系统输入模块参数相关

        #endregion

        #region 知识点三 VR相关中使用新输入系统注意事项
        //如果想在VR项目中使用新输入系统配合UGUI使用
        //需要在Canvas对象上添加Tracked Device Raycaster组件
        #endregion

        #region 知识点四 多人游戏使用多套UI
        //如果同一设备上的多人游戏，每个人想要使用自己的一套独立UI
        //需要将EventSystem中的EventSystem组件替换为Multiplayer Event System组件

        //与EventSystem组件不同，可以在场景中同时激活多个MultiplayerEventSystem。
        //这样，您可以有多个玩家，每个玩家都有自己的InputSystemUIInputModule和MultiplayerEventSystem组件
        //每个玩家都可以有自己的一组操作来驱动自己的UI实例。
        //如果您正在使用PlayerInput组件，还可以设置PlayerInput以自动配置玩家的InputSystemUIInputModule以使用玩家的操作
        //MultilayerEventSystem组件的属性与事件系统中的属性相同
        //此外，MultiplayerEventSystem组件还添加了一个playerRoot属性，您可以将其设置为一个游戏对象
        //该游戏对象包含此事件系统应在其层次结构中处理的所有UI可选择项
        #endregion

        #region 知识点五 On-Screen组件相关
        //On-Screen组件可以模拟UI和用户操作的交互
        //1.On-Screen Button：按钮交互
        //2.On-Screen Stick：摇杆交互
        #endregion

        #region 知识点六 更多内容
        //可查看官方文档了解更多新输入系统和UI配合使用的相关内容
        //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.2/manual/UISupport.html
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
