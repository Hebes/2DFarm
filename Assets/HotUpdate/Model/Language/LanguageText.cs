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

            //if (m_Text != null && !string.IsNullOrEmpty(m_Text.text))
            //    key = m_Text.text;
            //if (m_MeshText != null && !string.IsNullOrEmpty(m_MeshText.text))
            //    key = m_MeshText.text;
        }

        private void OnEnable()
        {
            OnSwitchLanguage();
            LanaguageBridge.Instance.OnLanguageChangeEvt += OnSwitchLanguage;
        }

        private void OnDisable()
        {
            LanaguageBridge.Instance.OnLanguageChangeEvt -= OnSwitchLanguage;
        }

        private void OnSwitchLanguage()
        {
            if (m_Text != null)
            {
                m_Text.font = LanaguageBridge.Instance.font;
                m_Text.text = LanaguageBridge.Instance.GetText(key);
            }

            if (m_MeshText != null)
            {
                m_MeshText.text = LanaguageBridge.Instance.GetText(key);
            }
        }

        /// <summary>
        /// 设置Key和切换语言
        /// </summary>
        public void OnSetKeyAndChange(string key)
        {
            this.key = key;
            OnSwitchLanguage();
        }
    }
}
