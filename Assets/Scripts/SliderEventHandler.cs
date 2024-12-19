using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Slider slider;

    public Action<bool> OnPointerEvent { get; set; }

    public float SliderValue { get { return slider == null ? 0f : slider.value; } }

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetSliderValue(float value)
    {
        if (slider == null)
            return;

        slider.value = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerEvent?.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerEvent?.Invoke(false);
    }

    public void SetInteractable(bool isOn)
    {
        slider.interactable = isOn;
    }
}
