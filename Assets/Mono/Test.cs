using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ACFrameworkCore
{
    public class Test : MonoBehaviour
    {

        async UniTaskVoid Start()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2), ignoreTimeScale: false);
            Debug.Log("等待了2秒执行");
            Debug.Log("开始执行");
            ttt().Forget();
            Debug.Log("执行l");
        }

        public async UniTaskVoid ttt()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2), ignoreTimeScale: false);
            Debug.Log("等待了2秒执行");
        }
    }
}
