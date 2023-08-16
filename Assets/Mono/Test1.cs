using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static ACFrameworkCore.Test1;

namespace ACFrameworkCore
{
    public class Test1 : MonoBehaviour
    {
        public delegate UniTask HDF();
        public event HDF hDF;
        public Action actEvent;
        private async UniTaskVoid Start()
        {
            hDF += Remove1;
            hDF += Remove2;
            //actEvent += UniTask.Action(async () =>
            //{
            //    await UniTask.DelayFrame(100);
            //    Debug.Log(1);
            //});

            //actEvent += UniTask.Action(async () =>
            //{
            //    Debug.Log(2);
            //    await UniTask.Yield();
            //});
            //Debug.Log("3");
            ////TODO 代码编写
            //actEvent?.Invoke();
            //Debug.Log("4");

            Debug.Log("1");
            Delegate[] trr = hDF.GetInvocationList();
            await UniTask.WhenAll(Array.ConvertAll(trr, del => ((HDF)del)()));
            Debug.Log("4");

            //3 2 4 1 现在的顺序
            //3 2 1 4 目标的顺序
        }

        private async UniTask Remove1()
        {
            Debug.Log(2);
            await UniTask.Yield();
        }
        private async UniTask Remove2()
        {
            await UniTask.DelayFrame(100);
            Debug.Log(3);
        }

        public void Remove()
        {

        }
    }
}
