using Excel;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    Excel转换

-----------------------*/


namespace ACFrameworkCore
{
    public class ExcelChange : EditorWindow
    {
        /// <summary>
        /// excel文件存放的路径
        /// </summary>
        public static string EXCEL_PATH = Application.dataPath + "/Editor/Excels/";

        [MenuItem("Tool/Excel转换")]//#E
        public static void GenerateExcelInfo()
        {
            //GetWindow(typeof(ExcelChange));
            //记在指定路径中的所有Excel文件 用于生成对应的3个文件
            DirectoryInfo dInfo = Directory.CreateDirectory(EXCEL_PATH);
            //得到指定路径中的所有文件信息 相当于就是得到所有的Excel表
            FileInfo[] files = dInfo.GetFiles();
            //数据表容器
            DataTableCollection tableConllection;
            for (int i = 0; i < files.Length; i++)
            {
                //如果不是excel文件就不要处理了
                if (files[i].Extension != ".xlsx" &&
                    files[i].Extension != ".xls")
                    continue;

                //打开一个Excel文件得到其中的所有表的数据
                using (FileStream fs = files[i].Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                    tableConllection = excelReader.AsDataSet().Tables;
                    //Debug.Log(excelReader.AsDataSet().Tables[0].TableName);
                    tableConllection = excelReader.AsDataSet().Tables;
                    fs.Close();
                }

                //遍历文件中的所有表的信息
                foreach (DataTable table in tableConllection)
                {
                    //生成数据结构类
                    //DataClass.GenerateExcelDataClass(table);
                    //生成容器类
                    //Container.GenerateExcelContainer(table);
                    //生成2进制数据
                    BinaryData.GenerateExcelBinary(table);
                }
            }

            //BinaryData.CreatBinaryData();
            //return;
            //string filePath = "F:\\Project\\ACFramework\\Assets\\Editor\\Excels\\TowerInfo.xlsx";
            ////打开文件
            //FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            ////判断加载的文件类型 解析
            //string[] fileType = filePath.Split('.');
            //IExcelDataReader excelReader = null;
            //switch (fileType[1])//报错异常处理 https://blog.csdn.net/qq_39221436/article/details/120951176
            //{
            //    case "xls": excelReader = ExcelReaderFactory.CreateBinaryReader(stream); break;
            //    case "xlsx": excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream); break;
            //}

            //DataTableCollection tableConllection;
            //tableConllection = excelReader.AsDataSet().Tables;
            //foreach (DataTable table in tableConllection)
            //{
            //    Debug.Log(table.TableName);
            //}
        }
    }
}
