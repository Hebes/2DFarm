using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class test : MonoBehaviour
{
    void Start()
    {
        //using (FileStream fs = File.Open(Application.dataPath + "/HotUpdate/GameMain/ExcelData/Binary/TestInfo.Bytes", FileMode.Open, FileAccess.Read))
        //{
        //    //����һ�� 2���Ƹ�ʽ����
        //    BinaryFormatter bf = new BinaryFormatter();
        //    //�����л�
        //    TowerInfo p = bf.Deserialize(fs) as TowerInfo;

        //    fs.Close();
        //}

        //BinaryDataMgr.Instance.InitData();
        //BinaryDataMgr.Instance.LoadTable<TowerInfoContainer, TowerInfo>();


        //TowerInfoContainer data = BinaryDataMgr.Instance.GetTable<TowerInfoContainer>();
        //print(data.dataDic[5].name);
    }
}
