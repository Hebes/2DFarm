using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using UnityEditor;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    打包DLL

-----------------------*/

namespace ACFrameworkCore
{
    public class BuildEditor : EditorWindow
    {
        //[MenuItem("Tool/克隆初始化项目Alt+Q")]//#E
        //public static void InitProject()
        //{
        //    //请先HybridCLR/Install查看是否安装
        //    //点击生成HybridCLR/Generate/ALl(报错如果显示先打包,请先打包,其他报错请联系作者)
        //    //打包:HybridCLR/Build/Win64
        //}

        [MenuItem("Tool/快速打包热更DLl _F5")]
        public static void BuildDLL()
        {
            CompileDllCommand.CompileDllActiveBuildTarget();
            BuildAssetsCommand.BuildAndCopyABAOTHotUpdateDlls();
        }
    }
}
