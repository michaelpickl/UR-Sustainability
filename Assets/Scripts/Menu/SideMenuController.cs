using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SideMenuController : MonoBehaviour
{
    public RectTransform menuPanel;
    public float menuWidth = 800f;
    public float animationSpeed = 3f;
    public TextMeshProUGUI  buildingNameText;

    private bool isMenuOpen = false;
    private Vector2 closedPosition;
    private Vector2 openPosition;

    private DataGetter dataGetter;
    private Building currentBuilding;

    void Start()
    {
        closedPosition = new Vector2(-menuWidth, 0);
        openPosition = new Vector2(100, 0);

        menuPanel.anchoredPosition = closedPosition;

        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
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
        if(buildingNameText != null)
        {
            buildingNameText.text = buildingName;
        }
        if (!isMenuOpen)
        {
            ToggleMenu();
        }
        currentBuilding = dataGetter.GetBuilding(buildingName);
        if(currentBuilding != null)
        {
            //ShowData()
            ShowMeasures();
        }
    }

    public void ShowMeasures()
    {
        // Sucht das Objekt "UpgradesTable" in den Kindobjekten des aktuellen Objekts
        Transform upgradesTable = GameObject.Find("UpgradesTable").transform;

        if (upgradesTable != null)
        {
            // Liste der Kindobjekte von "UpgradesTable"
            List<Transform> upgradeButtons = new List<Transform>();
            foreach (Transform child in upgradesTable)
            {
                upgradeButtons.Add(child);
            }

            // Befüllen der Buttons anhand der Measures-Liste
            for (int i = 0; i < upgradeButtons.Count; i++)
            {
                if (i < currentBuilding.measures.Length)
                {
                    upgradeButtons[i].gameObject.SetActive(true);
                    Measure measure = currentBuilding.measures[i];
                    GameObject buttonObject = upgradeButtons[i].gameObject;
                    
                    // Setze den Button aktiv
                    buttonObject.SetActive(true);

                    // Setze die "interactable" Eigenschaft des Buttons
                    Button button = buttonObject.GetComponent<Button>();
                    if (button != null)
                    {
                        button.interactable = !measure.done;
                    }

                    // Setze den Text von "UpgradeName"
                    Transform upgradeNameTransform = buttonObject.transform.Find("UpgradeName");
                    if (upgradeNameTransform != null)
                    {
                        TextMeshProUGUI upgradeNameText = upgradeNameTransform.GetComponent<TextMeshProUGUI>();
                        if (upgradeNameText != null)
                        {
                            upgradeNameText.text = measure.name;
                        }
                    }

                    // Setze das Bild falls notwendig
                    // Transform imageTransform = buttonObject.transform.Find("Image");
                    // Hier könntest du das Image entsprechend setzen, falls notwendig
                }
                else
                {
                    // Verstecke den Button, wenn es mehr Buttons als Measures gibt
                    upgradeButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("UpgradesTable nicht gefunden.");
        }
    }
}
