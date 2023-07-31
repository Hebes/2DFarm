
/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	单例接口

-----------------------*/

namespace UniFramework.Singleton
{
	public interface ISingleton
	{
		/// <summary>
		/// 创建单例
		/// </summary>
		void OnCreate(System.Object createParam);

		/// <summary>
		/// 更新单例
		/// </summary>
		void OnUpdate();

		/// <summary>
		/// 销毁单例
		/// </summary>
		void OnDestroy();
	}
}