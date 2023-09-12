using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	删除文件

-----------------------*/

namespace ACEditor
{
    public class DeleteFolder
    {
        [MenuItem("Tool/删除YooAsset打包文件")]
        private static void Run()
        {
            DaleteBundles();
            DaleteYoo();
        }

        private static void DaleteBundles()
        {
            string str = Application.dataPath.Replace("Assets", "Bundles");
            if (Directory.Exists(str)==false)
            {
                Debug.Log("Bundles文件夹不存在");
                return;
            }
            string bundlesPath = $"{str}";
            Directory.Delete(bundlesPath, true);
            Debug.Log("删除Bundles成功");
        }

        private static void DaleteYoo()
        {
            string str = Application.dataPath.Replace("Assets", "Assets/StreamingAssets/yoo");
            if (Directory.Exists(str) == false)
            {
                Debug.Log("Yoo文件夹不存在");
                return;
            }
            string bundlesPath = $"{str}";
            Directory.Delete(bundlesPath, true);
            Debug.Log("删除Yoo成功");
            AssetDatabase.Refresh();
        }
    }
}
