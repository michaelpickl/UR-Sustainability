using UnityEngine;
using UnityEngine.UI;

public class RankingPanelController : MonoBehaviour
{
    public RectTransform rankingPanel;  
    public float panelWidth = 600f;     
    public float visibleWidth = 350f;
     public float yOffset = -50f;
    public float animationSpeed = 3f;  

    private bool isPanelOpen = false;  
    private Vector2 closedPosition;   
    private Vector2 openPosition;       

    public Button toggleButton;        
    void Start()
    {
        closedPosition = new Vector2(panelWidth, yOffset);   
        openPosition = new Vector2(panelWidth - visibleWidth, yOffset);              

        rankingPanel.anchoredPosition = closedPosition;

        toggleButton.onClick.AddListener(TogglePanel);
    }

    void Update()
    {
    
        if (isPanelOpen)
        {
            rankingPanel.anchoredPosition = Vector2.Lerp(rankingPanel.anchoredPosition, openPosition, Time.deltaTime * animationSpeed);
        }
        else
        {
            rankingPanel.anchoredPosition = Vector2.Lerp(rankingPanel.anchoredPosition, closedPosition, Time.deltaTime * animationSpeed);
        }
    }

    public void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;
    }
}
