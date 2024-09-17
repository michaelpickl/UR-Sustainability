using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button myButton; 
    public Image buttonImage; 
    public Sprite newIcon; 
    public Color clickedColor = Color.red; 
    public GameObject panel;
    public GameObject fill1;
    public GameObject fill2;
    private Sprite originalIcon; 
    private Color originalColor; 
    private bool isClicked = false; 

    void Start()
    {
        
        originalIcon = buttonImage.sprite;
        originalColor = myButton.image.color;

        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (!isClicked)
        {
            buttonImage.sprite = newIcon;
            myButton.image.color = clickedColor;

            panel.SetActive(true);
            fill1.SetActive(true);
            fill2.SetActive(true);
        }
        else
        {
            buttonImage.sprite = originalIcon;
            myButton.image.color = originalColor;
            
            panel.SetActive(false);
            fill1.SetActive(false);
            fill2.SetActive(false);
        }
        isClicked = !isClicked;
    }
}
