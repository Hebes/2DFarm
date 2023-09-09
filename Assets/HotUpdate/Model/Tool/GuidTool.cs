/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	唯一标识符生成

-----------------------*/

namespace ACFarm
{
    public static class GuidTool
    {
        /// <summary>
        /// 生成唯一标识符
        /// </summary>
        /// <returns></returns>
        public static string GenerateGUID()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
