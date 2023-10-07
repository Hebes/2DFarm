using System;
using System.Collections.Generic;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    树节点

-----------------------*/

namespace Farm2D
{
    public class TreeNode
    {
        private Dictionary<RangeString, TreeNode> m_Children;   //子节点
        private Action<int> m_ChangeCallback;                   //节点值改变回调
        private string m_FullPath;                              //完整路径

        public string Name//节点名
        {
            get;
            private set;
        }
        public string FullPath//完整路径
        {
            get
            {
                if (string.IsNullOrEmpty(m_FullPath))
                {
                    if (Parent == null || Parent == RedDotSystem.Instance.Root)
                        m_FullPath = Name;
                    else
                        m_FullPath = Parent.FullPath + RedDotSystem.Instance.SplitChar + Name;
                }
                return m_FullPath;
            }
        }
        public int Value//节点值
        {
            get;
            private set;
        }
        public TreeNode Parent//父节点
        {
            get;
            private set;
        }
        public Dictionary<RangeString, TreeNode>.ValueCollection Children//子节点
        {
            get
            {
                return m_Children?.Values;
            }
        }
        public int ChildrenCount//子节点数量
        {
            get
            {
                if (m_Children == null)
                {
                    return 0;
                }

                int sum = m_Children.Count;
                foreach (TreeNode node in m_Children.Values)
                {
                    sum += node.ChildrenCount;
                }
                return sum;
            }
        }

        public TreeNode(string name)
        {
            Name = name;
            Value = 0;
            m_ChangeCallback = null;
        }
        public TreeNode(string name, TreeNode parent) : this(name)
        {
            Parent = parent;
        }

        /// <summary>
        /// 添加节点值监听
        /// </summary>
        /// <param name="callback"></param>
        public void AddListener(Action<int> callback)
        {
            m_ChangeCallback += callback;
        }

        /// <summary>
        /// 移除节点值监听
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveListener(Action<int> callback)
        {
            m_ChangeCallback -= callback;
        }

        /// <summary>
        /// 移除所有节点值监听
        /// </summary>
        public void RemoveAllListener()
        {
            m_ChangeCallback = null;
        }

        /// <summary>
        /// 改变节点值（使用传入的新值，只能在叶子节点上调用）
        /// </summary>
        public void ChangeValue(int newValue)
        {
            if (m_Children != null && m_Children.Count != 0)
                throw new Exception("不允许直接改变非叶子节点的值：" + FullPath);

            InternalChangeValue(newValue);
        }

        /// <summary>
        /// 改变节点值（根据子节点值计算新值，只对非叶子节点有效）
        /// </summary>
        public void ChangeValue()
        {
            int sum = 0;

            if (m_Children != null && m_Children.Count != 0)
            {
                foreach (KeyValuePair<RangeString, TreeNode> child in m_Children)
                {
                    sum += child.Value.Value;
                }
            }

            InternalChangeValue(sum);
        }

        /// <summary>
        /// 获取子节点，如果不存在则添加
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TreeNode GetOrAddChild(RangeString key)
        {
            TreeNode child = GetChild(key);
            if (child == null)
            {
                child = AddChild(key);
            }
            return child;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TreeNode GetChild(RangeString key)
        {

            if (m_Children == null)
            {
                return null;
            }

            TreeNode child;
            m_Children.TryGetValue(key, out child);
            return child;
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TreeNode AddChild(RangeString key)
        {
            if (m_Children == null)
            {
                m_Children = new Dictionary<RangeString, TreeNode>();
            }
            else if (m_Children.ContainsKey(key))
            {
                throw new Exception("子节点添加失败，不允许重复添加：" + FullPath);
            }

            TreeNode child = new TreeNode(key.ToString(), this);
            m_Children.Add(key, child);
            RedDotSystem.Instance.NodeNumChangeCallback?.Invoke();
            return child;
        }

        /// <summary>
        /// 移除子节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveChild(RangeString key)
        {
            if (m_Children == null || m_Children.Count == 0)
            {
                return false;
            }

            TreeNode child = GetChild(key);

            if (child != null)
            {
                //子节点被删除 需要进行一次父节点刷新
                RedDotSystem.Instance.MarkDirtyNode(this);

                m_Children.Remove(key);

                RedDotSystem.Instance.NodeNumChangeCallback?.Invoke();

                return true;
            }

            return false;
        }

        /// <summary>
        /// 移除所有子节点
        /// </summary>
        public void RemoveAllChild()
        {
            if (m_Children == null || m_Children.Count == 0)
            {
                return;
            }

            m_Children.Clear();
            RedDotSystem.Instance.MarkDirtyNode(this);
            RedDotSystem.Instance.NodeNumChangeCallback?.Invoke();
        }

        /// <summary>
        /// 改变节点值
        /// </summary>
        /// <param name="newValue"></param>
        private void InternalChangeValue(int newValue)
        {
            if (Value == newValue)
            {
                return;
            }

            Value = newValue;
            m_ChangeCallback?.Invoke(newValue);
            RedDotSystem.Instance.NodeValueChangeCallback?.Invoke(this, Value);

            //标记父节点为脏节点
            RedDotSystem.Instance.MarkDirtyNode(Parent);
        }

        public override string ToString()
        {
            return FullPath;
        }
    }
}
