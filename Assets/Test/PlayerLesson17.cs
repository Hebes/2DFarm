using ACFrameworkCore;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    控制玩家

-----------------------*/

public class PlayerLesson17 : MonoBehaviour
{
    public GameObject bullet;
    public Vector3 dir;
    public Rigidbody body;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = this.GetComponent<PlayerInput>();
        InputSystemManager inputSystemManager = InputSystemManager.Instance;
        inputSystemManager.playerInput = playerInput;
        inputSystemManager.EnableInputConfig();
        playerInput.onActionTriggered += (context) =>
        {
            // context.phase 动作的当前阶段。相当于访问
            //Performed 执行
            if (context.phase == InputActionPhase.Performed)
            {
                Debug.Log($"按键是 {context.action.activeControl}");
                switch (context.action.name)
                {
                    case "Fire":
                        Debug.Log("开火");
                        ////鼠标位置的射线检测
                        ////不是按键触发事件 就不处理后面的内容了
                        //if (context.phase != InputActionPhase.Performed)
                        //    return;
                        //RaycastHit info;
                        //if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out info))
                        //{
                        //    //得到子弹飞出去的向量
                        //    Vector3 point = info.point;
                        //    point.y = this.transform.position.y;
                        //    Vector3 dir = point - this.transform.position;
                        //    //创建子弹 飞出去
                        //    Instantiate(bullet, this.transform.position, Quaternion.LookRotation(dir));
                        //}
                        break;
                    case "Jump":
                        Debug.Log("跳跃");
                        //if (context.phase == InputActionPhase.Performed)
                        //    body.AddForce(Vector3.up * 200);
                        break;
                    case "Move":
                        Debug.Log("移动");
                        //dir = context.ReadValue<Vector2>();
                        //dir.z = dir.y;
                        //dir.y = 0;
                        break;
                }
            }
        };
    }
}
