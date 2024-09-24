using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRanking : MonoBehaviour
{
    public GameObject rankTemplate;
    public Transform contentPanel;
    public Sprite defaultIcon; 

    // Öffentliche Felder für die Icons
    public Sprite iconBuildingChemie;
    public Sprite iconBuildingRWS;
    public Sprite iconBuildingSport;


    private DataGetter dataGetter;
    private Converter converter;
    private Building[] buildings;
    
     // Dictionary für die Icons der Gebäude
    private Dictionary<string, Sprite> buildingIcons = new Dictionary<string, Sprite>();
    
    void Start()
    {
        Debug.Log("Initializing Building Ranking...");

        buildingIcons["Chemie"] = iconBuildingChemie;
        buildingIcons["Recht und Wirtschaft"] = iconBuildingRWS;
        buildingIcons["Sportzentrum"] = iconBuildingSport;


        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
        converter = GameObject.Find("Converter").GetComponent<Converter>();
        buildings = dataGetter.GetBuildings();
        buildings = buildings.OrderByDescending(building => converter.getBuildingYearlytCO2e(building)).ToArray();
        for (int i = 0; i< buildings.Length; i++){
            Building building = buildings[i];
            float co2Value = converter.getBuildingYearlytCO2e(building);
            int rank = buildings.Length - i;
            
            Sprite icon = buildingIcons.ContainsKey(building.name) ? buildingIcons[building.name] : defaultIcon;
            Debug.Log($"Creating rank {i + 1} for building: {building.name}, CO2: {co2Value}");
            CreateRank(rank, building.name, co2Value, icon);
        }
    }

    void CreateRank(int rank, string buildingName, float co2Value, Sprite icon){
        Debug.Log("in create Rank");
        GameObject newRank = Instantiate(rankTemplate, contentPanel);

        // Überprüfen, ob die UI-Elemente vorhanden sind
        Transform rankingTextTransform = newRank.transform.Find("RankingText");
        Transform buildingTextTransform = newRank.transform.Find("BuildingText");
        Transform co2TextTransform = newRank.transform.Find("CO2Text");
        Transform iconTransform = newRank.transform.Find("Icon");

        TMP_Text rankText = rankingTextTransform.GetComponent<TMP_Text>();
        TMP_Text buildingText = buildingTextTransform.GetComponent<TMP_Text>();
        TMP_Text co2Text = co2TextTransform.GetComponent<TMP_Text>();
        Image iconImage = iconTransform.GetComponent<Image>();

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
        Debug.Log(rankText.text + " " + "buildingText.text");
        iconImage.sprite = icon != null ? icon : defaultIcon;
        Debug.Log($"Assigned data to UI components for {buildingName}: Rank {rank}, CO2: {co2Value:F2} t CO2e.");
    }
  
}
