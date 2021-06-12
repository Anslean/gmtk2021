using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIScript : MonoBehaviour
{
    public CanvasGroup pauseGroup;
    [HideInInspector]
    public static bool isPaused;

    public Button continueButton;
    public Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        // Start unpaused
        pauseGroup.alpha = 0;
        isPaused = false;

        // Set up buttons
        continueButton.onClick.AddListener(ContinueClicked);
        quitButton.onClick.AddListener(QuitClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    // Toggle the pause state
    void TogglePause()
    {
        if (!isPaused)
        {
            pauseGroup.alpha = 1;
            Time.timeScale = 0.0f;
            isPaused = true;
            print("Game paused!");
        }
        else
        {
            pauseGroup.alpha = 0;
            Time.timeScale = 1.0f;
            isPaused = false;
        }
    }

    // Just unpause the game to continue
    void ContinueClicked()
    {
        TogglePause();
    }

    // Quit the game when Quit is clicked
    void QuitClicked()
    {
        Application.Quit();
        print("Game quit called");
    }
}
