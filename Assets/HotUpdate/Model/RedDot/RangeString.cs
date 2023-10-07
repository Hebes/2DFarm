using System;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    范围字符串
    表示在Source字符串中，从StartIndex到EndIndex范围的字符构成的字符串

-----------------------*/

namespace Farm2D
{
    public struct RangeString : IEquatable<RangeString>
    {
        private string m_Source;            //源字符串
        private int m_StartIndex;           //开始索引
        private int m_EndIndex;             //结束范围
        private int m_Length;               //长度
        private bool m_IsSourceNullOrEmpty; //源字符串是否为Null或Empty
        private int m_HashCode;             //哈希码

        /// <summary>
        /// 范围字符串
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="endIndex">结束范围</param>
        public RangeString(string source, int startIndex, int endIndex)
        {
            m_Source = source;
            m_StartIndex = startIndex;
            m_EndIndex = endIndex;
            m_Length = endIndex - startIndex + 1;
            m_IsSourceNullOrEmpty = string.IsNullOrEmpty(source);
            m_HashCode = 0;
        }

        public bool Equals(RangeString other)
        {
            bool isOtherNullOrEmpty = string.IsNullOrEmpty(other.m_Source);

            if (m_IsSourceNullOrEmpty && isOtherNullOrEmpty) return true;
            if (m_IsSourceNullOrEmpty || isOtherNullOrEmpty) return false;
            if (m_Length != other.m_Length) return false;

            for (int i = m_StartIndex, j = other.m_StartIndex; i <= m_EndIndex; i++, j++)
            {
                if (m_Source[i] != other.m_Source[j])
                    return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            if (m_HashCode == 0 && !m_IsSourceNullOrEmpty)
            {
                for (int i = m_StartIndex; i <= m_EndIndex; i++)
                    m_HashCode = 31 * m_HashCode + m_Source[i];
            }
            return m_HashCode;
        }
        public override string ToString()
        {
            RedDotSystem.Instance.CachedSb.Clear();
            for (int i = m_StartIndex; i <= m_EndIndex; i++)
                RedDotSystem.Instance.CachedSb.Append(m_Source[i]);
            string str = RedDotSystem.Instance.CachedSb.ToString();
            return str;
        }
    }
}
