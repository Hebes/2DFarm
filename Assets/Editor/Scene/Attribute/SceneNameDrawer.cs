using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    int sceneIndex = -1;
    GUIContent[] sceneNames;
    readonly string[] scenePathSplit = { "/", ".unity" };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (EditorBuildSettings.scenes.Length == 0) return;
        if (sceneIndex == -1)
            GetSceneNameArray(property);
        int oldIndex = sceneIndex;
        sceneIndex = EditorGUI.Popup(position, label, sceneIndex, sceneNames);
        if (oldIndex != sceneIndex)
            property.stringValue = sceneNames[sceneIndex].text;
    }

    private void GetSceneNameArray(SerializedProperty property)
    {
        var scenes = EditorBuildSettings.scenes;
        // 初始化数组
        sceneNames = new GUIContent[scenes.Length];
        for (int i = 0; i < sceneNames.Length; i++)
        {
            string path = scenes[i].path;
            string[] splitPath = path.Split(scenePathSplit, System.StringSplitOptions.RemoveEmptyEntries);
            string sceneName = string.Empty;
            sceneName = splitPath.Length > 0 ? splitPath[splitPath.Length - 1] : "(Deleted Scene)";
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
