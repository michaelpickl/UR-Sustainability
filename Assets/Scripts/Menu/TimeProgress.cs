using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement; //neu

public class TimeProgress : MonoBehaviour
{
    public Slider timeSlider;
    public float startTime = 0f;
    public float endTime = 100f;
    public float timeSpeed = 1f;
    public MoneyManager moneyManager;
    public float yearlyIncome = 2000000;
    private int lastYear;
    private DataGetter dataGetter;
    private Co2Manager co2Manager;

    private float currentTime;

    void Start()
    {
        currentTime = startTime;
        timeSlider.minValue = startTime;
        timeSlider.maxValue = endTime;
        timeSlider.value = currentTime;
        moneyManager = GameObject.FindObjectOfType<MoneyManager>();
        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
        co2Manager = GameObject.Find("Co2Manager").GetComponent<Co2Manager>();
        lastYear = GetCurrentYear();
    }

    void Update()
    {
        int currentYear = GetCurrentYear();
        
        currentTime += Time.deltaTime * timeSpeed;

        timeSlider.value = Mathf.Clamp(currentTime, startTime, endTime);

        //Debug.Log("Current Time: " + currentTime);

        if (GetCurrentTime() >= endTime)
        {
            PlayerPrefs.SetInt("MeasuresAll", dataGetter.GetNumberOfAllMeasures());
            PlayerPrefs.SetInt("MeasuresDone", dataGetter.GetNumberOfDoneMeasures());
            PlayerPrefs.SetInt("CO2%Saved", (int)(((co2Manager.maxCo2 - co2Manager.GetCurrentCo2()) / (float)co2Manager.maxCo2) * 100));
            PlayerPrefs.Save();
            SceneManager.LoadScene("End");
        }

        if (currentTime >= endTime)
        {
            currentTime = endTime;
        }

        
        if (currentYear > lastYear)
        {
            moneyManager.AddMoney(yearlyIncome);
            lastYear = currentYear; 
        }
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }


    public string GetCurrentMonth()
    {
        float totalMonths = (endTime - startTime) / 12;
        float currentMonthIndex = (currentTime / (endTime - startTime)) * totalMonths;

        int monthIndex = Mathf.FloorToInt(currentMonthIndex) % 12;

        string[] months = new string[]
        {
            "JAN", "FEB", "MAR", "APR", "MAY", "JUN",
            "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"
        };

        return months[monthIndex];
    }


    public int GetCurrentYear()
    {
        int currentYear = (int)(currentTime / (endTime - startTime) * (2050 - 2024)) + 2024;
        return currentYear;
    }

    public int GetYearsUntil2050()
    {
        int currentYear = GetCurrentYear();
        int yearsUntil2050 = 2050 - currentYear;

        return Mathf.Max(0, yearsUntil2050);
    }
}
