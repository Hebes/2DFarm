using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace ACFrameworkCore
{
    public interface ISceneLoad
    {
        /// <summary>
        /// 同步加载场景
        /// </summary>
        public abstract void LoadScene(string SceneName);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        public abstract void LoadSceneAsync(string SceneName, UnityAction unityAction);


        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="SceneName"></param>
        /// <param name="enumerator"></param>
        public void LoadSceneAsync(string SceneName, IEnumerator enumerator);

        /// <summary>
        /// 协程异步加载场景
        /// </summary>
        // abstract void LoadSceneIEnumerator(string SceneName, UnityAction fun);
    }
}
