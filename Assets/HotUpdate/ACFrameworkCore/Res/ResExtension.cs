/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    加载拓展类

-----------------------*/

namespace ACFrameworkCore
{
    public static  class ResExtension
    {

        public static void OnLoad<T>(this string path) where T : UnityEngine.Object
        {
            //ResComponent.Insatance.OnLoad<T>(path);

        }

        //public static void OnLoadAll(string path)
        //{
        //    ResComponent.Insatance.OnLoadAll(path);
        //}

        //public static void OnLoadAsync(string path)
        //{
        //    ResComponent.Insatance.OnLoadAsync(path);
        //}
        //public static void OnLoadAsync<T>(string path) where T : UnityEngine.Object
        //{
        //    ResComponent.Insatance.OnLoadAsync<T>(path);
        //}
    }
}
