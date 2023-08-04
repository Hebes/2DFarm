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
        //�ϼ�
        string str = jsonStr.Replace("<up>", inputInfo.up);
        //��
        str = str.Replace("<down>", inputInfo.down);
        //��
        str = str.Replace("<left>", inputInfo.left);
        //��
        str = str.Replace("<right>", inputInfo.right);
        //����
        str = str.Replace("<fire>", inputInfo.fire);
        //��Ծ
        str = str.Replace("<jump>", inputInfo.jump);

        return InputActionAsset.FromJson(str);
    }
}
