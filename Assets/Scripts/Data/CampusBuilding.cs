using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampusBuilding : MonoBehaviour
{
    private DataGetter dataGetter;
    public string buildingName;
    private Building building;

    void Start()
    {
        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
        building = dataGetter.GetBuilding(buildingName);
        if(building != null){
            print(building.consumers[0].type);
        }
    }

    void Update()
    {
        
    }
}
