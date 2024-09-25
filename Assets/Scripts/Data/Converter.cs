using UnityEngine;
using System.Linq;

public class Converter : MonoBehaviour
{
    const float tCO2e_PER_kWh_ENERGY = 0.0004491f; //https://calculator.carbonfootprint.com/calculator.aspx?lang=de&tab=2
    const float tCO2e_PER_kWh_GAS = 0.0001829f; //https://calculator.carbonfootprint.com/calculator.aspx?lang=de&tab=2

    public float ToCO2e(string type, float value)
    {
        if(type == "Strom")
        {
            return value * tCO2e_PER_kWh_ENERGY;
        }
        else if(type == "Wärme" || type == "Kälte")
        {
            return value * tCO2e_PER_kWh_GAS;
        }
        return value;
    }

    public float getBuildingYearlytCO2e(Building building)
    {
        float yearlytCO2e = 0f;
        foreach(Consumer consumer in building.consumers)
        {
            float kWhSum = consumer.monthly_values.Sum();
            yearlytCO2e += ToCO2e(consumer.type, kWhSum);
        }
        return yearlytCO2e;
    }
}