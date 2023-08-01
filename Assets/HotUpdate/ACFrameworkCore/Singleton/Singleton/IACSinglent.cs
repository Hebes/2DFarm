/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    单例接口(不是)

-----------------------*/

namespace ACFrameworkCore
{
    public interface IACSinglent<T> where T : class, new()
    {
        public static T instance { get; set; }

        public T GetInstance() 
        {
            if (instance == null)
                instance = new T();
            return instance;
        }

        //public virtual T Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //            instance= new T();
        //        return instance;
        //    }
        //}
    }
}
