using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson16 : MonoBehaviour
{
    public PlayerInput input;
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ ����PlayerInput�ļ��е����������ļ�

        #endregion

        #region ֪ʶ��� ͨ��Json�ֶ��������������ļ�
        string json = Resources.Load<TextAsset>("PlayerInputTest").text;
        InputActionAsset asset = InputActionAsset.FromJson(json);

        input.actions = asset;

        input.onActionTriggered += (context) =>
        {
            if(context.phase == InputActionPhase.Performed)
            {
                switch (context.action.name)
                {
                    case "Move":
                        print("�ƶ�");
                        break;
                    case "Look":
                        print("����");
                        break;
                    case "Fire":
                        print("����");
                        break;
                }
            }
        };
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
