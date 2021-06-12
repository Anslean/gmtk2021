using UnityEngine;

/// <summary>
/// Trigger script to show a section of dialogue upon entering the trigger
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
    [Tooltip("The dialogue script that links to a dialogue box and dialogue dictionary file")]
    public DialogueScript dialogueScript;

    [Tooltip("Whether the dialogue can be triggered again if the player re-enters the trigger")]
    public bool canBeTriggeredAgain = false;
    private bool triggered = false;

    // The start and end line positions for the dialogue section
    public int startLine = 1;
    public int endLine = 2;

    // Activate dialogue upon trigger entry
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered || canBeTriggeredAgain)
        {
            // Show the dialogue
            dialogueScript.ShowDialogue(startLine, endLine);
        }
    }
}
