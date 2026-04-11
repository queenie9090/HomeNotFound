using UnityEngine;
using System.Collections;

public class IntroSequence_lvl1 : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ExecuteIntro());
    }

    IEnumerator ExecuteIntro()
    {
        // 1. Start in the dark
        yield return new WaitForSeconds(1.5f);
        DialogueManager.Instance.ShowDialogue("It’s cold… raining again…");

        // 2. Wait for the fade to reveal the room
        yield return new WaitForSeconds(5.0f);
        DialogueManager.Instance.ShowDialogue("Nothing left but this…");

        // 3. The final "Mission" statement
        yield return new WaitForSeconds(5.7f);
        DialogueManager.Instance.ShowDialogue("I just need to survive today.");

        // 3. The final "Mission" statement
        yield return new WaitForSeconds(6.1f);
        DialogueManager.Instance.ShowDialogue("Press <color=yellow>(Y)</color> for Journal");

        // At this point, the player is fully "in" the game and looking for the Journal.
    }
}