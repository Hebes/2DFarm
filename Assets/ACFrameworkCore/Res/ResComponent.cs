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
    资源加载

-----------------------*/

namespace ACFrameworkCore
{
    public class ResComponent : ICoreComponent
    {
        public static ResComponent Insatance { get; set; }
        private Iload iload;

        public void OnCroeComponentInit()
        {
            Insatance = this;
            iload = new UnityLoad();
        }

        public void OnLoad(string path)
        {
            iload.Load(path);
        }

        public void OnLoadAll(string path)
        {
            iload.LoadAll(path);
        }
    }
}
