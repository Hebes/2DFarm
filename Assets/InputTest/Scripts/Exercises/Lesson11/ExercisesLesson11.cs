using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExercisesLesson11 : MonoBehaviour
{
    public GameObject bullet;

    public Rigidbody body;

    private Vector3 dir;

    public PlayerInput input;
    // Start is called before the first frame update
    void Start()
    {
        //body = this.GetComponent<Rigidbody>();

        input.onActionTriggered += (context) =>
        {
            switch (context.action.name)
            {
                case "Move":
                    dir = context.ReadValue<Vector2>();
                    dir.z = dir.y;
                    dir.y = 0;
                    break;
                case "Jump":
                    if(context.phase == InputActionPhase.Performed)
                        body.AddForce(Vector3.up * 200);
                    break;
                case "Fire":
                    //鼠标位置的射线检测
                    //不是按键触发事件 就不处理后面的内容了
                    if (context.phase != InputActionPhase.Performed)
                        return;
                    RaycastHit info;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out info))
                    {
                        //得到子弹飞出去的向量
                        Vector3 point = info.point;
                        point.y = this.transform.position.y;
                        Vector3 dir = point - this.transform.position;
                        //创建子弹 飞出去
                        Instantiate(bullet, this.transform.position, Quaternion.LookRotation(dir));
                    }
                    break;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        body.AddForce(dir);
    }

    public void OnJump(InputValue value)
    {
        body.AddForce(Vector3.up * 200);
    }

    public void OnFire(InputValue value)
    {
        //鼠标位置的射线检测
        RaycastHit info;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out info))
        {
            //得到子弹飞出去的向量
            Vector3 point = info.point;
            point.y = this.transform.position.y;
            Vector3 dir = point - this.transform.position;
            //创建子弹 飞出去
            Instantiate(bullet, this.transform.position, Quaternion.LookRotation(dir));
        }
    }

    //需要获取值的 这种函数 需要注意 只会在改变时进入函数
    public void OnMove(InputValue value)
    {
        dir = value.Get<Vector2>();
        dir.z = dir.y;
        dir.y = 0;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        body.AddForce(Vector3.up * 200);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        //鼠标位置的射线检测
        RaycastHit info;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out info))
        {
            //得到子弹飞出去的向量
            Vector3 point = info.point;
            point.y = this.transform.position.y;
            Vector3 dir = point - this.transform.position;
            //创建子弹 飞出去
            Instantiate(bullet, this.transform.position, Quaternion.LookRotation(dir));
        }
    }

    //需要获取值的 这种函数 需要注意 只会在改变时进入函数
    public void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
        dir.z = dir.y;
        dir.y = 0;
    }
}
