using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using YooAsset;

/// <summary>
/// YooAsset流程状态
/// </summary>
public enum EHotUpdateProcess
{
    /// <summary> 流程准备工作 </summary>
    FsmPatchPrepare,
    /// <summary> 初始化资源包 </summary>
    FsmInitialize,
    /// <summary> 更新资源版本号 </summary>
    FsmUpdateVersion,
    /// <summary> 更新资源清单 </summary>
    FsmUpdateManifest,
    /// <summary> 创建文件下载器 </summary>
    FsmCreateDownloader,
    /// <summary> 下载更新文件 </summary>
    FsmDownloadFiles,
    /// <summary> 下载完毕 </summary>
    FsmDownloadOver,
    /// <summary> 清理未使用的缓存文件 </summary>
    FsmClearCache,
    /// <summary> 流程更新完毕 </summary>
    FsmPatchDone,
    /// <summary> HybridCLR热更代码 </summary>
    FsmLoadHotDll,
}

public class Init : MonoBehaviour
{
    /// <summary> 资源系统运行模式 </summary>
	public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
    private EHotUpdateProcess HotUpdateProcess;

    private readonly string packageName = "PC";
    private ResourceDownloaderOperation downloader;//下载器
    public UpdatePackageVersionOperation operation;//更新包

    //补充元数据dll的列表，
    //通过RuntimeApi.LoadMetadataForAOTAssembly()函数来补充AOT泛型的原始元数据
    public static List<string> AOTMetaAssemblyNames { get; } = new List<string>()
    {
        "mscorlib.dll",
        "System.dll",
        "System.Core.dll",
    };
    private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();//资源数据

    private void Awake()
    {
        Debug.Log($"资源系统运行模式：{PlayMode}");
        Application.targetFrameRate = 60;//限定帧数
        Application.runInBackground = true;//是否后台开启
    }
    void Start()
    {
        // 初始化资源系统
        YooAssets.Initialize();
        YooAssets.SetOperationSystemMaxTimeSlice(30);//设置异步系统参数，每帧执行消耗的最大时间切片
        FsmProcessChange(EHotUpdateProcess.FsmPatchPrepare);
    }

    /// <summary>
    /// 状态机流程切换
    /// </summary>
    /// <param name="HotUpdateProcess">流程模式</param>
    private void FsmProcessChange(EHotUpdateProcess HotUpdateProcess)
    {
        this.HotUpdateProcess = HotUpdateProcess;
        switch (HotUpdateProcess)
        {
            case EHotUpdateProcess.FsmPatchPrepare: StartCoroutine(FsmPatchPrepare()); break;
            case EHotUpdateProcess.FsmInitialize: StartCoroutine(FsmInitialize()); break;
            case EHotUpdateProcess.FsmUpdateVersion: StartCoroutine(FsmUpdateVersion()); break;
            case EHotUpdateProcess.FsmUpdateManifest: StartCoroutine(FsmUpdateManifest()); break;
            case EHotUpdateProcess.FsmCreateDownloader: StartCoroutine(FsmCreateDownloader()); break;
            case EHotUpdateProcess.FsmDownloadFiles: StartCoroutine(FsmDownloadFiles()); break;
            case EHotUpdateProcess.FsmDownloadOver: StartCoroutine(FsmDownloadOver()); break;
            case EHotUpdateProcess.FsmClearCache: StartCoroutine(FsmClearCache()); break;
            case EHotUpdateProcess.FsmPatchDone: StartCoroutine(FsmPatchDone()); break;
            case EHotUpdateProcess.FsmLoadHotDll: StartCoroutine(FsmLoadHotDll()); break;
        }
    }

    #region YooAsset流程

    /// <summary>
    /// 流程准备工作
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmPatchPrepare()
    {
        Debug.Log("流程准备工作");
        yield return new WaitForSeconds(1f);
        //TODO 加载更新面板
        FsmProcessChange(EHotUpdateProcess.FsmInitialize);
    }

    /// <summary>
    /// 初始化资源包
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmInitialize()
    {
        Debug.Log("初始化资源包");
        yield return new WaitForSeconds(0.5f);
        // 创建默认的资源包

        var package = YooAssets.TryGetPackage(packageName);
        if (package == null)
        {
            // 创建默认的资源包
            package = YooAssets.CreatePackage(packageName);
        }
        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        YooAssets.SetDefaultPackage(package);

        //进入模式
        InitializationOperation initializationOperation = null;
        switch (PlayMode)
        {
            case EPlayMode.EditorSimulateMode://编辑器下的模拟模式,在编辑器下，不需要构建资源包，来模拟运行游戏。
                var initParametersEditorSimulateMode = new EditorSimulateModeParameters();
                initParametersEditorSimulateMode.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(packageName);
                initializationOperation = package.InitializeAsync(initParametersEditorSimulateMode);
                break;
            case EPlayMode.OfflinePlayMode://单机模式,离线运行模式,对于不需要热更新资源的游戏，可以使用单机运行模式。
                var initParametersOfflinePlayMode = new OfflinePlayModeParameters();
                initParametersOfflinePlayMode.DecryptionServices = new GameDecryptionServices();
                initializationOperation = package.InitializeAsync(initParametersOfflinePlayMode);
                break;
            case EPlayMode.HostPlayMode://联机运行模式,对于需要热更新资源的游戏，可以使用联机运行模式，该模式下初始化参数会很多。
                string defaultHostServer = GetHostServerURL();
                string fallbackHostServer = GetHostServerURL();
                var createParameters = new HostPlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                createParameters.QueryServices = new GameQueryServices();
                createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                initializationOperation = package.InitializeAsync(createParameters);
                break;
        }

        yield return initializationOperation;
        if (initializationOperation.Status != EOperationStatus.Succeed)
        {
            Debug.LogError($"资源包初始化失败：{initializationOperation.Error}");
            ProcessBreak();
            yield break;
        }
        Debug.Log("资源包初始化成功！");
        FsmProcessChange(EHotUpdateProcess.FsmUpdateVersion);
    }

    /// <summary>
    /// 更新资源版本号
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmUpdateVersion()
    {
        Debug.Log("更新资源版本号");
        yield return new WaitForSecondsRealtime(0.5f);
        var package = YooAssets.GetPackage(packageName);//获取包
        operation = package.UpdatePackageVersionAsync();
        yield return operation;
        if (operation.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning($"更新资源版本号失败: {operation.Error}");
            ProcessBreak();
            yield break;
        }
        Debug.Log($"更新补丁清单成功,远端或本地最新版本为: {operation.PackageVersion}");
        FsmProcessChange(EHotUpdateProcess.FsmUpdateManifest);
    }

    /// <summary>
    /// 更新资源清单
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmUpdateManifest()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        var package = YooAssets.GetPackage(packageName);//获取包
        bool savePackageVersion = true;

        var operationResource = package.UpdatePackageManifestAsync(operation.PackageVersion, savePackageVersion);
        yield return operationResource;

        if (operationResource.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning($"更新资源清单失败: {operationResource.Error}");
            ProcessBreak();
            yield break;
        }
        Debug.Log($"更新资源清单成功: {operationResource.Status}!");
        FsmProcessChange(EHotUpdateProcess.FsmCreateDownloader);
    }

    /// <summary>
    /// 创建文件下载器
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmCreateDownloader()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        var package = YooAssets.GetPackage(packageName);//获取包

        yield return new WaitForSecondsRealtime(0.5f);

        int downloadingMaxNum = 10;//下载最大值
        int failedTryAgain = 3;//重试失败次数
        int timeout = 60;//超时时间
        downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain, timeout);

        //没有需要下载的资源
        if (downloader.TotalDownloadCount == 0)
        {
            Debug.Log("没有找到需要下载的文件! Not found any download files !");
            FsmProcessChange(EHotUpdateProcess.FsmDownloadOver);
            yield break;
        }
        Debug.Log($"找到需要下载{downloader.TotalDownloadCount}的文件! Found total {downloader.TotalDownloadCount} files that need download ！");
        // 发现新更新文件后，挂起流程系统
        // 注意：开发者需要在下载前检测磁盘空间不足
        //需要下载的文件总数和总大小
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;
        //TODO 开发者需要在下载前检测磁盘空间不足
        FsmProcessChange(EHotUpdateProcess.FsmDownloadFiles);
    }

    /// <summary>
    /// 下载更新文件
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmDownloadFiles()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        //注册回调方法
        downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
        downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
        downloader.OnDownloadOverCallback = OnDownloadOverFunction;
        downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;

        //开启下载
        downloader.BeginDownload();
        yield return downloader;

        //检测下载结果
        if (downloader.Status != EOperationStatus.Succeed)
        {
            Debug.LogError($"更新失败！{downloader.Error}");
            ProcessBreak();
            yield break;
        }
        Debug.Log("更新完成!");
        FsmProcessChange(EHotUpdateProcess.FsmPatchDone);
    }

    /// <summary>
    /// 下载完毕
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmDownloadOver()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        FsmProcessChange(EHotUpdateProcess.FsmClearCache);
    }

    /// <summary>
    /// 清理未使用的缓存文件
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmClearCache()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        var package = YooAssets.GetPackage(packageName);
        var operation = package.ClearUnusedCacheFilesAsync();
        operation.Completed += OnClearCacheFunction;
        FsmProcessChange(EHotUpdateProcess.FsmPatchDone);
    }

    /// <summary>
    /// 流程更新完毕
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmPatchDone()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        FsmProcessChange(EHotUpdateProcess.FsmLoadHotDll);
    }

    /// <summary>
    /// HybridCLR热更代码
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmLoadHotDll()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        var package = YooAssets.GetPackage(packageName);

        //var assets = new List<string>
        //{
        //    "HotUpdate.dll",
        //}.Concat(AOTMetaAssemblyNames);

        //foreach (var asset in assets)
        //{
        //    RawFileOperationHandle handle = package.LoadRawFileAsync(asset);
        //    yield return handle;
        //    byte[] fileData = handle.GetRawFileData();
        //    s_assetDatas[asset] = fileData;
        //    Debug.Log($"dll名称是:{asset}  size大小为:{fileData.Length}");
        //}
        //LoadMetadataForAOTAssemblies();

#if !UNITY_EDITOR
        //System.Reflection.Assembly.Load(GetAssetData("HotUpdate.dll"));
        System.Reflection.Assembly.Load(GetAssetData("HotUpdate.dll"));
#endif
        //补充元数据
        foreach (var asset in AOTMetaAssemblyNames)
        {
            RawFileOperationHandle handle = package.LoadRawFileAsync(asset);
            yield return handle;
            byte[] fileData = handle.GetRawFileData();
            s_assetDatas[asset] = fileData;
            Debug.Log($"dll名称是:{asset}  size大小为:{fileData.Length}");
        }

        LoadMetadataForAOTAssemblies();
        //执行热更新代码
        yield return null;
        RawFileOperationHandle handleHotUpdate = package.LoadRawFileAsync("HotUpdate.dll");
        yield return handleHotUpdate;
        byte[] fileDataHotUpdate = handleHotUpdate.GetRawFileData();
        Debug.Log($"dll名称是:{"HotUpdate.dll"}  size大小为:{fileDataHotUpdate.Length}");
        Assembly _hotUpdateAss = Assembly.Load(fileDataHotUpdate);
        Type entryType = _hotUpdateAss.GetType("InitGame");
        entryType.GetMethod("Init").Invoke(null, null);
    }

    #endregion

    #region 辅助功能

    /// <summary>
    /// 获取资源服务器地址
    /// </summary>
    private string GetHostServerURL()
    {
        //TODO 后续会改成XML读取配置
        //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
        string hostServerIP = "http://127.0.0.1:8000";
        string appVersion = "1";

#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return $"{hostServerIP}/CDN/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/StandaloneWindows64/PC/{appVersion}";
        //return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
		if (Application.platform == RuntimePlatform.Android)
			return $"{hostServerIP}/CDN/Android/{appVersion}";
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			return $"{hostServerIP}/CDN/IPhone/{appVersion}";
		else if (Application.platform == RuntimePlatform.WebGLPlayer)
			return $"{hostServerIP}/CDN/WebGL/{appVersion}";
		else
			return $"{hostServerIP}/StandaloneWindows64/PC/{appVersion}";
#endif
    }

    /// <summary>
    /// 流程跳出
    /// </summary>
    private void ProcessBreak()
    {
        Debug.Log($"结束中断,中断点:{HotUpdateProcess}");
    }

    /// <summary>
    /// 下载出错
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="error"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnDownloadErrorFunction(string fileName, string error)
    {
        Debug.LogError(string.Format("下载出错：文件名：{0}, 错误信息：{1}", fileName, error));
    }

    /// <summary>
    /// 更新中
    /// </summary>
    /// <param name="totalDownloadCount"></param>
    /// <param name="currentDownloadCount"></param>
    /// <param name="totalDownloadBytes"></param>
    /// <param name="currentDownloadBytes"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
    {
        Debug.Log(string.Format("文件总数：{0}, 已下载文件数：{1}, 下载总大小：{2}, 已下载大小：{3}", totalDownloadCount, currentDownloadCount, totalDownloadBytes, currentDownloadBytes));
    }

    /// <summary>
    /// 开始下载
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="sizeBytes"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnStartDownloadFileFunction(string fileName, long sizeBytes)
    {
        Debug.Log(string.Format("开始下载：文件名：{0}, 文件大小：{1}", fileName, sizeBytes));
    }

    /// <summary>
    /// 下载完成
    /// </summary>
    /// <param name="isSucceed"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnDownloadOverFunction(bool isSucceed)
    {
        Debug.Log("下载" + (isSucceed ? "成功" : "失败"));
    }

    /// <summary>
    /// 清理缓存
    /// </summary>
    private void OnClearCacheFunction(AsyncOperationBase asyncOperationBase)
    {
        Debug.Log("清理缓存完毕,流程更新完毕");
    }

    /// <summary>
    /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    /// </summary>
    private static void LoadMetadataForAOTAssemblies()
    {
        /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        /// 
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyNames)
        {
            byte[] dllBytes = GetAssetData(aotDllName);
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }

    /// <summary>
    /// 获取byte字节数组
    /// </summary>
    /// <param name="dllName"></param>
    /// <returns></returns>
    private static byte[] GetAssetData(string dllName)
    {
        return s_assetDatas[dllName];
    }

    #endregion
}
