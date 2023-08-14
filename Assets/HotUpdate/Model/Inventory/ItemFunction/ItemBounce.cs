using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    物品抛出

-----------------------*/

namespace ACFrameworkCore
{
    public class ItemBounce : MonoBehaviour
    {
        private Transform spriteTrans => transform.GetChild(0);
        private BoxCollider2D coll => GetComponent<BoxCollider2D>();

        public float gravity = -3.5f;
        private bool isGround;
        private float distance;
        private Vector2 direction;
        private Vector3 targetPos;

        private void Awake() => coll.enabled = false;
        private void Update() => Bounce();

        public void InitBounceItem(Vector3 target, Vector2 dir)
        {
            coll.enabled = false;
            direction = dir;
            targetPos = target;
            distance = Vector3.Distance(target, transform.position);

            spriteTrans.position += Vector3.up * 1.84f;//1.84出自PersistentScene的player的HoldItem
        }

        /// <summary>
        /// 物品抛出
        /// </summary>
        private void Bounce()
        {
            isGround = spriteTrans.position.y <= transform.position.y;

            if (Vector3.Distance(transform.position, targetPos) > 0.1f)
                transform.position += (Vector3)direction * distance * -gravity * Time.deltaTime;

            if (!isGround)
            {
                spriteTrans.position += Vector3.up * gravity * Time.deltaTime;
            }
            else
            {
                spriteTrans.position = transform.position;
                coll.enabled = true;
            }
        }
    }
}
