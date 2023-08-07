using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static MyUI.CSPanelAnime;

namespace MyUI
{
#if UNITY_EDITOR
    [CustomEditor(typeof(CSPanelAnime))]
    public class CSPanelAnimeEditor : Editor
    {
        private CSPanelAnime _panelAnime;
        private List<AnimeValue> _liShow;
        private List<AnimeValue> _liHide;
        private void OnEnable()
        {
            _panelAnime = (CSPanelAnime)target;
            if (_panelAnime.LiShowValues == null) _panelAnime.LiShowValues = new List<AnimeValue>();
            _liShow = _panelAnime.LiShowValues;

            if (_panelAnime.LiHideValues == null) _panelAnime.LiHideValues = new List<AnimeValue>();
            _liHide = _panelAnime.LiHideValues;
        }
        public override void OnInspectorGUI()
        {
            ClickButton("��ʾ����", _liShow);
            GUILayout.Space(4);
            ClickButton("���ض���", _liHide);
        }
        /// <summary>
        /// �����ť
        /// </summary>
        /// <param name="showTip">��ť��ʾ</param>
        /// <param name="liAnimes">�����б�</param>
        private void ClickButton(string showTip, List<AnimeValue> liAnimes)
        {
            GUILayout.Label(showTip);
            EditorGUILayout.BeginHorizontal(GUIStyle.none);
            if (GUILayout.Button("���"))
            {
                liAnimes.Add(new AnimeValue());
            }
            if (GUILayout.Button("�Ƴ�"))
            {
                if (liAnimes.Count > 0)
                {
                    int last = liAnimes.Count - 1;
                    AnimeValue removeValue = liAnimes[last];
                    liAnimes.RemoveAt(last);
                    if (removeValue.AnimeType == EnPanelAnimeType.Fade && removeValue.FadeType == EnFadeType.Group)
                        DestroyImmediate(_panelAnime.GetComponent<CanvasGroup>());
                }
            }
            EditorGUILayout.EndHorizontal();
            //��ʾ����
            ShowListValues(liAnimes);

        }
        /// <summary>
        /// ��ʾ�б��еĲ���
        /// </summary>
        /// <param name="liAnimes">��Ӧ���б�</param>
        private void ShowListValues(List<AnimeValue> liAnimes)
        {
            foreach (var item in liAnimes)
            {
                item.AnimeType = (EnPanelAnimeType)EditorGUILayout.EnumPopup("Ч������", item.AnimeType);
                switch (item.AnimeType)
                {
                    case EnPanelAnimeType.Move:
                        ShowMoveValue(item);
                        break;
                    case EnPanelAnimeType.Scale:
                        item.Offset = EditorGUILayout.Vector3Field("���ű���", item.Offset);
                        break;
                    case EnPanelAnimeType.Rotate:
                        item.Offset = EditorGUILayout.Vector3Field("��ת�Ƕ�", item.Offset);
                        break;
                    case EnPanelAnimeType.Size:
                        item.Offset = EditorGUILayout.Vector3Field("�ߴ�����", item.Offset);
                        break;
                    case EnPanelAnimeType.Fade:
                        ShowFadeValue(item);
                        break;
                    default:
                        break;
                }

                item.SpendTime = EditorGUILayout.FloatField("��������", item.SpendTime);
                EditorGUILayout.Space();
            }
        }
        /// <summary>
        /// �ƶ�������ʾ
        /// </summary>
        /// <param name="item"></param>
        private void ShowMoveValue(AnimeValue item)
        {
            item.ValueType = (EnValueType)EditorGUILayout.EnumPopup("ƫ����", item.ValueType);
            switch (item.ValueType)
            {
                case EnValueType.Custom:
                    item.Offset = EditorGUILayout.Vector2Field("", item.Offset);
                    break;
                case EnValueType.Default:
                    item.MoveDir = (EnMoveDir)EditorGUILayout.EnumPopup("�ƶ�����", item.MoveDir);
                    break;
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="item"></param>
        private void ShowFadeValue(AnimeValue item)
        {
            item.FadeType = (EnFadeType)EditorGUILayout.EnumPopup("��������", item.FadeType);
            switch (item.FadeType)
            {
                case EnFadeType.Single:
                    if (_panelAnime.gameObject.TryGetComponent<CanvasGroup>(out CanvasGroup group1))
                    {
                        DestroyImmediate(group1);
                    }
                    
                    break;
                case EnFadeType.Group:
                    if (!_panelAnime.gameObject.TryGetComponent<CanvasGroup>(out CanvasGroup group))
                        _panelAnime.gameObject.AddComponent<CanvasGroup>();
                    break;
                default:
                    break;
            }
            item.Alpha = EditorGUILayout.FloatField("͸����", item.Alpha);
        }
    }
#endif
}
