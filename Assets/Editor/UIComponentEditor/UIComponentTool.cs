using ACTool;
using UnityEditor;
using UnityEngine;

namespace ACEditor
{
    public class UIComponentTool : EditorWindow
    {
        private string ACHierarchyToolReName_Prefix1 = "T_";

        [MenuItem("Tool/Hierarchy前缀工具#E #E")]
        public static void ShowUIComponentTool()
        {
            if (!EditorWindow.HasOpenInstances<UIComponentTool>())
                GetWindow(typeof(UIComponentTool), false, "Excel数据填充").Show();
            else
                GetWindow(typeof(UIComponentTool)).Close();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("Hierarchy前缀工具", EditorStyles.boldLabel);
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);


            EditorGUILayout.BeginHorizontal();
            {
                ACHierarchyToolReName_Prefix1 = EditorGUILayout.TextField("请输入组件查找前缀", ACHierarchyToolReName_Prefix1);
                if (GUILayout.Button("复制", EditorStyles.miniButtonMid))
                    ACHierarchyToolReName_Prefix1.ACCopyWord();
                if (GUILayout.Button("保存修改", EditorStyles.miniButtonMid))
                    ACCoreExpansion_Find.ACGetObjs().ACSaveModification();
                if (GUILayout.Button("清除", EditorStyles.miniButtonMid))
                    ACHierarchyToolReName_Prefix1 = string.Empty;
            }
            EditorGUILayout.EndHorizontal();



            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("获取前缀", EditorStyles.miniButtonMid))
                    ACHierarchyToolReName_Prefix1 = ACCoreExpansion_Find.ACGetObj().ACGetPrefix();
                if (GUILayout.Button("前缀添加", EditorStyles.miniButtonMid))
                    ACCoreExpansion_Find.ACGetObjs().ACChangePrefixLoop(ACHierarchyToolReName_Prefix1);
                if (GUILayout.Button("去除前缀", EditorStyles.miniButtonMid))
                    ACCoreExpansion_Find.ACGetObjs().ACChangePrefixLoop(ACHierarchyToolReName_Prefix1, false);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
