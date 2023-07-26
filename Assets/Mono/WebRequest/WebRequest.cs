using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    下载器

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
        //2.创建一个UnityWebRequest类 method属性为Get
        UnityWebRequest request = UnityWebRequest.Get(urlPath);
        //3.等待响应时间，超过5秒结束
        request.timeout = 5;
        //4.发送请求信息
        yield return request.SendWebRequest();

        //5.判断是否下载完成
        if (request.isDone)
        {
            //6.判断是否下载错误
            if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
                Debug.Log(request.error);
            else
                Debug.Log(request.downloadHandler.text);
        }
    }
}
