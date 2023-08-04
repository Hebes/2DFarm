using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExercisesLesson7 : MonoBehaviour
{
    public GameObject bullet;

    //[Header("移动控制")]
    //public InputAction move;
    //[Header("跳跃控制")]
    //public InputAction jump;
    //[Header("开火控制")]
    //public InputAction fire;

    private Rigidbody body;
    private Vector3 dir;

    private PlayerTest playerTest;

    // Start is called before the first frame update
    void Start()
    {
        body = this.GetComponent<Rigidbody>();

        //new输入对象
        playerTest = new PlayerTest();
        //激活它
        playerTest.Enable();

        //move.Enable();
        //jump.Enable();
        //fire.Enable();

        //jump.performed += (context) =>
        //{
        //    body.AddForce(Vector3.up * 200);
        //};

        playerTest.Player.Jump.performed += (context) =>
        {
            body.AddForce(Vector3.up * 200);
        };

        //fire.performed += (context) =>
        //{
        //    //鼠标位置的射线检测
        //    RaycastHit info;
        //    if(Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out info))
        //    {
        //        //得到子弹飞出去的向量
        //        Vector3 point = info.point;
        //        point.y = this.transform.position.y;
        //        Vector3 dir = point - this.transform.position;
        //        //创建子弹 飞出去
        //        Instantiate(bullet, this.transform.position, Quaternion.LookRotation(dir));
        //    }

        //};

        playerTest.Player.Fire.performed += (context) =>
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
        };
    }

    // Update is called once per frame
    void Update()
    {
        //dir = move.ReadValue<Vector2>();
        dir = playerTest.Player.Move.ReadValue<Vector2>();
        dir.z = dir.y;
        dir.y = 0;
        body.AddForce(dir);
    }
}
