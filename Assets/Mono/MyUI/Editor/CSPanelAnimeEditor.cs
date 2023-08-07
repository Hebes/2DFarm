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
            ClickButton("显示动画", _liShow);
            GUILayout.Space(4);
            ClickButton("隐藏动画", _liHide);
        }
        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <param name="showTip">按钮提示</param>
        /// <param name="liAnimes">动画列表</param>
        private void ClickButton(string showTip, List<AnimeValue> liAnimes)
        {
            GUILayout.Label(showTip);
            EditorGUILayout.BeginHorizontal(GUIStyle.none);
            if (GUILayout.Button("添加"))
            {
                liAnimes.Add(new AnimeValue());
            }
            if (GUILayout.Button("移除"))
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
            //显示参数
            ShowListValues(liAnimes);

        }
        /// <summary>
        /// 显示列表中的参数
        /// </summary>
        /// <param name="liAnimes">对应的列表</param>
        private void ShowListValues(List<AnimeValue> liAnimes)
        {
            foreach (var item in liAnimes)
            {
                item.AnimeType = (EnPanelAnimeType)EditorGUILayout.EnumPopup("效果类型", item.AnimeType);
                switch (item.AnimeType)
                {
                    case EnPanelAnimeType.Move:
                        ShowMoveValue(item);
                        break;
                    case EnPanelAnimeType.Scale:
                        item.Offset = EditorGUILayout.Vector3Field("缩放比例", item.Offset);
                        break;
                    case EnPanelAnimeType.Rotate:
                        item.Offset = EditorGUILayout.Vector3Field("旋转角度", item.Offset);
                        break;
                    case EnPanelAnimeType.Size:
                        item.Offset = EditorGUILayout.Vector3Field("尺寸增量", item.Offset);
                        break;
                    case EnPanelAnimeType.Fade:
                        ShowFadeValue(item);
                        break;
                    default:
                        break;
                }

                item.SpendTime = EditorGUILayout.FloatField("动画长度", item.SpendTime);
                EditorGUILayout.Space();
            }
        }
        /// <summary>
        /// 移动参数显示
        /// </summary>
        /// <param name="item"></param>
        private void ShowMoveValue(AnimeValue item)
        {
            item.ValueType = (EnValueType)EditorGUILayout.EnumPopup("偏移量", item.ValueType);
            switch (item.ValueType)
            {
                case EnValueType.Custom:
                    item.Offset = EditorGUILayout.Vector2Field("", item.Offset);
                    break;
                case EnValueType.Default:
                    item.MoveDir = (EnMoveDir)EditorGUILayout.EnumPopup("移动方向", item.MoveDir);
                    break;
            }
        }

        /// <summary>
        /// 渐变参数
        /// </summary>
        /// <param name="item"></param>
        private void ShowFadeValue(AnimeValue item)
        {
            item.FadeType = (EnFadeType)EditorGUILayout.EnumPopup("渐变类型", item.FadeType);
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
            item.Alpha = EditorGUILayout.FloatField("透明度", item.Alpha);
        }
    }
#endif
}
