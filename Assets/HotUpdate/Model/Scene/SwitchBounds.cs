using Cinemachine;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    切换场景边界

-----------------------*/

namespace ACFrameworkCore
{
    public class SwitchBounds : MonoBehaviour
    {

        private void Awake()
        {
            ConfigEvent.SwichConfinerShape.AddEventListener(SwichConfinerShape);
        }

        /// <summary>
        /// 切换场景的时候找到 限定范围的组件
        /// </summary>
        private void SwichConfinerShape()
        {
            PolygonCollider2D ConfinerShape = GameObject.FindGameObjectWithTag(ConfigTag.TagBoundsConfiner).GetComponent<PolygonCollider2D>();
            CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
            confiner.m_BoundingShape2D = ConfinerShape;
            //Call this if the bounding shape's points change at runtime
            //如果边界形状的点在运行时改变，调用这个函数
            confiner.InvalidatePathCache();
        }
    }
}
