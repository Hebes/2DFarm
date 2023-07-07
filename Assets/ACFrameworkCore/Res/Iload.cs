
using UnityEngine;

namespace ACFrameworkCore
{
    public interface Iload
    {
        /// <summary>
        /// 单独加载
        /// </summary>
        public void Load(string path);

        /// <summary>
        /// 单独加载
        /// </summary>
        public void Load<T>(string path) where T : UnityEngine.Object;

        /// <summary>
        /// 加载全部
        /// </summary>
        public void LoadAll(string path);

        /// <summary>
        /// 异步加载
        /// </summary>

        public void LoadAsync(string path);

        public void LoadAsync<T>(string path) where T : UnityEngine.Object;

    }
}
