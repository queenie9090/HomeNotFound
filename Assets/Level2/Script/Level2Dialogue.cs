using UnityEngine;
using TMPro;
using System.Collections;

public class Level2Dialogue : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ExecuteIntro());
    }

    IEnumerator ExecuteIntro()
    {
        // 1. Start in the dark
        yield return new WaitForSeconds(1.0f);
        DialogueManager.Instance.ShowDialogue("Street full of people...");

        // 2. Wait for the fade to reveal the room
        yield return new WaitForSeconds(5.5f);
        DialogueManager.Instance.ShowDialogue("Great, just what I need right now...");

        // 3. The final "Mission" statement
        yield return new WaitForSeconds(5.8f);
        DialogueManager.Instance.ShowDialogue("Find some food to eat first, I'm starving...");

        // At this point, the player is fully "in" the game and looking for the Journal.
    }
}