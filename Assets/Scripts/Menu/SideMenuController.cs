using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VectorGraphics;

public class SideMenuController : MonoBehaviour
{
    public RectTransform menuPanel;
    public float menuWidth = 800f;
    public float animationSpeed = 3f;
    public TextMeshProUGUI buildingNameText;
    public Button buyButton;
    
    private ButtonController showPanelScript;
    private bool isMenuOpen = false;
    private Vector2 closedPosition;
    private Vector2 openPosition;

    private DataGetter dataGetter;
    private Building currentBuilding;
    private Measure selectedMeasure;
    private Co2Manager co2Manager;
    private MoneyManager moneyManager;
    private LoggingSystem loggingSystem;

    void Start()
    {
        closedPosition = new Vector2(-menuWidth, 0);
        openPosition = new Vector2(100, 0);

        menuPanel.anchoredPosition = closedPosition;

        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
        co2Manager = GameObject.Find("Co2Manager").GetComponent<Co2Manager>();
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
        loggingSystem = GameObject.Find("LoggingSystem").GetComponent<LoggingSystem>();
        buyButton.onClick.AddListener(OnBuyButtonClicked);
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

    public void OpenMenuWithBuildingName(string buildingName)
    {
        HideAllPreviews();

        if (buildingNameText != null)
        {
            buildingNameText.text = buildingName;
        }
        if (!isMenuOpen)
        {
            ToggleMenu();
        }
        currentBuilding = dataGetter.GetBuilding(buildingName);
        if (currentBuilding != null)
        {
            //ShowData()
            ShowMeasures();
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
                if (i < currentBuilding.measures.Length)
                {
                    upgradeButtons[i].gameObject.SetActive(true);
                    Measure measure = currentBuilding.measures[i];
                    GameObject buttonObject = upgradeButtons[i].gameObject;

                    buttonObject.SetActive(true);

                    Button button = buttonObject.GetComponent<Button>();
                    if (button != null)
                    {
                        button.interactable = !measure.done;
                    }

                    Transform upgradeNameTransform = buttonObject.transform.Find("UpgradeName");
                    if (upgradeNameTransform != null)
                    {
                        TextMeshProUGUI upgradeNameText = upgradeNameTransform.GetComponent<TextMeshProUGUI>();
                        if (upgradeNameText != null)
                        {
                            upgradeNameText.text = measure.name;
                        }
                    }


                    // SET IMAGE HERE
                    Transform imageTransform = buttonObject.transform.Find("Image");
                    if (imageTransform != null)
                    {
                        SVGImage upgradeImage = imageTransform.GetComponent<SVGImage>();
                        if (upgradeImage != null && measure.icon != null)
                        {
                            Debug.Log("Icon gefunden "+ upgradeImage);

                            upgradeImage.sprite = measure.icon;
                        }
                        else
                        {
                            Debug.LogWarning("Kein Icon für die Maßnahme " + measure.name + " gefunden.");
                        }
                    }


                    showPanelScript = buttonObject.GetComponent<ButtonController>();
                    if (showPanelScript != null)
                    {
                        showPanelScript.SetBuildingAndMeasure(currentBuilding.name, measure.name);
                    }
                }
                else
                {
                    upgradeButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("UpgradesTable nicht gefunden.");
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
                showPanelScript.HidePanel();
                Debug.Log("Maßnahme " + selectedMeasure.name + " gekauft!");
                ToggleMenu();
                loggingSystem.addToLog(currentBuilding.name, selectedMeasure.name);
            }
            else
            {
                Debug.LogWarning("Nicht genug Geld für diese Maßnahme.");
            }
        }
    }

    public void HideAllPreviews()
    {
        Building[] buildings = dataGetter.GetBuildings();
        foreach (Building building in buildings)
        {
            GameObject buildingObject = GameObject.Find(building.name);

            if (buildingObject != null)
            {
                // Hole das Script "CampusBuilding" vom Gebäudeobjekt
                CampusBuilding campusBuilding = buildingObject.GetComponent<CampusBuilding>();
                if (campusBuilding != null)
                {
                    foreach (Measure measure in building.measures)
                    if(!measure.done && !campusBuilding.inConstructionMode()){
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
    }
}
