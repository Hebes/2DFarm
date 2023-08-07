using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static MyUI.MyButton;

namespace MyUI
{
#if UNITY_EDITOR
    [CustomEditor(typeof(MyButton))]
    public class MyButtonEditor : Editor
    {
        private MyButton btnType;
        private AnimeValue MouseEnterValue, MouseClickValue;
        public override void OnInspectorGUI()
        {
            Init();
            MouseEnterValue.EnAnimeType = (EnButtonAnimeType)EditorGUILayout.EnumPopup("鼠标进入效果", MouseEnterValue.EnAnimeType);
            MouseEnterValue = ShowValue(MouseEnterValue);
            EditorGUILayout.Space();
            MouseClickValue.EnAnimeType = (EnButtonAnimeType)EditorGUILayout.EnumPopup("鼠标点击效果", MouseClickValue.EnAnimeType);
            MouseClickValue = ShowValue(MouseClickValue);
            EditorGUILayout.Space();
        }

        private void Init()
        {
            btnType = (MyButton)target;
            MouseEnterValue = btnType.MouseEnterValue;//鼠标进入事件
            MouseClickValue = btnType.MouseClickValue;//鼠标点击事件
        }
        /// <summary>
        /// 根据类型显示数值
        /// </summary>
        /// <param name="onEnum"></param>
        /// <param name="mouseValue"></param>
        /// <returns></returns>
        private AnimeValue ShowValue(AnimeValue mouseValue)
        {
            switch (mouseValue.EnAnimeType)
            {
                case EnButtonAnimeType.Scale://缩放动画
                    mouseValue.NewScale = EditorGUILayout.Vector3Field("新尺寸", mouseValue.NewScale);
                    mouseValue.SpendTime = EditorGUILayout.FloatField("动画长度", mouseValue.SpendTime);
                    break;
                case EnButtonAnimeType.Move://移动动画
                    mouseValue.NewPosition = EditorGUILayout.Vector3Field("新坐标", mouseValue.NewPosition);
                    mouseValue.SpendTime = EditorGUILayout.FloatField("动画长度", mouseValue.SpendTime);
                    break;
                case EnButtonAnimeType.Rotate://旋转动画
                    mouseValue.NewAngle = EditorGUILayout.Vector3Field("新角度", mouseValue.NewAngle);
                    mouseValue.SpendTime = EditorGUILayout.FloatField("动画长度", mouseValue.SpendTime);
                    break;
                case EnButtonAnimeType.Outline://边缘描边效果
                    mouseValue.BorderSize = EditorGUILayout.FloatField("宽度", mouseValue.BorderSize);
                    mouseValue.BorderColor = EditorGUILayout.ColorField("描边颜色", mouseValue.BorderColor);
                    mouseValue.UseGraphicAlpha = EditorGUILayout.Toggle("跟随图片透明度", mouseValue.UseGraphicAlpha);
                    break;
                default:
                    break;
            }
            return mouseValue;
        }
    }
#endif
}

