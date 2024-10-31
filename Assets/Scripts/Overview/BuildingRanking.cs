using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRanking : MonoBehaviour
{
    public GameObject rankTemplate;
    public Transform contentPanel;
    public Sprite defaultIcon; 

    private DataGetter dataGetter;
    private Converter converter;
    private Building[] buildings;
    
    private Dictionary<string, Sprite> buildingIcons = new Dictionary<string, Sprite>();
    
    void Start()
    {
        //Debug.Log("Initializing Building Ranking...");

        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
        converter = GameObject.Find("Converter").GetComponent<Converter>();
        buildings = dataGetter.GetBuildings();
        buildings = buildings.OrderByDescending(building => converter.getBuildingYearlytCO2e(building)).ToArray();
        for (int i = 0; i< buildings.Length; i++){
            Building building = buildings[i];
            float co2Value = converter.getBuildingYearlytCO2e(building);
            int rank = buildings.Length - i;
            
            Sprite icon = building.icon != null ? building.icon : defaultIcon;
            //Debug.Log($"Creating rank {i + 1} for building: {building.name}, CO2: {co2Value}, iconPath: {building.icon}");
            CreateRank(rank, building.name, co2Value, icon);
        }
    }

    void CreateRank(int rank, string buildingName, float co2Value, Sprite icon){
        //Debug.Log("in create Rank");
        GameObject newRank = Instantiate(rankTemplate, contentPanel);

        Transform rankingTextTransform = newRank.transform.Find("RankingText");
        Transform buildingTextTransform = newRank.transform.Find("BuildingText");
        Transform co2TextTransform = newRank.transform.Find("CO2Text");
        Transform iconTransform = newRank.transform.Find("Icon");

        TMP_Text rankText = rankingTextTransform.GetComponent<TMP_Text>();
        TMP_Text buildingText = buildingTextTransform.GetComponent<TMP_Text>();
        TMP_Text co2Text = co2TextTransform.GetComponent<TMP_Text>();
        SVGImage iconImage = iconTransform.GetComponent<SVGImage>();

        if (rankText == null) 
        {
            Debug.LogError("RankingText is missing!");
            return;
        }
        if (buildingText == null) 
        {
            Debug.LogError("BuildingText is missing!");
            return;
        }
        if (co2Text == null) 
        {
            Debug.LogError("CO2Text is missing!");
            return;
        }
        if (iconImage == null) 
        {
            Debug.LogError("Icon is missing!");
            return;
        }

        rankText.text = rank.ToString();
        buildingText.text = buildingName;
        co2Text.text = co2Value.ToString("F2") + " t CO2e";
        //Debug.Log(rankText.text + " " + "buildingText.text");
        iconImage.sprite = icon != null ? icon : defaultIcon;
        //Debug.Log($"Assigned data to UI components for {buildingName}: Rank {rank}, CO2: {co2Value:F2} t CO2e.");
    }
  
}
