using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*--------�ű�����-----------
				
�������䣺
	1607388033@qq.com
����:
	����
����:
    ������

-----------------------*/

public class WebRequest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetData("http://127.0.0.1:8000/1.txt"));
    }

    IEnumerator GetData(string urlPath)
    {
        //2.����һ��UnityWebRequest�� method����ΪGet
        UnityWebRequest request = UnityWebRequest.Get(urlPath);
        //3.�ȴ���Ӧʱ�䣬����5�����
        request.timeout = 5;
        //4.����������Ϣ
        yield return request.SendWebRequest();

        //5.�ж��Ƿ��������
        if (request.isDone)
        {
            //6.�ж��Ƿ����ش���
            if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
                Debug.Log(request.error);
            else
                Debug.Log(request.downloadHandler.text);
        }
    }
}
