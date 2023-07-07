using System;
using System.Collections.Generic;

namespace ACFrameworkCore
{
    public class LanaguageBridge
    {
        private static LanaguageBridge instance;

        private Dictionary<string, string> LanguageTextKeyDic { get; set; }//语言字典
        public event Action OnLanguageChangeEvt;//回调事件

        public UnityEngine.Font font { get; set; }//字体

        public static LanaguageBridge Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LanaguageBridge();
                    return instance;
                }
                else
                    return instance;
            }
        }

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
            DLog.Error("多语言未配置：" + key);
            return key;
        }
    }
}
