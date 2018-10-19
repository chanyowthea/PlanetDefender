using System;
using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;

    public class MessageManager : TSingleton<MessageManager>
    {
        private const int MAX_TIPS = 6;
        private const int BASE_DEPTH = 900;

        private List<MessageView> m_CachedCtrls = new List<MessageView>();
        private int m_RunnigIndex;
        private int m_EmptyIndex;
        private int m_CurrentDepth;

        public override void Init()
        {
            for (int count = 0; count < MAX_TIPS; ++count)
            {
                var ctrl = UIManager.Instance.Open<MessageView>(); 
                m_CachedCtrls.Add(ctrl);
                ctrl.Hide();
            }
        }

        public override void Clear()
        {
            foreach (var ctrl in m_CachedCtrls)
            {
                if (ctrl != null)
                    ctrl.Close();
            }
            m_CachedCtrls.Clear();

            m_RunnigIndex = 0;
            m_EmptyIndex = 0;
            m_CurrentDepth = BASE_DEPTH;
        }
        
        public void ShowTips(string tips)
        {
            if (!string.IsNullOrEmpty(tips) && (m_EmptyIndex + 1) % MAX_TIPS != m_RunnigIndex)
            {
                if (m_EmptyIndex == m_RunnigIndex)
                {
                    m_CurrentDepth = BASE_DEPTH;
                }

                int start = m_RunnigIndex;
                while (start != m_EmptyIndex)
                {
                    if (m_CachedCtrls.Count <= start || m_CachedCtrls[start] == null)
                    {
                        return;
                    }
                    m_CachedCtrls[start].MoveUp();
                    start = (start + 1) % MAX_TIPS;
                }

                if (m_CachedCtrls.Count <= m_EmptyIndex || m_CachedCtrls[m_EmptyIndex] == null)
                {
                    return;
                }

                m_CachedCtrls[m_EmptyIndex].ShowTips(tips);

                m_EmptyIndex = (m_EmptyIndex + 1) % MAX_TIPS;
                m_CurrentDepth++;
            }
        }

        public void UpdateRunningIndex()
        {
            m_RunnigIndex = (m_RunnigIndex + 1) % MAX_TIPS;
        }
    }

