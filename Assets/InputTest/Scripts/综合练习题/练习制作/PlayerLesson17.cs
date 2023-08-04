using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLesson17 : MonoBehaviour
{
    private PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();

        playerInput.actions = DataManager.Instance.GetActionAsset();
        playerInput.actions.Enable();

        playerInput.onActionTriggered += (context) =>
        {
            if(context.phase == InputActionPhase.Performed)
            {
                switch (context.action.name)
                {
                    case "Fire":
                        print("����");
                        break;
                    case "Jump":
                        print("��Ծ");
                        break;
                    case "Move":
                        print("�ƶ�");
                        break;
                }
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //����Ҳ����Ľ�Ч��
    public void ChangeInput()
    {
        //�Ľ����Ǹı� ����PlayerInput�Ϲ���������������Ϣ��
        playerInput.actions = DataManager.Instance.GetActionAsset();
        playerInput.actions.Enable();
    }
}
