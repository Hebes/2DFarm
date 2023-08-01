using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    单例管理器

-----------------------*/

namespace ACFrameworkCore
{
    public class SingletonManger
    {
        List<IACSinglent<T>> SinglentList = new List<IACSinglent<T>>();

        /// <summary>
        /// 创建单例
        /// </summary>
        //public void CreatSinglent<T>()where T : IACSinglent<T>,new()
        //{
        //    T t=new T();
        //    //t.Instance as T = t;
        //}
    }
}
