using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACFrameworkCore
{
    public interface Iload
    {
        /// <summary>
        /// 单独加载
        /// </summary>
        public void Load(string path);

        /// <summary>
        /// 加载全部
        /// </summary>
        public void LoadAll(string path);
    }
}
