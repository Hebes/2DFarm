using ACFrameworkCore;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    鼠标样式管理

-----------------------*/


namespace ACFarm
{
    public class MouseManagerSystem : ICore
    {
        public static MouseManagerSystem Instance;
        private List<Sprite> MouseSpriteList;           //鼠标图标列表

        public void ICroeInit()
        {
            Instance = this;
            MouseSpriteList = new List<Sprite>();
            List<string> MouseSpriteTempList = new List<string>()
            {
                ConfigSprites.cursor11Png,
                ConfigSprites.cursor8Png,
                ConfigSprites.cursor7Png,
                ConfigSprites.cursor3Png,
            };

            //加载UI图标
            foreach (string spriteName in MouseSpriteTempList)
            {
                Sprite MouseSprite = ResourceExtension.Load<Sprite>(spriteName);
                MouseSpriteList.Add(MouseSprite);
            }
        }

        public Sprite GetMouseSprite(string mouseName)
        {
            return MouseSpriteList.Find(p => { return p.name == mouseName; });
        }
    }
}
