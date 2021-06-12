using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A reasonable script for handling dialogue box functionality
/// <br/><br/>
/// Required components: GameObject with a CanvasGroup component (the dialogue box) and a first child with a Text component (the dialogue text)
/// </summary>
public class DialogueScript : MonoBehaviour
{
    [Tooltip("The dialogue box object with a CanvasGroup component and first child with a Text component")]
    public Image dialogueBox;

    [Tooltip("The dialogue text file containing all the lines of dialogue for this instance of the script")]
    public TextAsset dialogueDictionary;

    /// <summary>
    /// The dialogue text component (auto-fetched from the dialogue box)
    /// </summary>
    private Text dialogueText;

    /// <summary>
    /// Whether the dialogue box is currently open or not
    /// </summary>
    [HideInInspector]
    public bool dialogueOpen { get; private set; }

    // The start, end, and current points for the currently open dialogue text
    private int startPos, endPos, currentPos;
    // The set of all dialogue lines in the dictionary
    private string[] lines;
    // The set of lines currently being shown in the dialogue box
    private string[] currentLines;

    // Start is called before the first frame update
    void Start()
    {
        // Auto-fetch the dialogue text component from the top child of the dialogue box
        dialogueText = dialogueBox.gameObject.transform.GetChild(0).GetComponent<Text>();

        // Load the dialogue lines from the dictionary file
        lines = dialogueDictionary.text.Split('\n');

        // Make sure dialogue box starts closed
        CanvasGroup group = dialogueBox.GetComponent<CanvasGroup>();
        group.alpha = 0;
        dialogueOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for continue key and handle continuing or closing the dialogue
        if (Input.GetButtonDown("Jump"))
        {
            currentPos++;
            if (currentPos > endPos)
            {
                // Dialogue done; close the box and unfreeze time
                dialogueBox.GetComponent<CanvasGroup>().alpha = 0;
                dialogueOpen = false;
                Time.timeScale = 1.0f;
            }
            else
            {
                // Show the next line of dialogue
                dialogueText.text = currentLines[currentPos];
            }
        }

        // TODO - Timer-based single-character output (typing effect)
    }

    /// <summary>
    /// Open the dialogue box and show the specified range of messages from the dictionary
    /// </summary>
    /// <param name="start">The line number in the dictionary to start with</param>
    /// <param name="end">The line number in the dictionary to end with</param>
    public void ShowDialogue(int start, int end)
    {
        // Update positions (indexes)
        startPos = start - 1;
        endPos = end - 1;
        currentPos = startPos;

        // Get the set of current lines
        currentLines = new string[end - start + 1];
        for (int i = 0; i < currentLines.Length; i++)
            currentLines[i] = lines[startPos + i];

        // Open the dialogue box
        dialogueText.text = currentLines[currentPos];
        dialogueBox.GetComponent<CanvasGroup>().alpha = 1;
        dialogueOpen = true;

        // Freeze time while dialogue box is open
        Time.timeScale = 0.0f;
    }
}
