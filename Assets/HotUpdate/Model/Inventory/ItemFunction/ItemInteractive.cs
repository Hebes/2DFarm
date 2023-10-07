using Core;
using System.Collections;
using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	物品互动摇晃

-----------------------*/

namespace Farm2D
{
    public class ItemInteractive : MonoBehaviour
    {
        private bool isAnimating;
        private WaitForSeconds pause = new WaitForSeconds(0.04f);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isAnimating)
            {
                if (other.transform.position.x < transform.position.x)
                    StartCoroutine(RotateRight());//对方在左侧 向右摇晃
                else
                    StartCoroutine(RotateLeft());//对方在右侧 向左摇晃
                ConfigEvent.PlaySoundEvent.EventTrigger(ESoundName.Rustle);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //TODO 玩家和NPC场景不同 NPC 触发时才会报错
            if (gameObject.activeInHierarchy == false) return;
            if (!isAnimating)
            {
                if (other.transform.position.x > transform.position.x)
                    StartCoroutine(RotateRight()); //对方在左侧 向右摇晃
                else
                    StartCoroutine(RotateLeft());//对方在右侧 向左摇晃
                ConfigEvent.PlaySoundEvent.EventTrigger(ESoundName.Rustle);
            }
        }

        private IEnumerator RotateLeft()
        {
            isAnimating = true;

            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(0).Rotate(0, 0, 2);
                yield return pause;
            }
            for (int i = 0; i < 5; i++)
            {
                transform.GetChild(0).Rotate(0, 0, -2);
                yield return pause;
            }
            transform.GetChild(0).Rotate(0, 0, 2);
            yield return pause;

            isAnimating = false;
        }

        private IEnumerator RotateRight()
        {
            isAnimating = true;

            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(0).Rotate(0, 0, -2);
                yield return pause;
            }
            for (int i = 0; i < 5; i++)
            {
                transform.GetChild(0).Rotate(0, 0, 2);
                yield return pause;
            }
            transform.GetChild(0).Rotate(0, 0, -2);
            yield return pause;

            isAnimating = false;
        }
    }
}
