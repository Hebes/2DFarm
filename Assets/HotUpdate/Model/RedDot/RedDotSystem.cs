using Core;
using System;
using System.Collections.Generic;
using System.Text;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    红点系统管理
    https://github.com/Aver58/Tools
    https://zhuanlan.zhihu.com/p/86069641

-----------------------*/

namespace Farm2D
{
    public class RedDotSystem : SingletonBase<RedDotSystem>
    {
        private Dictionary<string, TreeNode> m_AllNodes;                    //所有节点集合
        private HashSet<TreeNode> m_DirtyNodes;                             //脏节点集合
        private List<TreeNode> m_TempDirtyNodes;                            //临时脏节点集合

        public Action NodeNumChangeCallback;                                //节点数量改变回调
        public Action<TreeNode, int> NodeValueChangeCallback;               //节点值改变回调

        public char SplitChar//路径分隔字符
        {
            get;
            private set;
        }
        public StringBuilder CachedSb//缓存的StringBuild
        {
            get;
            private set;
        }
        public TreeNode Root//红点树根节点
        {
            get;
            private set;
        }

        public RedDotSystem()
        {
            SplitChar = '/';
            m_AllNodes = new Dictionary<string, TreeNode>();
            Root = new TreeNode("Root");
            m_DirtyNodes = new HashSet<TreeNode>();
            m_TempDirtyNodes = new List<TreeNode>();
            CachedSb = new StringBuilder();
        }

        /// <summary>
        /// 添加节点值监听
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public TreeNode AddListener(string path, Action<int> callback)
        {
            if (callback == null) return null;

            TreeNode node = GetTreeNode(path);
            node.AddListener(callback);

            return node;
        }

        /// <summary>
        /// 移除节点值监听
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void RemoveListener(string path, Action<int> callback)
        {
            if (callback == null) return;

            TreeNode node = GetTreeNode(path);
            node.RemoveListener(callback);
        }

        /// <summary>
        /// 移除所有节点值监听
        /// </summary>
        public void RemoveAllListener(string path)
        {
            TreeNode node = GetTreeNode(path);
            node.RemoveAllListener();
        }

        /// <summary>
        /// 改变节点值
        /// </summary>
        public void ChangeValue(string path, int newValue)
        {
            TreeNode node = GetTreeNode(path);
            node.ChangeValue(newValue);
        }

        /// <summary>
        /// 获取节点值
        /// </summary>
        public int GetValue(string path)
        {
            TreeNode node = GetTreeNode(path);
            if (node == null) return 0;

            return node.Value;
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        public TreeNode GetTreeNode(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new Exception("路径不合法，不能为空");

            TreeNode node;
            if (m_AllNodes.TryGetValue(path, out node)) return node;

            TreeNode cur = Root;
            int length = path.Length;

            int startIndex = 0;

            for (int i = 0; i < length; i++)
            {
                if (path[i] == SplitChar)
                {
                    if (i == length - 1)
                        throw new Exception("路径不合法，不能以路径分隔符结尾：" + path);

                    int endIndex = i - 1;
                    if (endIndex < startIndex)
                        throw new Exception("路径不合法，不能存在连续的路径分隔符或以路径分隔符开头：" + path);

                    TreeNode child = cur.GetOrAddChild(new RangeString(path, startIndex, endIndex));

                    //更新startIndex
                    startIndex = i + 1;

                    cur = child;
                }
            }

            //最后一个节点 直接用length - 1作为endIndex
            TreeNode target = cur.GetOrAddChild(new RangeString(path, startIndex, length - 1));

            m_AllNodes.Add(path, target);

            return target;


        }

        /// <summary>
        /// 移除节点
        /// </summary>
        public bool RemoveTreeNode(string path)
        {
            if (!m_AllNodes.ContainsKey(path)) return false;

            TreeNode node = GetTreeNode(path);
            m_AllNodes.Remove(path);
            return node.Parent.RemoveChild(new RangeString(node.Name, 0, node.Name.Length - 1));
        }

        /// <summary>
        /// 移除所有节点
        /// </summary>
        public void RemoveAllTreeNode()
        {
            Root.RemoveAllChild();
            m_AllNodes.Clear();
        }

        /// <summary>
        /// 管理器轮询
        /// </summary>
        public void Update()
        {
            if (m_DirtyNodes.Count == 0) return;

            m_TempDirtyNodes.Clear();
            foreach (TreeNode node in m_DirtyNodes)
                m_TempDirtyNodes.Add(node);
            m_DirtyNodes.Clear();

            //处理所有脏节点
            for (int i = 0; i < m_TempDirtyNodes.Count; i++)
                m_TempDirtyNodes[i].ChangeValue();
        }

        /// <summary>
        /// 标记脏节点
        /// </summary>
        public void MarkDirtyNode(TreeNode node)
        {
            if (node == null || node.Name == Root.Name) return;
            m_DirtyNodes.Add(node);
        }
    }
}
