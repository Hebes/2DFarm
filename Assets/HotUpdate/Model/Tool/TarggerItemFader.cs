using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    进入触发渐变,树木等

-----------------------*/

namespace Farm2D
{
    public  class TarggerItemFader : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>();
            for (int i = 0; i < faders?.Length; i++)
                faders[i].fadeOut();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>();
            for (int i = 0; i < faders?.Length; i++)
                faders[i].fadeIn();
        }
    }
}
