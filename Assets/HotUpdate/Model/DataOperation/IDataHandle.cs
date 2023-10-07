/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    数据处理

-----------------------*/

namespace Farm2D
{
    public interface IDataHandle
    {
        public abstract void Save(object obj, string fileName);

        public abstract T Load<T>(string fileName) where T : class;
    }
}
