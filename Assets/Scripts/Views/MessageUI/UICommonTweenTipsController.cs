//using UnityEngine;
//using GCommon;
//using System.Collections.Generic;
//using System.Collections;
//using System;
//using UIFramework;

//namespace COW
//{
//    class UICommonTweenTipsController : BaseUI
//    {
//        private Vector3 m_BasePos;
//        private Vector3 m_CurrentPos;
//        private Vector3 m_Offset;

//        private float m_ClipLength = 3;
//        [SerializeField] TweenPosition m_ParentTP;

//        protected override void OnUIInit()
//        {
//            m_Offset = new Vector3(0, 44 + 4, 0);
//            m_BasePos = m_ParentTP.transform.localPosition;
//            m_CurrentPos = m_BasePos;
//        }

//        public void MoveUp()
//        {
//            m_ParentTP.from = m_CurrentPos;
//            m_ParentTP.to = m_CurrentPos += m_Offset;
//            m_ParentTP.duration = 0.3f;
//            m_ParentTP.PlayForward();
//        }

//        public IEnumerator OnFinished()
//        {
//            yield return new WaitForSeconds(m_ClipLength + 0.3f);
//            m_ParentTP.ResetToBeginning();
//            m_View.ParentContainer.transform.localPosition = m_BasePos;
//            m_CurrentPos = m_BasePos;
//            TweenTipsManager.instance.UpdateRunningIndex();
//            Hide();
//        }
//    }
//}
