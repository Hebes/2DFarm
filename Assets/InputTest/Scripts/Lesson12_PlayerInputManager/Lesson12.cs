using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson12 : MonoBehaviour
{
    private Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ PlayerInputManager������
        //PlayerInputManager �����Ҫ�����ڹ����ض�����������������
        //����Ҫ������Ҽ�����뿪
        #endregion

        #region ֪ʶ��� �����Ӽ��������
        #endregion

        #region ֪ʶ���� PlayerInputManagerʹ��
        //��ȡPlayerInputManager
        //PlayerInputManager.instance
        //��Ҽ���ʱ
        PlayerInputManager.instance.onPlayerJoined += (playerInput) =>
        {
            print("������һ�����");
        };
        //����뿪ʱ
        PlayerInputManager.instance.onPlayerLeft += (playerInput) =>
        {
            print("�뿪��һ�����");
        };

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(dir * 10 * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
        dir.z = dir.y;
        dir.y = 0;
    }
}
