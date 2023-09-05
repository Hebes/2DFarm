using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//https://www.cnblogs.com/terrynoya/p/16783616.html
namespace ACFrameworkCore
{
    public class ActionAsyncHandler : IUniTaskSource
    {
        //static Action<object> cancellationCallback = CancellationCallback;

        //private CancellationToken cancellationToken;
        //private CancellationTokenRegistration registration;

        //private UniTaskCompletionSourceCore<bool> core;
        //bool isDisposed;
        //bool callOnce;

        public void GetResult(short token)
        {
            throw new NotImplementedException();
        }

        public UniTaskStatus GetStatus(short token)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted(Action<object> continuation, object state, short token)
        {
            throw new NotImplementedException();
        }

        public UniTaskStatus UnsafeGetStatus()
        {
            throw new NotImplementedException();
        }
    }
}
