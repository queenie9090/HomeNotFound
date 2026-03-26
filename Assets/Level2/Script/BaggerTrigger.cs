using UnityEngine;
using System.Collections;

public class BaggerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DialogueSequence());
        }
    }

    IEnumerator DialogueSequence()
    {
        DialogueManager.Instance.ShowDialogue("Damn man...");

        yield return new WaitForSeconds(2.5f);

        DialogueManager.Instance.ShowDialogue("There's money... I'm really starving...");

        yield return new WaitForSeconds(4.5f);

        DialogueManager.Instance.ShowDialogue("Should I take it...? He's blind...");

        yield return new WaitForSeconds(4.5f);

        DialogueManager.Instance.ShowDialogue("What am I even thinking about...");
    }
}