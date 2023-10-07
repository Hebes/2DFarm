using Core;
using Cysharp.Threading.Tasks;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	牧场数据初始化管理系统

-----------------------*/

namespace Farm2D
{
    public class DataManagerSystem : IModelInit
    {
        public static DataManagerSystem Instance;
        private CoreData dataManager;

        public async UniTask ModelInit()
        {
            Instance = this;
            dataManager = CoreData.Instance;
            await UniTask.Yield();
        }
    }
}
