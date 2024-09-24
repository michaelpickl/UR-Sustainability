using UnityEngine;
using TMPro;

public class SideMenuController : MonoBehaviour
{
    public RectTransform menuPanel;
    public float menuWidth = 800f;
    public float animationSpeed = 3f;
    public TextMeshProUGUI  buildingNameText;

    private bool isMenuOpen = false;
    private Vector2 closedPosition;
    private Vector2 openPosition;

    void Start()
    {
        closedPosition = new Vector2(-menuWidth, 0);
        openPosition = new Vector2(100, 0);

        menuPanel.anchoredPosition = closedPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMenu();
        }

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

    public void OpenMenuWithBuildingName(string buildingName)
    {
        if(buildingNameText != null)
        {
            buildingNameText.text = buildingName;
        }
        if (!isMenuOpen)
        {
            ToggleMenu();
        }
    }
}
