using UnityEngine;
using UnityEngine.UI;

public class CustomRightToLeftSlider : MonoBehaviour
{
    public Slider slider; 
    public RectTransform fillRect;

    void Update()
    {
        float width = slider.GetComponent<RectTransform>().rect.width;
        fillRect.sizeDelta = new Vector2(width * slider.normalizedValue, fillRect.sizeDelta.y);
    }
}
