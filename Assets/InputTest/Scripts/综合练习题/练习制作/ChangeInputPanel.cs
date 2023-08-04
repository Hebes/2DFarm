using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public enum BTN_TYPE
{
    UP,
    DOWN,
    LEFT,
    RIGHT,

    FIRE,
    JUMP, 
}

public class ChangeInputPanel : MonoBehaviour
{
    public Text txtUp;
    public Text txtDown;
    public Text txtLeft;
    public Text txtRight;
    public Text txtFire;
    public Text txtJump;

    public Button btnUp;
    public Button btnDown;
    public Button btnLeft;
    public Button btnRight;
    public Button btnFire;
    public Button btnJump;

    private InputInfo inputInfo;

    //记录当前改哪一个键
    private BTN_TYPE nowType;

    public PlayerLesson17 player;

    // Start is called before the first frame update
    void Start()
    {
        inputInfo = DataManager.Instance.InputInfo;
        UpdateBtnInfo();

        btnUp.onClick.AddListener(() =>
        {
            ChangeBtn(BTN_TYPE.UP);
        });
        btnDown.onClick.AddListener(() =>
        {
            ChangeBtn(BTN_TYPE.DOWN);
        });
        btnLeft.onClick.AddListener(() =>
        {
            ChangeBtn(BTN_TYPE.LEFT);
        });
        btnRight.onClick.AddListener(() =>
        {
            ChangeBtn(BTN_TYPE.RIGHT);
        });
        btnFire.onClick.AddListener(() =>
        {
            ChangeBtn(BTN_TYPE.FIRE);
        });
        btnJump.onClick.AddListener(() =>
        {
            ChangeBtn(BTN_TYPE.JUMP);
        });
    }

    private void ChangeBtn(BTN_TYPE type)
    {
        nowType = type;
        //得到一次任意键输入
        InputSystem.onAnyButtonPress.CallOnce(ChangeBtnReally);
    }

    private void ChangeBtnReally(InputControl control)
    {
        print(control.path);
        string[] strs = control.path.Split('/');
        string path = "<" + strs[1] + ">/" + strs[2];
        switch (nowType)
        {
            case BTN_TYPE.UP:
                inputInfo.up = path;
                break;
            case BTN_TYPE.DOWN:
                inputInfo.down = path;
                break;
            case BTN_TYPE.LEFT:
                inputInfo.left = path;
                break;
            case BTN_TYPE.RIGHT:
                inputInfo.right = path;
                break;
            case BTN_TYPE.FIRE:
                inputInfo.fire = path;
                break;
            case BTN_TYPE.JUMP:
                inputInfo.jump = path;
                break;
        }

        UpdateBtnInfo();

        //让玩家产生改建效果
        player.ChangeInput();
    }

    /// <summary>
    /// 更新键位显示信息
    /// </summary>
    private void UpdateBtnInfo()
    {
        txtUp.text = inputInfo.up;
        txtDown.text = inputInfo.down;
        txtLeft.text = inputInfo.left;
        txtRight.text = inputInfo.right;

        txtFire.text = inputInfo.fire;
        txtJump.text = inputInfo.jump;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
