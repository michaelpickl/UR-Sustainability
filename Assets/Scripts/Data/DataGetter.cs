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
        foreach(Building building in GetBuildings()){
            print(building.name);
        }
        LoadIconsForBuildings();
        LoadIconsForMeasures();
    }

    private void LoadIconsForBuildings()
    {
        foreach (Building building in campus.buildings)
        {
            string iconPath = "icons/gebäude/" + building.abbreviation;
            building.icon = LoadIcon(iconPath);
        }
    }

    private void LoadIconsForMeasures()
    {
        foreach (Building building in campus.buildings)
        {
            foreach (Measure measure in building.measures)
            {
                string iconPath = "icons/maßnahmen/" + measure.name.ToLower().Replace(" ", "_"); // Beispiel: "icons/maßnahmen/solarpanel"
                measure.icon = LoadIcon(iconPath);
            }
        }
    }


    public Sprite LoadIcon(string iconPath)
    {
        Sprite icon = Resources.Load<Sprite>(iconPath);
        if (icon == null)
        {
            Debug.LogWarning("Kein Icon gefunden unter Pfad: " + iconPath);
        }
        return icon;
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

    public Building[] GetBuildings()
    {
        if(campus == null){
            return null;
        }
        return campus.buildings;
    }

    public int GetNumberOfAllMeasures()
    {
        int counter = 0;
        foreach (Building building in campus.buildings)
        {
            foreach(Measure measure in building.measures)
            {
                counter++;
            }
        }
        return counter;
    }

    public int GetNumberOfDoneMeasures()
    {
        int counter = 0;
        foreach (Building building in campus.buildings)
        {
            foreach(Measure measure in building.measures)
            {
                if(measure.done)
                {
                    counter++;
                }
            }
        }
        return counter;
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
    public Sprite icon;
}

[System.Serializable]
public class Consumer
{
    public string type;
    public float[] monthly_values;
}

[System.Serializable]
public class Measure
{
    public string name;
    public string description;
    public int cost;
    public string duration;
    public int co2_savings;
    public int cost_savings;
    public string type;
    public bool done;
    public Sprite icon;
}
