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
    public Slider electricitySlider;
    public Slider warmthSlider;
    public Slider coldSlider;
    public TextMeshProUGUI buildingElectricityConsumption;
    private float maxSliderValue;
    private float currentSliderValue; 
    public GameObject fillElectricity;
    public GameObject fillWarmth;
    public GameObject fillCold;

    public TextMeshProUGUI headingText;
    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI priceText;
    public TextMeshProUGUI durationText;

    public TextMeshProUGUI costSavingsText;
    public TextMeshProUGUI CO2SavingsText;
    private Sprite originalIcon;
    private Color originalColor;
    private bool isClicked = false;
    private string currentBuildingName;
    private string currentMeasureName;
    private Measure currentMeasure;
    private SideMenuController sideMenuController;
    private static List<ButtonController> allButtonControllers = new List<ButtonController>();
    private MoneyManager moneyManager;

    // consumption slider fill
    private TimeProgress timeProgress;
    private float currentYear;


    void Start()
    {
        sideMenuController = GameObject.FindObjectOfType<SideMenuController>();
        moneyManager = GameObject.FindObjectOfType<MoneyManager>();

        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
        else
        {
            Debug.LogWarning("ButtonImage ist nicht zugewiesen!");
        }
        allButtonControllers.Add(this);
        myButton.onClick.AddListener(OnButtonClick);

        //consumption slider fill
        timeProgress = GameObject.Find("TimeSlider").GetComponent<TimeProgress>();
    }

    // consumption slider fill
    void Update()
    {
        if (timeProgress != null)
        {
            currentYear = timeProgress.GetYearsUntil2050();
            //Debug.Log("Current Year from TimeProgress: " + currentYear);
        }
    }
    // consumption slider fill

    void OnButtonClick()
    {
        ResetAllOtherButtons();
        if (!isClicked)
        {
            buttonImage.color = clickedColor;
            ShowPanel();
        }
        else
        {
            HidePanel();
        }
        isClicked = !isClicked;

        showMeasurePreview(isClicked);
    }

    private void ResetAllOtherButtons()
    {
        foreach (var controller in allButtonControllers)
        {
            if (controller != this && controller.isClicked)
            {
                controller.HidePanel(); 
                sideMenuController.HideAllPreviews(); 
            }

            if (controller.buttonImage != null && controller.isClicked)
            {
                controller.buttonImage.color = controller.originalColor;
            }

            controller.isClicked = false;
        }
    }

    public void ResetButtonState()
    {
        if (buttonImage != null)
        {
            buttonImage.color = originalColor;
        }

        panel.SetActive(false);
        fillElectricity.SetActive(false);
        fillWarmth.SetActive(false);
        fillCold.SetActive(false);

        isClicked = false;
    }

    private void ShowPanel()
    {
        panel.SetActive(true);
        // fillElectricity.SetActive(true);
        // fillWarmth.SetActive(true);
        // fillCold.SetActive(true);

        showConsumptionSavings(currentMeasure);

        if (currentMeasure != null)
        {
            headingText.text = currentMeasure.name;
            descriptionText.text = currentMeasure.description;
            priceText.text = moneyManager.getMoneyString(currentMeasure.cost);
            durationText.text = currentMeasure.duration.ToString("0.##") + " Monate";
            if (currentMeasure.co2_savings < 10)
            {
                CO2SavingsText.text = currentMeasure.co2_savings.ToString("0.000");
            }
            else
            {
                CO2SavingsText.text = Mathf.RoundToInt(currentMeasure.co2_savings).ToString();
            }
            if (currentMeasure.cost_savings < 10)
            {
                costSavingsText.text = currentMeasure.cost_savings.ToString("0.000");
            }
            else
            {
                costSavingsText.text = Mathf.RoundToInt(currentMeasure.cost_savings).ToString();
            }


            if (moneyManager.GetCurrentMoney() < currentMeasure.cost)
            {
                priceText.color = Color.red;
            }
            else
            {
                priceText.color = Color.black;
            }

            sideMenuController.SetSelectedMeasure(currentMeasure);
        }
    }

    private void showConsumptionSavings(Measure measure)
    {
        if (measure.type == "Strom")
        {
            float maxValue = electricitySlider.maxValue;
            //Debug.Log("Max Value des Sliders: " + maxValue);
            currentSliderValue = measure.co2_savings;
            electricitySlider.value = currentSliderValue;

            //Debug.Log("Current Slider value set");

            //    float fillWidth = 50;
            //     RectTransform fillRect = electricitySlider.fillRect.GetComponent<RectTransform>();
            //     fillRect.sizeDelta = new Vector2(fillWidth, fillRect.sizeDelta.y);

            fillElectricity.SetActive(true);
        }
        else if (measure.type == "Wärme")
        {
            currentSliderValue = measure.co2_savings;
            warmthSlider.value = currentSliderValue;
            fillWarmth.SetActive(true);
        }
        else if (measure.type == "Kälte")
        {
            currentSliderValue = measure.co2_savings;
            coldSlider.value = currentSliderValue;
            fillCold.SetActive(true);
        }
        //Debug.Log("Type der Maßnahme: " + currentMeasure.type);
    }

    public void HidePanel()
    {
        if (buttonImage != null)
        {
            buttonImage.color = originalColor;
        }
        panel.SetActive(false);
        fillElectricity.SetActive(false);
        fillWarmth.SetActive(false);
        fillCold.SetActive(false);
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

    void showMeasurePreview(bool show = true)
    {
        GameObject buildingObject = GameObject.Find(currentBuildingName);

        if (buildingObject != null)
        {
            CampusBuilding campusBuilding = buildingObject.GetComponent<CampusBuilding>();

            if (campusBuilding != null)
            {
                if (show)
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
