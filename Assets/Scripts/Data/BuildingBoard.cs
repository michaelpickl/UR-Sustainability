using UnityEngine;
using System.Linq;

public class BuildingBoard : MonoBehaviour
{
    const float tCO2e_PER_kWh_ENERGY = 0.0004491f; //https://calculator.carbonfootprint.com/calculator.aspx?lang=de&tab=2
    const float tCO2e_PER_kWh_GAS = 0.0001829f; //https://calculator.carbonfootprint.com/calculator.aspx?lang=de&tab=2

    private DataGetter dataGetter;
    private Converter converter;
    private Building[] buildings;

    void Start()
    {
        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
        converter = GameObject.Find("Converter").GetComponent<Converter>();
        buildings = dataGetter.GetBuildings();

        float totalCO2 = buildings.Sum(building => converter.getBuildingYearlytCO2e(building));
        
        buildings = buildings.OrderByDescending(building => converter.getBuildingYearlytCO2e(building)).ToArray();
        
        Debug.Log("Gesamter jährlicher CO2-Ausstoß aller Gebäude: " + totalCO2);

        // Optional: print result
        foreach (Building building in buildings)
        {
            Debug.Log($"Building: {building.name}, CO2: {converter.getBuildingYearlytCO2e(building)}");
        }
    }
}
