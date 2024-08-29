using UnityEngine;

public class SideMenuController : MonoBehaviour
{
    public RectTransform menuPanel; // Panel, das das Seitenmenü enthält
    public float menuWidth = 800f; // Breite des Menüs
    public float animationSpeed = 3f; // Geschwindigkeit der Animation

    private bool isMenuOpen = false;
    private Vector2 closedPosition;
    private Vector2 openPosition;

    void Start()
    {
        // Positionen für geöffnetes und geschlossenes Menü berechnen
        closedPosition = new Vector2(-menuWidth, 0);
        openPosition = new Vector2(100, 0);

        // Startposition auf geschlossen setzen
        menuPanel.anchoredPosition = closedPosition;
    }

    void Update()
    {
        // Umschalten mit einer Taste, z.B. Leertaste
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMenu();
        }

        // Menü-Animation
        if (isMenuOpen)
        {
            menuPanel.anchoredPosition = Vector2.Lerp(menuPanel.anchoredPosition, openPosition, Time.deltaTime * animationSpeed);
        }
        else
        {
            menuPanel.anchoredPosition = Vector2.Lerp(menuPanel.anchoredPosition, closedPosition, Time.deltaTime * animationSpeed);
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
    }
}
