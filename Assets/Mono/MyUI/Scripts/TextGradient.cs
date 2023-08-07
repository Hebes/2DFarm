/*
  Author:lfw
  Data:2021.05.31
*/
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UI.Extension
{
    //[AddComponentMenu("UI/Effects/TextGradientFour")]
    //[RequireComponent(typeof(Text))]
    public class TextGradient : BaseMeshEffect
    {
        public Color[] colors;

        public bool MultiplyTextColor = false;

        protected TextGradient()
        {

        }

        public static Color32 Multiply(Color32 a, Color32 b)
        {
            a.r = (byte)((a.r * b.r) >> 8);
            a.g = (byte)((a.g * b.g) >> 8);
            a.b = (byte)((a.b * b.b) >> 8);
            a.a = (byte)((a.a * b.a) >> 8);
            return a;
        }

        private void ModifyVertices(VertexHelper vh)
        {
            List<UIVertex> verts = new List<UIVertex>(vh.currentVertCount);
            vh.GetUIVertexStream(verts);
            vh.Clear();

            int step = 6;

            /*
               5-0 ---- 1
                | \    |
                |  \   |
                |   \  |
                |    \ |
                4-----3-2
            */
            if(colors?.Length == 0)
            {
                Debug.LogWarning("«Îœ»ÃÌº”—’…´");
                return;
            }
            int colorCount = colors.Length;
            for (int n = 0; n < verts.Count / 6; n++)
            {
                UIVertex lastBottomR = default;
                UIVertex lastbottomL = default;
                for (int i = 0; i < colorCount - 1; i++)
                {
                    UIVertex tl = new UIVertex(), tr = new UIVertex(), bl = new UIVertex(), br = new UIVertex();
                    if (i == 0)
                    {
                        tl = multiplyColor(verts[n * step + 0], colors[0]);
                        tr = multiplyColor(verts[n * step + 1], colors[0]);
                    }
                    else
                    {
                        tl = lastbottomL;
                        tr = lastBottomR;
                    }

                    if (i == colorCount - 2)
                    {
                        bl = multiplyColor(verts[n * step + 4], colors[colors.Length - 1]);
                        br = multiplyColor(verts[n * step + 3], colors[colors.Length - 1]);
                    }
                    else
                    {
                        bl = calcCenterVertex(tl, verts[n * step + 4], (colorCount - i - 2) * 1f / (colorCount - i - 1), colors[i + 1]);
                        br = calcCenterVertex(tr, verts[n * step + 3], (colorCount - i - 2) * 1f / (colorCount - i - 1), colors[i + 1]);
                    }

                    vh.AddVert(tl);
                    vh.AddVert(tr);
                    vh.AddVert(br);

                    vh.AddVert(br);
                    vh.AddVert(bl);
                    vh.AddVert(tl);

                    lastBottomR = br;
                    lastbottomL = bl;
                }

                // Debug.Log("----------------"+n);
                // Debug.Log(verts[n * step + 0].position.ToString() + " "+ verts[n * step + 1].position.ToString());
                // Debug.Log(verts[n * step + 4].position.ToString() + " "+ verts[n * step + 3].position.ToString());
            }
            for (int i = 0; i < vh.currentVertCount; i += 3)
            {
                vh.AddTriangle(i + 0, i + 1, i + 2);
            }
        }

        private UIVertex multiplyColor(UIVertex vertex, Color color)
        {
            if (MultiplyTextColor)
                vertex.color = Multiply(vertex.color, color);
            else
                vertex.color = color;
            return vertex;
        }

        private UIVertex calcCenterVertex(UIVertex top, UIVertex bottom, float ratio, Color centerColor)
        {
            UIVertex center = new UIVertex();
            center.normal = (top.normal - bottom.normal) * ratio + bottom.normal;
            center.position = (top.position - bottom.position) * ratio + bottom.position;
            center.tangent = (top.tangent - bottom.tangent) * ratio + bottom.tangent;
            center.uv0 = (top.uv0 - bottom.uv0) * ratio + bottom.uv0;
            center.uv1 = (top.uv1 - bottom.uv1) * ratio + bottom.uv1;

            if (MultiplyTextColor)
            {
                var color = Color.Lerp(top.color, bottom.color, ratio);
                center.color = Multiply(color, centerColor);
            }
            else
            {
                center.color = centerColor;
            }

            return center;
        }

        #region implemented abstract members of BaseMeshEffect

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!this.IsActive())
            {
                return;
            }

            ModifyVertices(vh);
        }

        #endregion
    }
}