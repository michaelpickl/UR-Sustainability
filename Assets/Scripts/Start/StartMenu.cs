using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour
{
    public TMP_Text textHeadingIntro;
    public TMP_Text textHeadingControl;
    public TMP_Text textIntro;
    public TMP_Text textControl;

    public Button buttonNext;
    public Button buttonBack;
    public Button buttonStart;

    private void Start()
    {
        ShowIntro(); // Startet mit dem Intro-Text
    }

    public void ShowIntro()
    {
        textHeadingIntro.gameObject.SetActive(true);
        textIntro.gameObject.SetActive(true);
        buttonNext.gameObject.SetActive(true);

        textHeadingControl.gameObject.SetActive(false);
        textControl.gameObject.SetActive(false);
        buttonBack.gameObject.SetActive(false);
        buttonStart.gameObject.SetActive(false);
    }

    public void ShowControls()
    {
        textHeadingIntro.gameObject.SetActive(false);
        textIntro.gameObject.SetActive(false);
        textHeadingControl.gameObject.SetActive(true);
        textControl.gameObject.SetActive(true);
        buttonBack.gameObject.SetActive(true);
        buttonStart.gameObject.SetActive(true);

        buttonNext.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Gebäude");
    }
}
