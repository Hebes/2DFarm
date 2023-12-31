﻿using System;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	计划的安排

-----------------------*/

namespace Farm2D
{
    [Serializable]
    public class ScheduleDetails : IComparable<ScheduleDetails>
    {
        public string targetScene;
        public int hour, minute, day;
        public int priority;    //优先级越小优先执行
        public ESeason season;
        public Vector2Int targetGridPosition;
        public AnimationClip clipAtStop;
        public bool interactable;

        public ScheduleDetails(int hour, int minute, int day, 
            int priority, ESeason season, string targetScene, 
            Vector2Int targetGridPosition, AnimationClip clipAtStop, bool interactable)
        {
            this.hour = hour;
            this.minute = minute;
            this.day = day;
            this.priority = priority;
            this.season = season;
            this.targetScene = targetScene;
            this.targetGridPosition = targetGridPosition;
            this.clipAtStop = clipAtStop;
            this.interactable = interactable;
        }
        public int Time => (hour * 100) + minute;

        public int CompareTo(ScheduleDetails other)
        {
            if (Time == other.Time)
            {
                if (priority > other.priority)
                    return 1;
                else
                    return -1;
            }
            else if (Time > other.Time)
            {
                return 1;
            }
            else if (Time < other.Time)
            {
                return -1;
            }
            return 0;
        }
    }
}
