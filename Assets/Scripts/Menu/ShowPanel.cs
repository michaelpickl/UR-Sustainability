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
    private Sprite originalIcon; 
    private Color originalColor; 
    private bool isClicked = false; 
    private string currentBuildingName;
    private string currentMeasureName;

    void Start()
    {
        
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }else
        {
            Debug.LogWarning("ButtonImage ist nicht zugewiesen!");
        }
      
        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (!isClicked)
        {
            if (buttonImage != null)
            {
                buttonImage.color = clickedColor;
            }
            panel.SetActive(true);
            fill1.SetActive(true);
            fill2.SetActive(true);
            Debug.Log("in OnButtonClick "+ myButton.name + " " + buttonImage.color);
        }
        else
        {
              if (buttonImage != null)
            {
                buttonImage.color = originalColor;
            }
            panel.SetActive(false);
            fill1.SetActive(false);
            fill2.SetActive(false);
        }
        isClicked = !isClicked;
        
        showMeasurePreview(isClicked);
    }



    public void SetBuildingAndMeasure(string buildingName, string measureName)
    {
        currentBuildingName = buildingName;
        currentMeasureName = measureName;
        Debug.Log("IN SETRBUILIODNG EASUR"+buildingName+ " " + measureName );
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
