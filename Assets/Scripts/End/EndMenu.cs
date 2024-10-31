using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndMenu : MonoBehaviour
{
    public TMP_Text textMeasuresAndCO2;
    public Button buttonCredits;

    public Button buttonRestart;
    public Button buttonBack;

    public GameObject endPanel;
    public GameObject creditsPanel;

    private void Start()
    {
        string textMeasures = "<b>" + PlayerPrefs.GetInt("MeasuresDone") + "/" + PlayerPrefs.GetInt("MeasuresAll") + "</b>";
        string textC02 = "<b>" + PlayerPrefs.GetInt("CO2%Saved") + "%</b>";
        textMeasuresAndCO2.text = "Immerhin konntest du " + textMeasures + " der Maßnahmen umsetzen und damit " + textC02 + " des jährlichen CO2-Ausstoßes der Universität Regensburg einsparen.";
    }

    public void ShowCredits()
    {
        endPanel.SetActive(false);
        creditsPanel.SetActive(true);
        buttonCredits.gameObject.SetActive(false);
        buttonBack.gameObject.SetActive(true);
    }

    public void ShowEnd()
    {
        endPanel.SetActive(true);
        creditsPanel.SetActive(false);
        buttonCredits.gameObject.SetActive(true);
        buttonBack.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Start");
    }
}
