using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ACFrameworkCore
{
    public class BuildVersions : EditorWindow
    {
        private string SaveXMLVersion = $"{Application.streamingAssetsPath}/ACPackageVersion.xml";
        private string ServerHost = "127.0.0.1:8000";
        private int PackageVersion;

        [MenuItem("Tool/生成版本号")]//#E
        public static void BuildPackageVersions()
        {
            GetWindow(typeof(BuildVersions));
        }

        private void OnGUI()
        {
            PackageVersion = EditorGUILayout.IntField("请输入版本号:", PackageVersion);
            ServerHost = EditorGUILayout.TextField("请输入服务器地址:", ServerHost);
            EditorGUILayout.LabelField("生成版本号:");
            if (GUILayout.Button("开始生成版本号"))
            {
                string dirPath = Path.GetDirectoryName(SaveXMLVersion);//获取文件的上一级目录
                Directory.Delete(dirPath, true);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                using (FileStream fs = new FileStream(SaveXMLVersion, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"<?xml version=\"1.0\" encoding=\"UTF-8\"?><!-- 更新包版本 -->");
                    sb.AppendLine($"<PackageInfo>");
                    sb.AppendLine($"\t<URL PackageURL=\"{ServerHost}\" Verson=\"{PackageVersion}\" /><!-- 资源包下载地址 -->");
                    sb.AppendLine($"</PackageInfo>");
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(sb);
                    sw.Close();
                    sw.Dispose();
                }
                Debug.Log("生成成功");
            }
        }
    }
}
