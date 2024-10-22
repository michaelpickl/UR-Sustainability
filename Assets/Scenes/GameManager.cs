using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public InputField playerNameInput;
    public Text introText;
    private string playerName;

    void Start()
    {
        introText.text = "Willkommen zum Spiel! Bitte trage deinen Namen ein."; // Intro-Text
    }

    public void StartGame()
    {
        playerName = playerNameInput.text;  // Spielername speichern
        // Spielername kann hier an den nächsten Spielablauf übergeben werden, z.B. PlayerPrefs
        PlayerPrefs.SetString("PlayerName", playerName); // Speichername für nächste Szene
        SceneManager.LoadScene("GameScene");  // Wechsel zu Spielszene
    }
}
