using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[ExecuteInEditMode]
public class CustomSlider : MonoBehaviour
{
    [SerializeField] RectTransform _SliderArea;
    [SerializeField] RectTransform _Handle;
    Slider _Slider;
    float _SliderAreaLength; 

    private void Start()
    {
        _Slider = GetComponent<Slider>(); 
        _Slider.onValueChanged.AddListener(OnValueChanged); 
        _SliderAreaLength = _SliderArea.rect.width; 

        _Slider.value = 0.1f; 
    }

    private void OnDestroy()
    {
        _Slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    void OnValueChanged(float value)
    {
        _Handle.anchoredPosition = new Vector3(value * _SliderAreaLength, _Handle.anchoredPosition.y); 
    }
}
