using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ACFrameworkCore
{
    public class LanguageText : MonoBehaviour
    {
        public string key = "错误";

        private Text m_Text;
        private TMP_Text m_MeshText;
        private void Awake()
        {
            m_Text = GetComponent<Text>();
            m_MeshText = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            OnSwitchLanguage();
            LanaguageSystem.Instance.OnLanguageChangeEvt += OnSwitchLanguage;
        }

        private void OnDisable()
        {
            LanaguageSystem.Instance.OnLanguageChangeEvt -= OnSwitchLanguage;
        }

        private void OnSwitchLanguage()
        {
            if (m_Text != null)
            {
                m_Text.font = LanaguageSystem.Instance.font;
                m_Text.text = LanaguageSystem.Instance.GetText(key);
            }

            if (m_MeshText != null)
            {
                m_MeshText.text = LanaguageSystem.Instance.GetText(key);
            }
        }

        /// <summary>
        /// 设置Key和切换语言
        /// </summary>
        /// <param name="key">关键词</param>
        public void OnSetKeyAndChange(string key)
        {
            this.key = key;
            OnSwitchLanguage();
        }
    }
}
