using ACFrameworkCore;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

/*--------�ű�����-----------
				
�������䣺
	1607388033@qq.com
����:
	����
����:
    �������

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
            // context.phase �����ĵ�ǰ�׶Ρ��൱�ڷ���
            //Performed ִ��
            if (context.phase == InputActionPhase.Performed)
            {
                Debug.Log($"������ {context.action.activeControl}");
                switch (context.action.name)
                {
                    case "Fire":
                        Debug.Log("����");
                        ////���λ�õ����߼��
                        ////���ǰ��������¼� �Ͳ���������������
                        //if (context.phase != InputActionPhase.Performed)
                        //    return;
                        //RaycastHit info;
                        //if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out info))
                        //{
                        //    //�õ��ӵ��ɳ�ȥ������
                        //    Vector3 point = info.point;
                        //    point.y = this.transform.position.y;
                        //    Vector3 dir = point - this.transform.position;
                        //    //�����ӵ� �ɳ�ȥ
                        //    Instantiate(bullet, this.transform.position, Quaternion.LookRotation(dir));
                        //}
                        break;
                    case "Jump":
                        Debug.Log("��Ծ");
                        //if (context.phase == InputActionPhase.Performed)
                        //    body.AddForce(Vector3.up * 200);
                        break;
                    case "Move":
                        Debug.Log("�ƶ�");
                        //dir = context.ReadValue<Vector2>();
                        //dir.z = dir.y;
                        //dir.y = 0;
                        break;
                }
            }
        };
    }
}
