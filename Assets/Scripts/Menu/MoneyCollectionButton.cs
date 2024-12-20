using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCollectionButton : MonoBehaviour
{
    public Button myButton; 
    public TextMeshProUGUI moneyText;

    private string buildingName;
    private string measureName;
    private MoneyManager moneyManager;
    private DataGetter dataGetter;

    private float moneyCount;



    void Start()
    {
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();

        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        moneyManager.AddMoney(moneyCount);
        moneyCount = 0f;
        HideButton();
    }

    public void ShowButton(string buildingNameNew, string measureNameNew)
    {
        buildingName = buildingNameNew;
        measureName = measureNameNew;
        if(dataGetter == null)
        {
            dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
        }
        Building building = dataGetter.GetBuilding(buildingName);
        if(building != null)
        {
            float costSavings = 0f;
            foreach(Measure measure in building.measures)
            {
                if(measure.name == measureName)
                {
                    costSavings = measure.cost_savings;
                }
            }
            if(gameObject.activeSelf)
            {
                moneyCount += costSavings;
            }
            else{
                moneyCount = costSavings;
                if(moneyCount > 0)
                {
                    gameObject.SetActive(true);
                }
            }
            UpdateCountView();
        }
    }

    void UpdateCountView()
    {
        moneyText.text = "" +  Mathf.Round(moneyCount);
    }

    void HideButton()
    {
        gameObject.SetActive(false);
    }

}
