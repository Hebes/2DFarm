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
            MouseEnterValue.EnAnimeType = (EnButtonAnimeType)EditorGUILayout.EnumPopup("������Ч��", MouseEnterValue.EnAnimeType);
            MouseEnterValue = ShowValue(MouseEnterValue);
            EditorGUILayout.Space();
            MouseClickValue.EnAnimeType = (EnButtonAnimeType)EditorGUILayout.EnumPopup("�����Ч��", MouseClickValue.EnAnimeType);
            MouseClickValue = ShowValue(MouseClickValue);
            EditorGUILayout.Space();
        }

        private void Init()
        {
            btnType = (MyButton)target;
            MouseEnterValue = btnType.MouseEnterValue;//�������¼�
            MouseClickValue = btnType.MouseClickValue;//������¼�
        }
        /// <summary>
        /// ����������ʾ��ֵ
        /// </summary>
        /// <param name="onEnum"></param>
        /// <param name="mouseValue"></param>
        /// <returns></returns>
        private AnimeValue ShowValue(AnimeValue mouseValue)
        {
            switch (mouseValue.EnAnimeType)
            {
                case EnButtonAnimeType.Scale://���Ŷ���
                    mouseValue.NewScale = EditorGUILayout.Vector3Field("�³ߴ�", mouseValue.NewScale);
                    mouseValue.SpendTime = EditorGUILayout.FloatField("��������", mouseValue.SpendTime);
                    break;
                case EnButtonAnimeType.Move://�ƶ�����
                    mouseValue.NewPosition = EditorGUILayout.Vector3Field("������", mouseValue.NewPosition);
                    mouseValue.SpendTime = EditorGUILayout.FloatField("��������", mouseValue.SpendTime);
                    break;
                case EnButtonAnimeType.Rotate://��ת����
                    mouseValue.NewAngle = EditorGUILayout.Vector3Field("�½Ƕ�", mouseValue.NewAngle);
                    mouseValue.SpendTime = EditorGUILayout.FloatField("��������", mouseValue.SpendTime);
                    break;
                case EnButtonAnimeType.Outline://��Ե���Ч��
                    mouseValue.BorderSize = EditorGUILayout.FloatField("���", mouseValue.BorderSize);
                    mouseValue.BorderColor = EditorGUILayout.ColorField("�����ɫ", mouseValue.BorderColor);
                    mouseValue.UseGraphicAlpha = EditorGUILayout.Toggle("����ͼƬ͸����", mouseValue.UseGraphicAlpha);
                    break;
                default:
                    break;
            }
            return mouseValue;
        }
    }
#endif
}

