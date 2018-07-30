using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramwork;
using UnityEngine.UI;

public class TipsView : BaseView
{
	[SerializeField] CanvasGroup _canvasGroup; 
    [SerializeField] Text _messageText;
    IEnumerator _routine;
    public override void Open()
    {
		Debug.LogError("TipsView.Open"); 
        base.Open();
        _routine = Routine();
		CoroutineUtil.Start(_routine);
    }

    public override void Close()
    {
        if (_routine != null)
        {
			CoroutineUtil.Stop(_routine);
            _routine = null; 
        }
		base.Close();
		Debug.LogError("TipsView.Close"); 
    }

    public void SetData(string s)
    {
        _messageText.text = s;
    }

    IEnumerator Routine()
    {
		_canvasGroup.alpha = 1; 
		yield return new WaitForSeconds(1); 
		float time = 0; 
		while (time <= 1)
		{
			yield return null;
			time += Time.deltaTime; 
			_canvasGroup.alpha = 1- time / 1; 
		}
        ViewManager.instance.Close(GetHashCode());
    }
}
