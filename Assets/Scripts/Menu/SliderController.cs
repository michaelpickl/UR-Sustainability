using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    private float maxSliderValue;  // Der Maximalwert des Sliders
    private float currentValue;    // Der aktuelle Wert, den der Slider anzeigen soll

    void Start()
    {
        // Setze den maximalen Wert des Sliders auf den gewünschten Maximalwert
        slider.maxValue = maxSliderValue;
        UpdateSliderFill();
    }

    public void UpdateSliderFill()
    {
        // Berechne den Füllstand basierend auf dem aktuellen Wert relativ zum Maximalwert
        slider.value = currentValue;
    }
}
