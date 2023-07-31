using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace ACFrameworkCore
{
    public static class BuildHelper
    {
        private const string relativeDirPrefix = "../Release";

        public static string BuildFolder = "../Release/{0}/StreamingAssets/";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="buildAssetBundleOptions"></param>
        /// <param name="buildOptions"></param>
        /// <param name="isBuildExe">是否打包Exe</param>
        /// <param name="isContainAB"></param>
        /// <param name="clearFolder"></param>
        public static void Build(PlatformType type, BuildAssetBundleOptions buildAssetBundleOptions, BuildOptions buildOptions, bool isBuildExe, bool isContainAB, bool clearFolder)
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;
            string programName = "Test打包";
            string exeName = programName;
            switch (type)
            {
                case PlatformType.PC:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    exeName += ".exe";
                    break;
                case PlatformType.Android:
                    buildTarget = BuildTarget.Android;
                    exeName += ".apk";
                    break;
                case PlatformType.IOS:
                    buildTarget = BuildTarget.iOS;
                    break;
                case PlatformType.MacOS:
                    buildTarget = BuildTarget.StandaloneOSX;
                    break;
            }

            //打包输出地址
            string fold = string.Format(BuildFolder, type);
            if (clearFolder && Directory.Exists(fold))
                Directory.Delete(fold, true);
            Directory.CreateDirectory(fold);


            UnityEngine.Debug.Log("开始资源打包");
            //BuildPipeline.BuildAssetBundles(fold, buildAssetBundleOptions, buildTarget);

            //UnityEngine.Debug.Log("完成资源打包");

            //if (isContainAB)
            //{
            //    FileHelper.CleanDirectory("Assets/StreamingAssets/");
            //    FileHelper.CopyDirectory(fold, "Assets/StreamingAssets/");
            //}

            if (isBuildExe)
            {
                AssetDatabase.Refresh();
                string[] levels = {
                    "Assets/AssetsPackage/Scenes/Init.unity",
                };
                UnityEngine.Debug.Log("开始EXE打包");
                BuildPipeline.BuildPlayer(levels, $"{relativeDirPrefix}/{exeName}", buildTarget, buildOptions);
                UnityEngine.Debug.Log("完成exe打包");
            }
            else
            {
                //if (isContainAB && type == PlatformType.PC)
                //{
                //    string targetPath = Path.Combine(relativeDirPrefix, $"{programName}_Data/StreamingAssets/");
                //    FileHelper.CleanDirectory(targetPath);
                //    Debug.Log($"src dir: {fold}    target: {targetPath}");
                //    FileHelper.CopyDirectory(fold, targetPath);
                //}
            }
        }
    }
}
