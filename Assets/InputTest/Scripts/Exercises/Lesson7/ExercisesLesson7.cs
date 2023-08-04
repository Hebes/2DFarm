using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExercisesLesson7 : MonoBehaviour
{
    public GameObject bullet;

    //[Header("�ƶ�����")]
    //public InputAction move;
    //[Header("��Ծ����")]
    //public InputAction jump;
    //[Header("�������")]
    //public InputAction fire;

    private Rigidbody body;
    private Vector3 dir;

    private PlayerTest playerTest;

    // Start is called before the first frame update
    void Start()
    {
        body = this.GetComponent<Rigidbody>();

        //new�������
        playerTest = new PlayerTest();
        //������
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
        //    //���λ�õ����߼��
        //    RaycastHit info;
        //    if(Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out info))
        //    {
        //        //�õ��ӵ��ɳ�ȥ������
        //        Vector3 point = info.point;
        //        point.y = this.transform.position.y;
        //        Vector3 dir = point - this.transform.position;
        //        //�����ӵ� �ɳ�ȥ
        //        Instantiate(bullet, this.transform.position, Quaternion.LookRotation(dir));
        //    }

        //};

        playerTest.Player.Fire.performed += (context) =>
        {
            //���λ�õ����߼��
            RaycastHit info;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out info))
            {
                //�õ��ӵ��ɳ�ȥ������
                Vector3 point = info.point;
                point.y = this.transform.position.y;
                Vector3 dir = point - this.transform.position;
                //�����ӵ� �ɳ�ȥ
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
