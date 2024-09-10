using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private float currentMoney;

    public void AddMoney(float amount)
    {
        currentMoney += amount;
    }

    public void SubtractMoney(float amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
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

    public string getMoneyString(int value)
    {
        if (value >= 1000000)
        {
            float shortenedValue = value / 1000000f;
            return shortenedValue.ToString("0.00") + "M€";
        }
        else if (value >= 1000)
        {
            float shortenedValue = value / 1000f;
            return shortenedValue.ToString("0.00") + "T€";
        }
        else
        {
            return value.ToString() + "€";
        }
    }
}
