using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VectorGraphics;
using System.Linq;
using RTS_Cam;

public class SideMenuController : MonoBehaviour
{
    public RectTransform menuPanel;
    public float menuWidth = 800f;
    public float animationSpeed = 3f;
    public TextMeshProUGUI buildingNameText;
    public TextMeshProUGUI buildingLevel;
    public Slider electricitySlider;
    public Slider warmthSlider;
    public Slider coldSlider;
    public TextMeshProUGUI buildingElectricityConsumption;
    public TextMeshProUGUI buildingWarmthConsumption;
    public TextMeshProUGUI buildingColdConsumption;
    public Slider electricityConsumptionSlider;
    public Slider warmthConsumptionSlider;
    public Slider coldConsumptionSlider;
    
    public SVGImage buildingIcon;
    public Button buyButton;
    public Button closeButton;

    private RTS_Camera camManager;

    private ButtonController currentButtonController;
    private bool isMenuOpen = false;
    private Vector2 closedPosition;
    private Vector2 openPosition;

    private DataGetter dataGetter;
    private Building currentBuilding;
    private Measure selectedMeasure;
    private Co2Manager co2Manager;
    private MoneyManager moneyManager;
    private LoggingSystem loggingSystem;
    private Converter converter;

    void Start()
    {
        closedPosition = new Vector2(-menuWidth, 0);
        openPosition = new Vector2(100, 0);
        menuPanel.anchoredPosition = closedPosition;

        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
        co2Manager = GameObject.Find("Co2Manager").GetComponent<Co2Manager>();
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
        loggingSystem = GameObject.Find("LoggingSystem").GetComponent<LoggingSystem>();
        camManager = GameObject.Find("RTS_Camera").GetComponent<RTS_Camera>();
        converter = GameObject.Find("Converter").GetComponent<Converter>();
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        closeButton.onClick.AddListener(CloseSideMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMenu();
        }

        if (isMenuOpen)
        {
            menuPanel.anchoredPosition = Vector2.Lerp(menuPanel.anchoredPosition, openPosition, Time.deltaTime * animationSpeed);
        }
        else
        {
            menuPanel.anchoredPosition = Vector2.Lerp(menuPanel.anchoredPosition, closedPosition, Time.deltaTime * animationSpeed);
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
    }

    public bool IsMenuOpen()
    {
        return isMenuOpen;
    }

    public void OpenMenuWithBuildingName(string buildingName)
    {
        HideAllPreviews();
        ResetAllButtonStates();
        if (!isMenuOpen) ToggleMenu();

        currentBuilding = dataGetter.GetBuilding(buildingName);
        if (currentBuilding != null)
        {
            UpdateBuildingInfo(buildingName);
            ShowMeasures();
        }
    }

    private void ResetAllButtonStates()
    {
        ButtonController[] allButtons = FindObjectsOfType<ButtonController>();
        foreach (var button in allButtons)
        {
            button.ResetButtonState();
        }
    }

    private void UpdateBuildingInfo(string buildingName)
    {
        buildingNameText.text = buildingName;
        ShowBuildingLevel();
        SetBuildingIcon();
        ShowBuildingConsumption();
    }

    private void ShowBuildingLevel()
    {
        int completedMeasures = currentBuilding.measures.Count(m => m.done);
        buildingLevel.text = "Level " + completedMeasures;
    }

    private void SetBuildingIcon()
    {
        if (buildingIcon != null && currentBuilding.icon != null)
        {
            buildingIcon.sprite = currentBuilding.icon;
        }
        else
        {
            Debug.LogWarning("Kein Icon für das Gebäude gefunden.");
        }
    }

    string GetMonthlyValueForType(Building building, string consumerType, Slider slider)
    {
        foreach (Consumer consumer in building.consumers)
        {
            if (consumer.type == consumerType)
            {
                if (consumer.monthly_values.Length > 0)
                {
                    float firstMonthValue = converter.getBuildingYearlytCO2eByType(building, consumerType);
                    slider.maxValue = firstMonthValue;
                    Debug.Log("Slider MaxValue set");
                    string valueString = firstMonthValue.ToString();
                    return valueString;
                }
                else
                {
                    //Debug.LogWarning("No monthly_values found for consumer " + consumerType);
                    return "N/A";
                }
            }
        }
        return "N/A";
    }

    private void ShowBuildingConsumption()
    {
        string electricityConsumptionText = GetMonthlyValueForType(currentBuilding, "Strom", electricitySlider);
        string warmthConsumptionText = GetMonthlyValueForType(currentBuilding, "Wärme", warmthSlider);
        string coldConsumptionText = GetMonthlyValueForType(currentBuilding, "Kälte", coldSlider);

        CheckForConsumptionValue(buildingElectricityConsumption, electricityConsumptionText, electricityConsumptionSlider);
        CheckForConsumptionValue(buildingWarmthConsumption, warmthConsumptionText, warmthConsumptionSlider);
        CheckForConsumptionValue(buildingColdConsumption, coldConsumptionText, coldConsumptionSlider);

        /*
        No check for "0" and "N/A"
        //buildingElectricityConsumption.text = GetMonthlyValueForType(currentBuilding, "Strom") + " t CO2e";
        //buildingWarmthConsumption.text = GetMonthlyValueForType(currentBuilding, "Wärme") + " t CO2e";
        buildingColdConsumption.text = GetMonthlyValueForType(currentBuilding, "Kälte") + " t CO2e";
        */
    }

    private void CheckForConsumptionValue(TextMeshProUGUI consumptionText, string consumptionValue, Slider consumptionSlider)
    {
        if(consumptionValue != "0" && consumptionValue != "N/A")
        {
            consumptionSlider.gameObject.SetActive(true);
            consumptionText.text = consumptionValue + " t CO2e";
        }
        else
        {
            consumptionSlider.gameObject.SetActive(false);
        } 
    }

    public void ShowMeasures()
    {
        Transform upgradesTable = GameObject.Find("UpgradesTable").transform;

        if (upgradesTable != null)
        {
            List<Transform> upgradeButtons = new List<Transform>();
            foreach (Transform child in upgradesTable)
            {
                upgradeButtons.Add(child);
            }

            for (int i = 0; i < upgradeButtons.Count; i++)
            {
                GameObject buttonObject = upgradeButtons[i].gameObject;
                if (i < currentBuilding.measures.Length)
                {
                    buttonObject.SetActive(true);
                    SetupMeasureButton(buttonObject, currentBuilding.measures[i]);
                }
                else
                {
                    buttonObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("UpgradesTable nicht gefunden.");
        }
    }

    private void SetupMeasureButton(GameObject buttonObject, Measure measure)
    {
        Button button = buttonObject.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = !measure.done;
        }

        // Setzt den Text
        Transform upgradeNameTransform = buttonObject.transform.Find("UpgradeName");
        if (upgradeNameTransform != null)
        {
            TextMeshProUGUI upgradeNameText = upgradeNameTransform.GetComponent<TextMeshProUGUI>();
            if (upgradeNameText != null)
            {
                upgradeNameText.text = measure.name;
            }
        }

        //Setzt das Icon
        Transform imageTransform = buttonObject.transform.Find("Image");
        if (imageTransform != null)
        {
            SVGImage upgradeImage = imageTransform.GetComponent<SVGImage>();
            if (upgradeImage != null && measure.icon != null)
            {
                Debug.Log("Icon gefunden " + upgradeImage);
                upgradeImage.sprite = measure.icon;
            }
            else
            {
                Debug.LogWarning("Kein Icon für die Maßnahme " + measure.name + " gefunden.");
            }
        }

        // Zeige den Haken, wenn die Maßnahme abgeschlossen ist
        Transform checkmarkTransform = buttonObject.transform.Find("Checkmark"); // Name des Haken-Bildes im Prefab
        if (checkmarkTransform != null)
        {
            SVGImage checkmarkImage = checkmarkTransform.GetComponent<SVGImage>();
            checkmarkImage.enabled = measure.done;
            Debug.Log("Checkmark is enabled:  " + checkmarkImage.enabled);
        }

        currentButtonController = buttonObject.GetComponent<ButtonController>();
        if (currentButtonController != null)
        {
            currentButtonController.SetBuildingAndMeasure(currentBuilding.name, measure.name);
        }

    }

    public void SetSelectedMeasure(Measure measure)
    {
        selectedMeasure = measure;
    }

    public void OnBuyButtonClicked()
    {
        if (selectedMeasure != null && !selectedMeasure.done)
        {
            if (moneyManager.GetCurrentMoney() >= selectedMeasure.cost)
            {

                moneyManager.SubtractMoney(selectedMeasure.cost);
                co2Manager.ReduceCo2(selectedMeasure.co2_savings);
                selectedMeasure.done = true;

                CampusBuilding campusBuilding = GameObject.Find(currentBuilding.name).GetComponent<CampusBuilding>();
                if (campusBuilding != null)
                {
                    campusBuilding.HideMeasure(selectedMeasure.name);
                    StartCoroutine(campusBuilding.StartConstruction(selectedMeasure.duration, selectedMeasure.name)); // duration setzen
                }
                currentButtonController.HidePanel();
                CloseSideMenu();
                loggingSystem.addToLog(currentBuilding.name, selectedMeasure.name);
                Debug.Log($"Maßnahme {selectedMeasure.name} gekauft!");
            }
        }
    }

    public void HideAllPreviews()
    {
        if (currentButtonController != null) currentButtonController.HidePanel();

        foreach (Building building in dataGetter.GetBuildings())
        {
            GameObject buildingObject = GameObject.Find(building.name);

            if (buildingObject != null)
            {
                // Hole das Script "CampusBuilding" vom Gebäudeobjekt
                CampusBuilding campusBuilding = buildingObject.GetComponent<CampusBuilding>();
                if (campusBuilding != null)
                {
                    foreach (Measure measure in building.measures)
                        if (!measure.done && !campusBuilding.inConstructionMode())
                        {
                            campusBuilding.HideMeasure(measure.name);
                        }

                }
            }
        }
    }

    //TODO: Don`t forget to Hide the Previews when closing the menu!
    void CloseSideMenu()
    {
        HideAllPreviews();
        ToggleMenu();
        camManager.ResetTarget();
    }   
}
