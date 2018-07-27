//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UIFramwork;
//using UnityEngine.UI;

//public class PromptView : BaseView
//{
//    [SerializeField] Text _timeText;
//    [SerializeField] Image _bg;
//    public void OnClickClose()
//    {
//        GameManager._instance._viewManager.Close(this.GetHashCode());
//    }

//    public void SetData(int index, float time)
//    {
//        _bg.transform.localPosition = new Vector3(330 * (index == 0 ? -1 : 1), _bg.transform.localPosition.y, _bg.transform.localPosition.z);
//        _timeText.text = time.ToString();
//    }
//}
