using DG.Tweening.Plugins.Core.PathCore;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ACFrameworkCore
{
    /// <summary>
    /// https://blog.csdn.net/Xz616/article/details/128893023
    /// https://www.cnblogs.com/noteswiki/p/6095868.html
    /// https://blog.csdn.net/shuaiLS/article/details/107369443
    /// </summary>
    public class ExcelWrite : EditorWindow
    {

        public static string NewLoadExcelPath = string.Empty;   //新路径
        public static string OldLoadExcelPath = string.Empty;   //老路径
        private bool isSelectFile = true;                       //点击加载路径
        private Vector2 scrollPosition = Vector2.zero;          //滑动
        private Vector2 scrollExcel = Vector2.zero;             //滑动
        string[][] data = null;                                 //数据
        private string Message = string.Empty;                  //消息提示
        private Dictionary<string, string> DesDic;              //Excel描述信息

        private void Awake()
        {
            //读取说明文档
            DesDic = new Dictionary<string, string>();
            string desDicPath = $"{Application.dataPath}/Editor/Excels/ExcelDes.txt";

            FileStream fileStream = File.Open(desDicPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //byte[] buffer = new byte[fileStream.Length];
            //fileStream.Read(buffer, 0, buffer.Length);
            //string fileContent = System.Text.Encoding.UTF8.GetString(buffer);
            //fileContent.Split('\t');
            //Debug.Log(fileContent);
            StreamReader reader = new StreamReader(fileStream);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] strings = line.Split(',');
                DesDic.Add(strings[0], strings[1]);
            }
            reader.Close();
            fileStream.Close();

            //ReadData();
        }

        [MenuItem("Tool/编辑Excel#E #E")]
        public static void BuildPackageVersions()
        {
            if (!EditorWindow.HasOpenInstances<ExcelWrite>())
                GetWindow(typeof(ExcelWrite), false, "Excel数据填充").Show();
            else
                GetWindow(typeof(ExcelWrite)).Close();
        }

        private void OnGUI()
        {
            GUILayout.Space(5f);
            EditorGUILayout.LabelField("加载单个Excel文件", EditorStyles.label);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Browse...", GUILayout.Width(80f))) { BrowseLoadFilePanel(); }
            isSelectFile = EditorGUILayout.ToggleLeft("点击加载文件路径", isSelectFile, GUILayout.Width(130f));
            EditorGUILayout.LabelField("选择的Excel文件路径:", EditorStyles.label, GUILayout.Width(130));
            NewLoadExcelPath = EditorGUILayout.TextField(NewLoadExcelPath);
            EditorGUILayout.EndHorizontal();


            scrollExcel = GUILayout.BeginScrollView(scrollExcel, GUILayout.Height(60));
            GUILayout.Space(10f);
            EditorGUILayout.BeginHorizontal();
            string[] paths = Directory.GetFiles($"{Application.dataPath}/Editor/Excels");
            for (int i = 0; i < paths.Length; i++)
            {

                string path = paths[i];
                if (path.EndsWith("meta") || path.EndsWith("txt")) continue;

                string[] strings = path.Split('\\');
                string btnName = string.Empty;
                if (DesDic.TryGetValue(strings[strings.Length - 1], out string value))
                    btnName = $"{strings[strings.Length - 1]}\t\n{value}";
                else
                    btnName = $"{strings[strings.Length - 1]}";
                if (GUILayout.Button(btnName))
                {
                    NewLoadExcelPath = path;
                    ReadData();
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.EndScrollView();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("读取数据", GUILayout.Width(80f)))
            {
                ReadData();
            }
            if (GUILayout.Button("添加数据", GUILayout.Width(80f)))
            {
                if (data == null)
                {
                    Message = "请先读取数据";
                    return;
                }

                string[] strs = new string[data[0].Length];
                List<string[]> strings = data.ToList();
                strings.Add(strs);
                data = strings.ToArray();
                Message = "数据添加成功";
            }
            if (GUILayout.Button("写入数据", GUILayout.Width(80f)))
            {
                FileInfo _excelName = new FileInfo(NewLoadExcelPath);
                //通过ExcelPackage打开文件
                using (ExcelPackage package = new ExcelPackage(_excelName))
                {
                    //1表示第一个表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    //Debug.Log(worksheet.Name);
                    for (int i = 0; i < data.Length; i++)
                    {
                        string[] item1 = data[i];
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width));
                        for (global::System.Int32 j = 0; j < item1.Length; j++)
                        {
                            string item2 = item1[j];
                            if (IsInteger(item2, out int number1))
                                worksheet.SetValue(i + 1, j + 1, number1);
                            else if (IsFloateger(item2, out float number2))
                                worksheet.SetValue(i + 1, j + 1, number2);
                            else if (string.IsNullOrEmpty(item2))
                                worksheet.SetValue(i + 1, j + 1, null);
                            else
                                worksheet.Cells[i + 1, j + 1].Value = item2;
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    package.Save(); //储存
                }
                Message = "数据写入成功";
            }
            if (GUILayout.Button("清除消息", GUILayout.Width(80f)))
            {
                Message = string.Empty;
            }
            //其他功能
            if (GUILayout.Button("Excel转换", GUILayout.Width(80f)))
            {
                ExcelChange.GenerateExcelInfo();
                Message = "转换成功";
            }
            if (GUILayout.Button("打开Excel", GUILayout.Width(80f)))
            {
                Application.OpenURL(NewLoadExcelPath);
            }
            EditorGUILayout.LabelField("消息提示:", GUILayout.Width(80f));
            EditorGUILayout.LabelField(Message, EditorStyles.label);
            EditorGUILayout.EndHorizontal();


            ClickFileLoadPath();
            RefreshData();
        }




        /// <summary>
        /// 打开文件
        /// </summary>
        private void BrowseLoadFilePanel()
        {
            string directory = EditorUtility.OpenFilePanel("选择Execl文件", NewLoadExcelPath, "xls,xlsx,csv");
            if (!string.IsNullOrEmpty(directory))
                NewLoadExcelPath = directory;
        }
        private void RefreshData()
        {
            if (data == null) return;
            for (int i = 0; i < 3; i++)
            {
                string[] item1 = data[i];
                EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width));
                if (GUILayout.Button("删除", GUILayout.Width(80f)))
                {
                    //删除内存数据
                    List<string[]> strings = data.ToList();
                    strings.Remove(item1);
                    data = strings.ToArray();
                    //删除实际数据
                    int deleteNumber = i;
                    FileInfo _excelName = new FileInfo(NewLoadExcelPath);
                    //通过ExcelPackage打开文件
                    using (ExcelPackage package = new ExcelPackage(_excelName))
                    {
                        //1表示第一个表
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        for (int j = 0; j < data[0].Length; j++)
                            worksheet.SetValue(deleteNumber + 1, j + 1, null);
                        package.Save(); //储存
                    }
                    Message = "数据删除成功";

                    EditorGUILayout.EndHorizontal();
                    continue;
                }
                for (global::System.Int32 j = 0; j < item1.Length; j++)
                {
                    string item2 = item1[j];
                    data[i][j] = EditorGUILayout.TextField(item2);
                }
                EditorGUILayout.EndHorizontal();
            }

            //显示数据
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);//GUILayout.Width(400), GUILayout.Height(500)
            for (int i = 3; i < data.Length; i++)
            {
                string[] item1 = data[i];
                EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width));
                if (GUILayout.Button("删除", GUILayout.Width(80f)))
                {
                    //删除内存数据
                    List<string[]> strings = data.ToList();
                    strings.Remove(item1);
                    data = strings.ToArray();
                    //删除实际数据
                    int deleteNumber = i;
                    FileInfo _excelName = new FileInfo(NewLoadExcelPath);
                    //通过ExcelPackage打开文件
                    using (ExcelPackage package = new ExcelPackage(_excelName))
                    {
                        //1表示第一个表
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        for (int j = 0; j < data[0].Length; j++)
                            worksheet.SetValue(deleteNumber + 1, j + 1, null);
                        package.Save(); //储存
                    }
                    Message = "数据删除成功";

                    EditorGUILayout.EndHorizontal();
                    continue;
                }
                for (global::System.Int32 j = 0; j < item1.Length; j++)
                {
                    string item2 = item1[j];
                    data[i][j] = EditorGUILayout.TextField(item2);
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        private void ReadData()
        {
            data = NewLoadExcelPath.LoadExcel();//读取Excel数据
            Message = "数据读取成功";
        }
        private void ReadDataPathChange()
        {
            if (NewLoadExcelPath != OldLoadExcelPath)
            {
                OldLoadExcelPath = NewLoadExcelPath;
                ReadData();
            }
        }

        /// <summary>
        /// 点击文件加载路径,Unity专用
        /// </summary>
        private void ClickFileLoadPath()
        {
            if (isSelectFile == false) return;
            if (Selection.activeObject != null)
            {
                Repaint();
                string path;
                path = AssetDatabase.GetAssetPath(Selection.activeObject);//选择的文件的路径 
                if (path.Contains("xls") || path.Contains("xlsx"))
                {
                    path = path.Split("Assets")[1];
                    NewLoadExcelPath = string.Format($"{Application.dataPath}{path}");
                    ReadDataPathChange();
                }
            }
        }


        private bool IsInteger(string input, out int number)
        {
            //string pattern = @"^-?\d+$";  
            //return IsMatch(input, pattern);  
            if (int.TryParse(input, out int i))
            {
                number = i;
                return true;
            }
            else
            {
                number = 0;
                return false;
            }
        }
        private bool IsFloateger(string input, out float number)
        {
            if (float.TryParse(input, out float i))
            {
                number = i;
                return true;
            }
            else
            {
                number = 0;
                return false;
            }
        }
    }
}
