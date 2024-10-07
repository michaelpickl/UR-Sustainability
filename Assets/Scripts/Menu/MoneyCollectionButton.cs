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
        HideButton();
    }

    void showButton(string buildingNameNew, string measureNameNew)
    {
        buildingName = buildingNameNew;
        measureName = measureNameNew;
        Building building = dataGetter.GetBuilding(buildingName);
        int costSavings = 0;
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
            gameObject.SetActive(true);
        }
        UpdateCountView();
    }

    void UpdateCountView()
    {
        moneyText.text = "" + moneyCount;
    }

    void HideButton()
    {
        gameObject.SetActive(false);
    }

}
