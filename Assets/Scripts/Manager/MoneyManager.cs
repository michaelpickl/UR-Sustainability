using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    private float currentMoney = 10000000;
    private float maxMoney = 1000000000; 


    void Start()
    {
        UpdateMoneyText(); // Geldwert beim Start setzen
    }

    public void AddMoney(float amount)
    {
        currentMoney += amount;
        UpdateMoneyText();
    }

    public void SubtractMoney(float amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyText();
        }
        else
        {
            Debug.LogWarning("Nicht genügend Geld verfügbar.");
        }
    }

    public float GetCurrentMoney()
    {
        return currentMoney;
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = $"{getMoneyString(currentMoney)}/{getMoneyString(maxMoney)}";
        }
        else
        {
            Debug.LogWarning("MoneyText ist nicht zugewiesen.");
        }
    }

    public string getMoneyString(float value)
    {
        if (value >= 1000000)
        {
            float shortenedValue = value / 1000000f;
            return shortenedValue.ToString("0.00") + "M €";
        }
        else if (value >= 1000)
        {
            float shortenedValue = value / 1000f;
            return shortenedValue.ToString("0.00") + "T €";
        }
        else
        {
            return value.ToString() + " €";
        }
    }
}
