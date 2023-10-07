using UnityEditor;
using UnityEngine;

namespace Farm2D
{
    public class OpenFolder
    {
        [MenuItem("Tool/打开ApplicationpersistentDataPath")]
        private static void Run()
        {
            //System.Diagnostics.Process.Start("explorer.exe", Application.persistentDataPath);
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}
