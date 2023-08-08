using ACFrameworkCore;
using UnityEngine;
using UnityEngine.UI;


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

    public ConfigInputInfo inputInfo { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        inputInfo = InputSystemManager.Instance.inputInfo;
        UpdateBtnInfo();

        btnUp.onClick.AddListener(() =>
        {
            InputSystemManager.Instance.ChangeBtn(BTN_TYPE.UP);
        });
        btnDown.onClick.AddListener(() =>
        {
            InputSystemManager.Instance.ChangeBtn(BTN_TYPE.DOWN);
        });
        btnLeft.onClick.AddListener(() =>
        {
            InputSystemManager.Instance.ChangeBtn(BTN_TYPE.LEFT);
        });
        btnRight.onClick.AddListener(() =>
        {
            InputSystemManager.Instance.ChangeBtn(BTN_TYPE.RIGHT);
        });
        btnFire.onClick.AddListener(() =>
        {
            InputSystemManager.Instance.ChangeBtn(BTN_TYPE.FIRE);
        });
        btnJump.onClick.AddListener(() =>
        {
            InputSystemManager.Instance.ChangeBtn(BTN_TYPE.JUMP);
        });
    }

    private void Update()
    {
        UpdateBtnInfo();
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

}
