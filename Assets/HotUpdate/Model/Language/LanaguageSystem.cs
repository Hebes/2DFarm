﻿using Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = Core.Debug;

namespace Farm2D
{
    /// <summary>
    /// 语言类型
    /// </summary>
    public enum ELanguageType
    {
        Chinese = 0,
        English = 1,
    }

    public class LanaguageSystem : SingletonBase<LanaguageSystem>
    {
        private Dictionary<string, string> LanguageTextKeyDic;//语言字典
        public event Action OnLanguageChangeEvt;//回调事件
        public Font font;//字体

        /// <summary>
        /// 切换语言
        /// </summary>
        public void OnLanguageChange()
        {
            OnLanguageChangeEvt?.Invoke();
        }

        /// <summary>
        /// 设置字典
        /// </summary>
        /// <param name="keyValuePairs"></param>
        public void OnSetLanguageTDic(Dictionary<string, string> keyValuePairs)
        {
            LanguageTextKeyDic = keyValuePairs;
        }

        /// <summary>
        /// 获取文字
        /// </summary>
        public string GetText(string key)
        {
            if (LanguageTextKeyDic.ContainsKey(key))
                return LanguageTextKeyDic[key];
            Debug.Error("多语言未配置：" + key);
            return key;
        }
    }
}
