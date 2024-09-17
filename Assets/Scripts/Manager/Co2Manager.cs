using UnityEngine;
using UnityEngine.UI;

public class Co2Manager : MonoBehaviour
{

    public Slider co2Slider;
    public float maxCo2 = 337399.05f;
    private float currentCo2;

    
    void Start()
    {
        co2Slider.maxValue = maxCo2;
        currentCo2 = maxCo2;
        co2Slider.value = currentCo2;
        
        //OnMeasureCompleted wird aufgerufen, wenn das Ereignis in der CampusBuilding Klasse ausgelöst wird
        //muss noch angepasst werden auf das SideMenu
        //CampusBuilding.OnMeasureCompleted += OnMeasureCompleted;
    }

    private void OnMeasureCompleted(Measure measure)
    {
        ReduceCo2(measure.co2_savings);
    }

    public void ReduceCo2(int co2_savings)
    {
        currentCo2 -= co2_savings;

        if (currentCo2 < 0)
        {
            currentCo2 = 0;
        }

        // Slider aktualisieren
        co2Slider.value = currentCo2;
        Debug.Log("Reduzierter CO2-Wert: " + currentCo2);
    }
}
