using Cysharp.Threading.Tasks;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	模块接口

-----------------------*/

namespace Farm2D
{
    public interface IModelInit
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <returns></returns>
        public UniTask ModelInit();

    }
}
