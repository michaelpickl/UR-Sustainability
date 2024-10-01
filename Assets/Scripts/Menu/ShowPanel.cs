using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button myButton; 
    public SVGImage buttonImage; 
    public Color clickedColor = Color.red; 
    public GameObject panel;
    public GameObject fill1;
    public GameObject fill2;


    public TextMeshProUGUI headingText;
    public TextMeshProUGUI descriptionText;
    
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI durationText;
    private Sprite originalIcon; 
    private Color originalColor; 
    private bool isClicked = false; 
    private string currentBuildingName;
    private string currentMeasureName;
    private Measure currentMeasure;
    private SideMenuController sideMenuController;
    private static List<ButtonController> allButtonControllers = new List<ButtonController>();


    void Start()
    {
        sideMenuController = GameObject.FindObjectOfType<SideMenuController>();
        
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }else
        {
            Debug.LogWarning("ButtonImage ist nicht zugewiesen!");
        }
        allButtonControllers.Add(this);
        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        foreach (var controller in allButtonControllers)
        {
            if (controller != this && controller.isClicked) // Wenn ein anderer Button bereits geklickt ist
            {
                controller.HidePanel(); // Blende das Panel des anderen Buttons aus
                sideMenuController.HideAllPreviews();
            }
        }
        if (!isClicked)
        {
            if (buttonImage != null)
            {
                buttonImage.color = clickedColor;
            }
            panel.SetActive(true);
            fill1.SetActive(true);
            fill2.SetActive(true);

            if(currentMeasure != null)
            {
                headingText.text = currentMeasure.name;
                descriptionText.text = currentMeasure.description;
                priceText.text = currentMeasure.cost + " €";
                durationText.text = currentMeasure.duration + " Monate";
                if (sideMenuController != null)
                {
                    sideMenuController.SetSelectedMeasure(currentMeasure);
                }
            }
        }
        else
        {
            HidePanel();
        }
        isClicked = !isClicked;
        
        showMeasurePreview(isClicked);
    }

      public void HidePanel()
    {
        if (buttonImage != null)
        {
            buttonImage.color = originalColor;
        }
        panel.SetActive(false);
        fill1.SetActive(false);
        fill2.SetActive(false);
    }


    public void SetBuildingAndMeasure(string buildingName, string measureName)
    {
        currentBuildingName = buildingName;
        currentMeasureName = measureName;
        Building building = GameObject.FindObjectOfType<DataGetter>().GetBuilding(buildingName);
        if (building != null)
        {
            foreach (Measure measure in building.measures)
            {
                if (measure.name == measureName)
                {
                    currentMeasure = measure;
                    break;
                }
            }
        }
    }

    void showMeasurePreview(bool show = true){
        // Suche das Gebäudeobjekt anhand des Namens
        GameObject buildingObject = GameObject.Find(currentBuildingName);

        if (buildingObject != null)
        {
            // Hole das Script "CampusBuilding" vom Gebäudeobjekt
            CampusBuilding campusBuilding = buildingObject.GetComponent<CampusBuilding>();

            if (campusBuilding != null)
            {
                // Rufe die ShowMeasure-Methode auf
                if(show)
                {
                    campusBuilding.ShowMeasure(currentMeasureName);
                }
                else
                {
                    campusBuilding.HideMeasure(currentMeasureName);
                }
            }
            else
            {
                Debug.LogError($"CampusBuilding-Skript auf dem Objekt '{currentBuildingName}' nicht gefunden.");
            }
        }
        else
        {
            Debug.LogError($"Gebäude mit dem Namen '{currentBuildingName}' nicht gefunden.");
        }
    }
}
