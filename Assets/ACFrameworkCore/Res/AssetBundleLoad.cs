using UnityEngine;
using UnityEngine.Events;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    AB包加载

-----------------------*/

namespace ACFrameworkCore
{
    public class AssetBundleLoad : Iload
    {
        public void Load(string path)
        {
        }

        public void Load<T>(string path) where T : Object
        {
        }

        public void LoadAll(string path)
        {
        }

        public void LoadAsync(string path)
        {
        }

        public void LoadAsync<T>(string path) where T : UnityEngine.Object
        {
        }

        public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
        {
        }
    }
}
