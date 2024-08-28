using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;

public class DataGetter : MonoBehaviour
{
    public TextAsset jsonFile;
    private Buildings campus;
    void Awake()
    {
        campus = JsonUtility.FromJson<Buildings>(jsonFile.text);
        Building oth = GetBuilding("OTH");
        print("IMPORT TEST");
        print(oth.consumers[0].monthly_values[2]);
    }

    public Building GetBuilding(string buildingName)
    {
        if(campus == null){
            return null;
        }
        foreach(Building building in campus.buildings)
        {
            if(building.name == buildingName)
            {
                return building;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Buildings
{
    public Building[] buildings;
}

[System.Serializable]
public class Building
{
    public string name;
    public string abbreviation;
    public Consumer[] consumers;
    public Measure[] measures;
}

[System.Serializable]
public class Consumer
{
    public string type;
    public int[] monthly_values;
}

[System.Serializable]
public class Measure
{
    public string name;
    public int cost;
    public string duration;
    public int co2_savings;
}
