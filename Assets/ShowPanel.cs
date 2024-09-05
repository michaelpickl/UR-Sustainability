using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button myButton; // Der Button
    public Image buttonImage; // Das Icon-Image des Buttons
    public Sprite newIcon; // Das neue Icon nach dem Klick
    public Color clickedColor = Color.green; // Die neue Farbe nach dem Klick
    public GameObject panel; // Das Panel, das ein- und ausgeblendet wird
    public GameObject fill1;
    public GameObject fill2;
    private Sprite originalIcon; // Das ursprüngliche Icon
    private Color originalColor; // Die ursprüngliche Farbe des Buttons
    private bool isClicked = false; // Status des Klicks

    void Start()
    {
        // Speichere das Original-Icon und die Originalfarbe
        originalIcon = buttonImage.sprite;
        originalColor = myButton.image.color;

        // Button-Click-Listener hinzufügen
        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Überprüfen, ob der Button bereits geklickt wurde
        if (!isClicked)
        {
            // Icon und Farbe ändern
            buttonImage.sprite = newIcon;
            myButton.image.color = clickedColor;

            // Panel anzeigen
            panel.SetActive(true);
            fill1.SetActive(true);
            fill2.SetActive(true);
        }
        else
        {
            // Icon und Farbe zurücksetzen
            buttonImage.sprite = originalIcon;
            myButton.image.color = originalColor;

            // Panel ausblenden
            panel.SetActive(false);
            fill1.SetActive(false);
            fill2.SetActive(false);
        }

        // Zustand umschalten
        isClicked = !isClicked;
    }
}
