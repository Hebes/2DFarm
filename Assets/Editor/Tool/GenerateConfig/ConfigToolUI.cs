using Farm2D;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ACEditor
{
    public class ConfigToolUI : EditorWindow
    {
        private static string CommonPath = $"{Application.dataPath}\\";
        private string CreatPath = $"{Application.dataPath}/HotUpdate/Model/Config/GenerateConofig";
        private string content;
        private string LoadPath = $"{CommonPath}/Resources/";
        private Vector2 scrollPosition;

        [MenuItem("Tool/生成配置文件#C #C")]
        public static void ShowConfigToolUI()
        {
            if (!EditorWindow.HasOpenInstances<ConfigToolUI>())
                GetWindow(typeof(ConfigToolUI), false, "生成配置文件").Show();
            else
                GetWindow(typeof(ConfigToolUI)).Close();
        }


        //[MenuItem("Tool/GenerateConfig/生成Prefab配置文件")]//#E
        //[MenuItem("Tool/GenerateConfig/生成UIPanel配置文件")]//#E
        //[MenuItem("Tool/GenerateConfig/生成Scenes配置文件")]//#E
        //[MenuItem("Tool/GenerateConfig/生成SortingLayer配置文件")]//#E
        //[MenuItem("Tool/GenerateConfig/生成bytes配置文件")]//#E
        //[MenuItem("Tool/GenerateConfig/生成Sprites配置文件")]//#E
        //[MenuItem("Tool/GenerateConfig/生成Animations配置文件")]//#E
        //[MenuItem("Tool/GenerateConfig/生成Effects配置文件")]//#E
        //[MenuItem("Tool/GenerateConfig/生成Sound配置文件")]//#E


        private void OnGUI()
        {
            Repaint();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Tag数据预览"))
            {
                content = GenerateConfigTool.ReadTagData();
            }
            if (GUILayout.Button("生成Tag数据文件"))
            {
                content = GenerateConfigTool.ReadTagData();
                string creatFilePath = $"{CreatPath}/ConfigTag.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();



            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Layer数据预览"))
            {
                content = GenerateConfigTool.ReadLayerData();
            }
            if (GUILayout.Button("生成Layer数据文件"))
            {
                content = GenerateConfigTool.ReadLayerData();
                string creatFilePath = $"{CreatPath}/ConfigLayer.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();



            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Prefab数据预览(全路径)"))
            {
                string LoadPath = $"{CommonPath}";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigPrefab", DataReadType.CommonNoSuffix, ".prefab");
            }
            if (GUILayout.Button("生成Prefab数据文件(全路径)"))
            {
                string LoadPath = $"{CommonPath}";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigPrefab", DataReadType.CommonNoSuffix, ".prefab");
                string creatFilePath = $"{CreatPath}/ConfigPrefab.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();



            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Material数据预览(全路径)"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigMaterial", DataReadType.AllPathSuffixation, ".mat");
            }
            if (GUILayout.Button("生成Material数据文件(全路径)"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigMaterial", DataReadType.AllPathSuffixation, ".mat");
                string creatFilePath = $"{CreatPath}/ConfigMaterial.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();








            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成SortingLayer数据预览"))
            {
                content = GenerateConfigTool.ReadSortingLayerData();
            }
            if (GUILayout.Button("生成SortingLayer数据文件"))
            {
                content = GenerateConfigTool.ReadSortingLayerData();
                string creatFilePath = $"{CreatPath}/ConfigSortingLayer.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Scenes数据预览"))
            {
                string LoadPath = $"{CommonPath}/Scenes/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigScenes", DataReadType.CommonSuffixation, ".unity");
            }
            if (GUILayout.Button("生成Scenes数据文件"))
            {
                string LoadPath = $"{CommonPath}/Scenes/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigScenes", DataReadType.CommonSuffixation, ".unity");
                string creatFilePath = $"{CreatPath}/ConfigScenes.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Audio数据预览"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigAudio", DataReadType.AllPathNoSuffix, ".mp3", ".wav");
            }
            if (GUILayout.Button("生成Audio数据文件"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigAudio", DataReadType.AllPathNoSuffix, ".mp3", ".wav");
                string creatFilePath = $"{CreatPath}/ConfigAudio.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            GUILayout.Space(5f);
            //EditorGUILayout.LabelField("预览", EditorStyles.label);
            //isOpenPreview = EditorGUILayout.ToggleLeft("是否开启预览", isOpenPreview, GUILayout.Width(130f));
            //if (isOpenPreview && !string.IsNullOrEmpty(content))
            //    EditorGUILayout.TextArea(content);

            // 创建一个可滚动的区域
            EditorGUILayout.LabelField("预览", EditorStyles.label);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(700f));
            if (!string.IsNullOrEmpty(content))
                content = EditorGUILayout.TextArea(content, GUILayout.ExpandHeight(true)); // 显示和编辑多行文本的TextArea
            EditorGUILayout.EndScrollView();
        }
    }
}
