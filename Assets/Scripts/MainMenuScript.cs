using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Button startButton;
    public Button creditsButton;
    public Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        // Set up buttons
        startButton.onClick.AddListener(StartClicked);
        creditsButton.onClick.AddListener(CreditsClicked);
        quitButton.onClick.AddListener(QuitClicked);
    }

    // Start the game
    void StartClicked()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("PuzzleMap"));
    }

    // View the credits
    void CreditsClicked()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("Credits"));
    }

    // Quit the game when Quit is clicked
    void QuitClicked()
    {
        Application.Quit();
        print("Game quit called");
    }
}
