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

    private int moneyCount;



    void Start()
    {
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();

        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        moneyManager.AddMoney(moneyCount);
        moneyCount = 0;
        HideButton();
    }

    public void ShowButton(string buildingNameNew, string measureNameNew)
    {
        print("show collection " + buildingNameNew + "" + measureNameNew);
        buildingName = buildingNameNew;
        measureName = measureNameNew;
        if(dataGetter == null)
        {
            dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
        }
        Building building = dataGetter.GetBuilding(buildingName);
        if(building != null)
        {
            print("Collection 1 - " + buildingNameNew);
            int costSavings = 0;
            foreach(Measure measure in building.measures)
            {
                if(measure.name == measureName)
                {
                    costSavings = measure.cost_savings;
                }
            }
            print("collection costsavings: " + costSavings);
            if(gameObject.activeSelf)
            {
                print("Collection 2 - " + buildingNameNew);
                moneyCount += costSavings;
            }
            else{
                print("Collection 3 - " + buildingNameNew);
                moneyCount = costSavings;
                print("collection moneycount: " + moneyCount);
                if(moneyCount > 0)
                {
                    print("Collection 1 - " + buildingNameNew);
                    gameObject.SetActive(true);
                }
            }
            UpdateCountView();
        }
    }

    void UpdateCountView()
    {
        print("CHANGE MONEY TEXT TO: " + moneyCount);
        moneyText.text = "" + moneyCount;
    }

    void HideButton()
    {
        gameObject.SetActive(false);
    }

}
