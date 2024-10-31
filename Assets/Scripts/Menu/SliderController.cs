using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    private float maxSliderValue; 
    private float currentValue;   

    void Start()
    {
        slider.maxValue = maxSliderValue;
        UpdateSliderFill();
    }

    public void UpdateSliderFill()
    {
        slider.value = currentValue;
    }
}
