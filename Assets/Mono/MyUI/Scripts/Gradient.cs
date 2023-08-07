using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Text��������˳��
    5-0 ---- 1
    | \    |
    |  \   |
    |   \  |
    |    \ |
    4-----3-2
*/

namespace AnUIComponents
{
    /// <summary>
    /// ����
    /// </summary>
    [DisallowMultipleComponent]
    //[AddComponentMenu("LFramework/UI/Effects/Gradient", 1)]
    public class Gradient : BaseMeshEffect
    {
        protected Gradient()
        {

        }

        /// <summary>
        /// ���䷽��
        /// </summary>
        public enum EGradientDir
        {
            TopToBottom,//left-right
            BottomToTop,
            LeftToRight,
            RightToLeft,//top-bottom
        }

        /// <summary>
        /// ���䷽��
        /// </summary>
        [SerializeField]
        EGradientDir m_GradientDir = EGradientDir.TopToBottom;
        public EGradientDir GradientDir
        {
            get
            {
                return m_GradientDir;
            }
            set
            {
                m_GradientDir = value;
                graphic.SetVerticesDirty();
            }
        }

        //��ɫ����
        [SerializeField]
        Color32[] m_ColorArray = new Color32[2] { Color.black, Color.white };
        public Color32[] ColorArray
        {
            get
            {
                return m_ColorArray;
            }
            set
            {
                m_ColorArray = value;
                graphic.SetVerticesDirty();
            }
        }

        //���㻺��
        List<UIVertex> m_VertexCache = new List<UIVertex>();
        //����ʹ�õĶ����б�
        List<UIVertex> m_VertexList = new List<UIVertex>();

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            vh.GetUIVertexStream(m_VertexCache);

            switch (m_GradientDir)
            {
                case EGradientDir.TopToBottom:
                    ApplyGradient_TopToBottom(m_VertexCache);
                    break;
                case EGradientDir.BottomToTop:
                    ApplyGradient_BottomToTop(m_VertexCache);
                    break;
                case EGradientDir.LeftToRight:
                    ApplyGradient_LeftToRight(m_VertexCache);
                    break;
                case EGradientDir.RightToLeft:
                    ApplyGradient_RightToLeft(m_VertexCache);
                    break;
                default:
                    break;
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(m_VertexList);
            m_VertexCache.Clear();
            m_VertexList.Clear();
        }

        void ApplyGradient_LeftToRight(List<UIVertex> vertexCache)
        {
            if (vertexCache.Count == 0)
            {
                return;
            }
            if (m_ColorArray.Length < 2)
            {
                return;
            }

            int vertexCountPer = 6;//ÿһ�����ֵĶ�����
            int vertexCount = vertexCache.Count;
            int colorCount = m_ColorArray.Length;
            for (int n = 0; n < vertexCount / 6; n++)
            {
                UIVertex lastVertexLB = new UIVertex();
                UIVertex lastVertexRB = new UIVertex();
                for (int i = 0; i < colorCount - 1; i++)
                {
                    UIVertex vertexRT;
                    UIVertex vertexLT;
                    UIVertex vertexRB;
                    UIVertex vertexLB;

                    //���ϽǺ����Ͻ�
                    if (i == 0)
                    {
                        vertexLT = CalcVertex(vertexCache[n * vertexCountPer + 0], m_ColorArray[i]);
                        vertexRT = CalcVertex(vertexCache[n * vertexCountPer + 1], m_ColorArray[i]);
                    }
                    else
                    {
                        vertexLT = lastVertexLB;
                        vertexRT = lastVertexRB;
                    }

                    //���½Ǻ����½�
                    if (i == colorCount - 2)
                    {
                        vertexLB = CalcVertex(vertexCache[n * vertexCountPer + 4], m_ColorArray[i + 1]);
                        vertexRB = CalcVertex(vertexCache[n * vertexCountPer + 2], m_ColorArray[i + 1]);
                    }
                    else
                    {
                        vertexLB = CalcVertex(vertexCache[n * vertexCountPer + 4], vertexCache[n * vertexCountPer + 0],
                            (colorCount - i - 2) * 1f / (colorCount - 1), m_ColorArray[i + 1]);
                        vertexRB = CalcVertex(vertexCache[n * vertexCountPer + 2], vertexCache[n * vertexCountPer + 1],
                            (colorCount - i - 2) * 1f / (colorCount - 1), m_ColorArray[i + 1]);
                    }

                    lastVertexLB = vertexLB;
                    lastVertexRB = vertexRB;

                    m_VertexList.Add(vertexLT);
                    m_VertexList.Add(vertexRT);
                    m_VertexList.Add(vertexRB);
                    m_VertexList.Add(vertexRB);
                    m_VertexList.Add(vertexLB);
                    m_VertexList.Add(vertexLT);
                }
            }
        }

        void ApplyGradient_RightToLeft(List<UIVertex> vertexCache)
        {
            if (vertexCache.Count == 0)
            {
                return;
            }
            if (m_ColorArray.Length < 2)
            {
                return;
            }

            int vertexCountPer = 6;//ÿһ�����ֵĶ�����
            int vertexCount = vertexCache.Count;
            int colorCount = m_ColorArray.Length;
            for (int n = 0; n < vertexCount / 6; n++)
            {
                UIVertex lastVertexLT = new UIVertex();
                UIVertex lastVertexRT = new UIVertex();
                for (int i = 0; i < colorCount - 1; i++)
                {
                    UIVertex vertexRT;
                    UIVertex vertexLT;
                    UIVertex vertexRB;
                    UIVertex vertexLB;

                    //���½Ǻ����½�
                    if (i == 0)
                    {
                        vertexLB = CalcVertex(vertexCache[n * vertexCountPer + 4], m_ColorArray[i]);
                        vertexRB = CalcVertex(vertexCache[n * vertexCountPer + 2], m_ColorArray[i]);
                    }
                    else
                    {
                        vertexLB = lastVertexLT;
                        vertexRB = lastVertexRT;
                    }

                    //���ϽǺ����Ͻ�
                    if (i == colorCount - 2)
                    {
                        vertexLT = CalcVertex(vertexCache[n * vertexCountPer + 0], m_ColorArray[i + 1]);
                        vertexRT = CalcVertex(vertexCache[n * vertexCountPer + 1], m_ColorArray[i + 1]);
                    }
                    else
                    {
                        vertexLT = CalcVertex(vertexCache[n * vertexCountPer + 0], vertexCache[n * vertexCountPer + 4],
                            (colorCount - i - 2) * 1f / (colorCount - 1), m_ColorArray[i + 1]);
                        vertexRT = CalcVertex(vertexCache[n * vertexCountPer + 1], vertexCache[n * vertexCountPer + 2],
                            (colorCount - i - 2) * 1f / (colorCount - 1), m_ColorArray[i + 1]);
                    }

                    lastVertexLT = vertexLT;
                    lastVertexRT = vertexRT;

                    m_VertexList.Add(vertexLT);
                    m_VertexList.Add(vertexRT);
                    m_VertexList.Add(vertexRB);
                    m_VertexList.Add(vertexRB);
                    m_VertexList.Add(vertexLB);
                    m_VertexList.Add(vertexLT);
                }
            }
        }

        void ApplyGradient_BottomToTop(List<UIVertex> vertexCache)
        {
            if (vertexCache.Count == 0)
            {
                return;
            }
            if (m_ColorArray.Length < 2)
            {
                return;
            }

            int vertexCountPer = 6;//ÿһ�����ֵĶ�����
            int vertexCount = vertexCache.Count;
            int colorCount = m_ColorArray.Length;
            for (int n = 0; n < vertexCount / 6; n++)
            {
                UIVertex lastVertexRT = new UIVertex();
                UIVertex lastVertexRB = new UIVertex();
                for (int i = 0; i < colorCount - 1; i++)
                {
                    UIVertex vertexRT;
                    UIVertex vertexLT;
                    UIVertex vertexRB;
                    UIVertex vertexLB;

                    //���ϽǺ����½�
                    if (i == 0)
                    {
                        vertexLT = CalcVertex(vertexCache[n * vertexCountPer + 0], m_ColorArray[i]);
                        vertexLB = CalcVertex(vertexCache[n * vertexCountPer + 4], m_ColorArray[i]);
                    }
                    else
                    {
                        vertexLT = lastVertexRT;
                        vertexLB = lastVertexRB;
                    }

                    //���ϽǺ����½�
                    if (i == colorCount - 2)
                    {
                        vertexRT = CalcVertex(vertexCache[n * vertexCountPer + 1], m_ColorArray[i + 1]);
                        vertexRB = CalcVertex(vertexCache[n * vertexCountPer + 2], m_ColorArray[i + 1]);
                    }
                    else
                    {
                        vertexRT = CalcVertex(vertexCache[n * vertexCountPer + 1], vertexCache[n * vertexCountPer + 0],
                            (colorCount - i - 2) * 1f / (colorCount - 1), m_ColorArray[i + 1]);
                        vertexRB = CalcVertex(vertexCache[n * vertexCountPer + 2], vertexCache[n * vertexCountPer + 4],
                            (colorCount - i - 2) * 1f / (colorCount - 1), m_ColorArray[i + 1]);
                    }

                    lastVertexRT = vertexRT;
                    lastVertexRB = vertexRB;

                    m_VertexList.Add(vertexLT);
                    m_VertexList.Add(vertexRT);
                    m_VertexList.Add(vertexRB);
                    m_VertexList.Add(vertexRB);
                    m_VertexList.Add(vertexLB);
                    m_VertexList.Add(vertexLT);
                }
            }
        }

        void ApplyGradient_TopToBottom(List<UIVertex> vertexCache)
        {
            if (vertexCache.Count == 0)
            {
                return;
            }
            if (m_ColorArray.Length < 2)
            {
                return;
            }

            int vertexCountPer = 6;//ÿһ�����ֵĶ�����
            int vertexCount = vertexCache.Count;
            int colorCount = m_ColorArray.Length;
            for (int n = 0; n < vertexCount / 6; n++)
            {
                UIVertex lastVertexLT = new UIVertex();
                UIVertex lastVertexLB = new UIVertex();
                for (int i = 0; i < colorCount - 1; i++)
                {
                    UIVertex vertexRT;
                    UIVertex vertexLT;
                    UIVertex vertexRB;
                    UIVertex vertexLB;

                    //���ϽǺ����½�
                    if (i == 0)
                    {
                        vertexRT = CalcVertex(vertexCache[n * vertexCountPer + 1], m_ColorArray[i]);
                        vertexRB = CalcVertex(vertexCache[n * vertexCountPer + 2], m_ColorArray[i]);
                    }
                    else
                    {
                        vertexRT = lastVertexLT;
                        vertexRB = lastVertexLB;
                    }

                    //���ϽǺ����½�
                    if (i == colorCount - 2)
                    {
                        vertexLT = CalcVertex(vertexCache[n * vertexCountPer + 0], m_ColorArray[i + 1]);
                        vertexLB = CalcVertex(vertexCache[n * vertexCountPer + 4], m_ColorArray[i + 1]);
                    }
                    else
                    {
                        vertexLT = CalcVertex(vertexCache[n * vertexCountPer + 0], vertexCache[n * vertexCountPer + 1],
                            (colorCount - i - 2) * 1f / (colorCount - 1), m_ColorArray[i + 1]);
                        vertexLB = CalcVertex(vertexCache[n * vertexCountPer + 4], vertexCache[n * vertexCountPer + 2],
                            (colorCount - i - 2) * 1f / (colorCount - 1), m_ColorArray[i + 1]);
                    }

                    lastVertexLT = vertexLT;
                    lastVertexLB = vertexLB;

                    m_VertexList.Add(vertexLT);
                    m_VertexList.Add(vertexRT);
                    m_VertexList.Add(vertexRB);
                    m_VertexList.Add(vertexRB);
                    m_VertexList.Add(vertexLB);
                    m_VertexList.Add(vertexLT);
                }
            }
        }

        /// <summary>
        /// ���㶥������(ֻ������ɫ)
        /// </summary>
        UIVertex CalcVertex(UIVertex vertex, Color32 color)
        {
            vertex.color = color;
            return vertex;
        }

        /// <summary>
        /// ���㶥������
        /// </summary>
        UIVertex CalcVertex(UIVertex vertexA, UIVertex vertexB, float ratio, Color32 color)
        {
            UIVertex vertexTemp = new UIVertex();
            vertexTemp.position = (vertexB.position - vertexA.position) * ratio + vertexA.position;
            vertexTemp.color = color;
            vertexTemp.normal = (vertexB.normal - vertexA.normal) * ratio + vertexA.normal;
            vertexTemp.tangent = (vertexB.tangent - vertexA.tangent) * ratio + vertexA.tangent;
            vertexTemp.uv0 = (vertexB.uv0 - vertexA.uv0) * ratio + vertexA.uv0;
            vertexTemp.uv1 = (vertexB.uv1 - vertexA.uv1) * ratio + vertexA.uv1;
            return vertexTemp;
        }
    }
}
