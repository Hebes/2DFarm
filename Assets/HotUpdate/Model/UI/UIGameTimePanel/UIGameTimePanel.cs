using ACFarm;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ACFrameworkCore;

/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    时间显示

-----------------------*/

namespace ACFarm
{
    public class UIGameTimePanel : UIBase
    {
        private RectTransform dayNightImage; //日月转换的
        private RectTransform clockParent;   //时间
        private Text dateText;    //显示日期
        private Text timeText;    //显示12:12
        private Image seasonImage;           //季节的图标
        private Sprite[] seasonSprites;      //季节的图片
        private List<GameObject> clockBlocks;//小时的格子

        public override async void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);
            clockBlocks = new List<GameObject>();

            ACUIComponent UIComponent = panelGameObject.GetComponent<ACUIComponent>();
            GameObject T_TimeText = UIComponent.Get<GameObject>("T_TimeText");
            GameObject T_StopTime = UIComponent.Get<GameObject>("T_StopTime");
            GameObject T_SeasonImage = UIComponent.Get<GameObject>("T_SeasonImage");
            GameObject T_GameDate = UIComponent.Get<GameObject>("T_GameDate");
            GameObject T_Clock = UIComponent.Get<GameObject>("T_Clock");
            GameObject T_DayNightImage = UIComponent.Get<GameObject>("T_DayNightImage");

            dayNightImage = (RectTransform)T_DayNightImage.transform;
            clockParent = (RectTransform)T_Clock.transform;
            dateText = T_GameDate.GetText();
            timeText = T_TimeText.GetText();
            seasonImage = T_SeasonImage.GetImage();

            seasonSprites = new Sprite[4];
            seasonSprites[0] = await ConfigSprites.ui_time_12Png.LoadAsyncUniTask<Sprite>();
            seasonSprites[1] = await ConfigSprites.ui_time_13Png.LoadAsyncUniTask<Sprite>();
            seasonSprites[2] = await ConfigSprites.ui_time_14Png.LoadAsyncUniTask<Sprite>();
            seasonSprites[3] = await ConfigSprites.ui_time_15Png.LoadAsyncUniTask<Sprite>();

            for (int i = 0; i < clockParent.childCount; i++)
            {
                clockBlocks.Add(clockParent.GetChild(i).gameObject);
                clockParent.GetChild(i).SetActive(false);
            }

            ButtonOnClickAddListener(T_StopTime.name, p => { ConfigUIPanel.UISettingPanel.ShwoUIPanel<UISettingPanel>(); });

            ConfigEvent.GameDate.AddEventListener<int, int, int, int, ESeason>(OnGameDateEvent);
            ConfigEvent.GameMinute.AddEventListener<int, int, int, ESeason>(OnGameMinuteEvent);
        }

        /// <summary>
        /// 显示时间
        /// </summary>
        /// <param name="minute"></param>
        /// <param name="hour"></param>
        private void OnGameMinuteEvent(int minute, int hour, int day, ESeason season)
        {
            timeText.text = $"{hour.ToString("00")}:{minute.ToString("00")}";
        }

        /// <summary>
        /// 显示日期和春夏秋冬
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="season"></param>
        private void OnGameDateEvent(int hour, int day, int month, int year, ESeason season)
        {
            dateText.text = $"{year}年{month.ToString("00")}月{day.ToString("00")}日";
            seasonImage.sprite = seasonSprites[(int)season];//切换春夏秋冬
            SwitchHourImage(hour);
            DayNightImageRotate(hour);
        }

        /// <summary>
        /// 显示时间的格子
        /// </summary>
        /// <param name="hour"></param>
        private void SwitchHourImage(int hour)
        {
            int index = hour / 4;
            if (index == 0)//如果是0点的话
            {
                foreach (var item in clockBlocks)
                    item.SetActive(false);
                return;
            }

            for (int i = 0; i < clockBlocks.Count; i++)
                clockBlocks[i].SetActive(i < index + 1);
        }

        /// <summary>
        /// 夜晚等时间图片的切换
        /// </summary>
        /// <param name="hour"></param>
        private void DayNightImageRotate(int hour)
        {
            var target = new Vector3(0, 0, hour * 15 - 90);//保证从黑天开始
            dayNightImage.DORotate(target, 1f, RotateMode.Fast);
        }
    }
}
