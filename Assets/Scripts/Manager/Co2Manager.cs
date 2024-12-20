using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Co2Manager : MonoBehaviour
{

    public Slider co2Slider;
    public TextMeshProUGUI co2Text; 
    public float maxCo2 = 49637.15f;
    private float currentCo2;

    
    void Start()
    {
        co2Slider.maxValue = maxCo2;
        currentCo2 = maxCo2;
        co2Slider.value = currentCo2;
        UpdateCo2Text();

        //CampusBuilding.OnMeasureCompleted += OnMeasureCompleted;
    }

    private void OnMeasureCompleted(Measure measure)
    {
        ReduceCo2(measure.co2_savings);
    }

    public void ReduceCo2(float co2_savings)
    {
        currentCo2 -= co2_savings;

        if (currentCo2 < 0)
        {
            currentCo2 = 0;
        }

        // Slider aktualisieren
        co2Slider.value = currentCo2;
        UpdateCo2Text();
        //Debug.Log("Reduzierter CO2-Wert: " + currentCo2);
    }

    private void UpdateCo2Text()
    {
        // Den Text mit dem aktuellen CO2-Wert aktualisieren
        co2Text.text = currentCo2.ToString("F2") + " t";
    }

    public float GetCurrentCo2()
    {
        return currentCo2;
    }
}
