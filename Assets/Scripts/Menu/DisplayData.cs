using UnityEngine;
using UnityEngine.UI;

public class DisplayData : MonoBehaviour
{
    public Slider slider1;
    public Text maxText1;
    
    public Slider slider2;
    public Text maxText2;
    
    public Slider slider3;
    public Text maxText3;

    private DataGetter dataGetter;

    void Start()
    {
        dataGetter = GetComponent<DataGetter>();
        UpdateUI();
    }

    void UpdateUI()
    {
        /*
        float value1 = dataGetter.GetValue1();
        float max1 = dataGetter.GetMaxValue1();
        slider1.value = value1 / max1;
        maxText1.text = "Max: " + max1.ToString();

        float value2 = dataGetter.GetValue2();
        float max2 = dataGetter.GetMaxValue2();
        slider2.value = value2 / max2;
        maxText2.text = "Max: " + max2.ToString();

        float value3 = dataGetter.GetValue3();
        float max3 = dataGetter.GetMaxValue3();
        slider3.value = value3 / max3;
        maxText3.text = "Max: " + max3.ToString();
        */
    }

    void Update()
    {
        UpdateUI();
    }
}
