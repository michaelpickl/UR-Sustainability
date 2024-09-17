using UnityEngine;
using UnityEngine.UI;

public class TimeProgress : MonoBehaviour
{
    public Slider timeSlider; 
    public float startTime = 0f; 
    public float endTime = 100f; 
    public float timeSpeed = 1f; 

    private float currentTime; 

    void Start()
    {
        currentTime = startTime;
        timeSlider.minValue = startTime;
        timeSlider.maxValue = endTime;
        timeSlider.value = currentTime;
    }

    void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;

        timeSlider.value = Mathf.Clamp(currentTime, startTime, endTime);

        // Reset (optional)
        if (currentTime >= endTime)
        {
            currentTime = endTime;
        }
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
