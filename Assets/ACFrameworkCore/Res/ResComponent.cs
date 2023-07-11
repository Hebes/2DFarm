/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    资源加载

-----------------------*/

using UnityEngine.Events;

namespace ACFrameworkCore
{
    public class ResComponent : ICoreComponent
    {
        public static ResComponent Insatance { get; set; }

        private Iload iload;

        public void OnCroeComponentInit()
        {
            Insatance = this;
            iload = new UnityLoad();
        }

        public void OnLoad(string path)
        {
            iload.Load(path);
        }
        public void OnLoad<T>(string path) where T : UnityEngine.Object
        {
            iload.Load<T>(path);
        }
        public void OnLoadAll(string path)
        {
            iload.LoadAll(path);
        }

        public void OnLoadAsync(string path)
        {
            iload.LoadAsync(path);
        }
        public void OnLoadAsync<T>(string path) where T : UnityEngine.Object
        {
            iload.LoadAsync<T>(path);
        }
        public void OnLoadAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
        {
            iload.LoadAsync<T>(path, callback);
        }

    }
}
