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

    //private InputInfo inputInfo;

    ////记录当前改哪一个键
    //private BTN_TYPE nowType;

    //public PlayerLesson17 player;

    // Start is called before the first frame update
    void Start()
    {
        //inputInfo = DataManager.Instance.InputInfo;
        //UpdateBtnInfo();

        //btnUp.onClick.AddListener(() =>
        //{
        //    ChangeBtn(BTN_TYPE.UP);
        //});
        //btnDown.onClick.AddListener(() =>
        //{
        //    ChangeBtn(BTN_TYPE.DOWN);
        //});
        //btnLeft.onClick.AddListener(() =>
        //{
        //    ChangeBtn(BTN_TYPE.LEFT);
        //});
        //btnRight.onClick.AddListener(() =>
        //{
        //    ChangeBtn(BTN_TYPE.RIGHT);
        //});
        //btnFire.onClick.AddListener(() =>
        //{
        //    ChangeBtn(BTN_TYPE.FIRE);
        //});
        //btnJump.onClick.AddListener(() =>
        //{
        //    ChangeBtn(BTN_TYPE.JUMP);
        //});
    }


    /// <summary>
    /// 更新键位显示信息
    /// </summary>
    private void UpdateBtnInfo()
    {
        //txtUp.text = inputInfo.up;
        //txtDown.text = inputInfo.down;
        //txtLeft.text = inputInfo.left;
        //txtRight.text = inputInfo.right;

        //txtFire.text = inputInfo.fire;
        //txtJump.text = inputInfo.jump;
    }

}
