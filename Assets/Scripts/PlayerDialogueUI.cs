using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerDialogueUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Drag your screen text here
    public static PlayerDialogueUI Instance;


    void Awake() => Instance = this;

    // Call this when the dog takes the food
    public void TriggerSnatchedDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(ShowDialogue("Hey! My bread! ...Wait. Look at him. He looks even hungrier than I am... let him have it."));
    }

    // Call this when the player finds clothes
    public void TriggerWarmthDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(ShowDialogue("Found something warm. One less thing to worry about tonight."));
    }

    IEnumerator ShowDialogue(string message)
    {
        dialogueText.text = message;
        dialogueText.color = Color.yellow; // Make new thoughts stand out

        yield return new WaitForSeconds(5f); // Show for 5 seconds

        dialogueText.color = Color.white;
        dialogueText.text = "... (Back to survival)";
    }
}