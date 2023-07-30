using YooAsset;

/// <summary>
/// 远端资源地址查询服务类
/// </summary>
public class RemoteServices : IRemoteServices
{
    /// <summary>
    /// 默认远端地址
    /// </summary>
    private readonly string _defaultHostServer;
    /// <summary>
    /// 备用远端地址
    /// </summary>
    private readonly string _fallbackHostServer;

    public RemoteServices(string defaultHostServer, string fallbackHostServer)
    {
        _defaultHostServer = defaultHostServer;
        _fallbackHostServer = fallbackHostServer;
    }
    string IRemoteServices.GetRemoteFallbackURL(string fileName)
    {
        return $"{_defaultHostServer}/{fileName}";
    }
    string IRemoteServices.GetRemoteMainURL(string fileName)
    {
        return $"{_fallbackHostServer}/{fileName}";
    }
}