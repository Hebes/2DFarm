using Farm2D;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EffectsAttribute))]
public class EffectsDrawer : PropertyDrawer
{
    int sceneIndex = -1;
    GUIContent[] sceneNames;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type type = typeof(ConfigEffects);
        FieldInfo[] fields = type.GetFields();
        if (fields.Length == 0) return;
        if (sceneIndex == -1)
        {
            GetSceneNameArray(property, fields);
        }
        int oldIndex = sceneIndex;
        sceneIndex = EditorGUI.Popup(position, label, sceneIndex, sceneNames);
        if (oldIndex != sceneIndex)
            property.stringValue = sceneNames[sceneIndex].text;
    }

    private void GetSceneNameArray(SerializedProperty property, FieldInfo[] fields)
    {
        // 初始化数组
        sceneNames = new GUIContent[fields.Length];
        for (int i = 0; i < sceneNames.Length; i++)
        {
            string sceneName = (string)fields[i].GetValue(fields[i].Name);
            sceneNames[i] = new GUIContent(sceneName);
        }
        if (sceneNames.Length == 0)
        {
            sceneNames = new[] { new GUIContent("Check Your Build Settings") };
        }
        if (!string.IsNullOrEmpty(property.stringValue))//如果有了内容
        {
            bool nameFound = false;
            for (int i = 0; i < sceneNames.Length; i++)
            {
                if (sceneNames[i].text == property.stringValue)
                {
                    sceneIndex = i;
                    nameFound = true;
                    break;
                }
            }
            if (nameFound == false)
                sceneIndex = 0;
        }
        else
        {
            sceneIndex = 0;
        }
        property.stringValue = sceneNames[sceneIndex].text;
    }
}
