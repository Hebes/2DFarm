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
    Excel读取配置表

-----------------------*/

namespace ACFrameworkCore
{
    public class ExcelConfig
    {
        /// <summary>
        /// 开始读取字段的行数
        /// </summary>
        public const int startRowName = 2;

        /// <summary>
        /// 开始读取类型的行数
        /// </summary>
        public const int startRowType = 3;

        /// <summary>
        /// 开始读取描述的行数
        /// </summary>
        public const int startRowDescribe = 4;

        /// <summary>
        /// 开始的列数
        /// </summary>
        public const int startColumns = 2;

        /// <summary>
        /// 真正内容开始的行号
        /// </summary>
        public static int BEGIN_INDEX = 5;
    }
}
