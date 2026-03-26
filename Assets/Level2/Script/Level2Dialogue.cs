using UnityEngine;
using TMPro;
using System.Collections;

public class Level2Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Drag your screen text here
    public static Level2Dialogue Instance;


    void Awake() => Instance = this;

    public void EnoughMoneyDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(ShowDialogue("Finally, my have enough money to buy food, lets head back to the food stall."));
    }

    public void DiscoveredPriceIncreasedDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(ShowDialogue("WHAT?! The food price has increased? Due to increased cost... of course..."));
    }

    public void DiscoveredBeggingDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(ShowDialogue("Maybe I can try to beg..."));
    }

    public void FinallyEnoughMoneyDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(ShowDialogue("Finally! I have enough money! Lets go check out that Mamak again!"));
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