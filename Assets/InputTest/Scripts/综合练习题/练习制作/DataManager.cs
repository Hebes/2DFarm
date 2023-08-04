using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataManager
{
    private static DataManager instance = new DataManager();

    public static DataManager Instance => instance;

    private InputInfo inputInfo;

    public InputInfo InputInfo => inputInfo;

    private string jsonStr;

    private DataManager()
    {
        inputInfo = new InputInfo();

        jsonStr = Resources.Load<TextAsset>("Lesson17").text;
    }

    public InputActionAsset GetActionAsset()
    {
        //ÉÏ¼ü
        string str = jsonStr.Replace("<up>", inputInfo.up);
        //ÏÂ
        str = str.Replace("<down>", inputInfo.down);
        //×ó
        str = str.Replace("<left>", inputInfo.left);
        //ÓÒ
        str = str.Replace("<right>", inputInfo.right);
        //¿ª»ð
        str = str.Replace("<fire>", inputInfo.fire);
        //ÌøÔ¾
        str = str.Replace("<jump>", inputInfo.jump);

        return InputActionAsset.FromJson(str);
    }
}
