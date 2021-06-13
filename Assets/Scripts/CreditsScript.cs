using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour
{
    public Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        // Set up buttons
        backButton.onClick.AddListener(BackClicked);
    }

    // Start the game
    void BackClicked()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("MainMenu"));
    }
}
