using UnityEngine;
using System.Collections;

public class BaggerTrigger : MonoBehaviour
{

    public bool moneyStole = false;
    public bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (moneyStole)
                StartCoroutine(StoleDialogueSequence());
            else if(!hasTriggered)
                StartCoroutine(DialogueSequence());
        }
    }

    IEnumerator DialogueSequence()
    {
        hasTriggered = true;

        DialogueManager.Instance.ShowDialogue("Damn man...");

        yield return new WaitForSeconds(2.5f);

        DialogueManager.Instance.ShowDialogue("There's money... I'm really starving...");

        yield return new WaitForSeconds(4.5f);

        DialogueManager.Instance.ShowDialogue("Should I take it...? He's blind...");

        yield return new WaitForSeconds(4.5f);

        DialogueManager.Instance.ShowDialogue("What am I even thinking about...");
    }

    IEnumerator StoleDialogueSequence()
    {
        DialogueManager.Instance.ShowDialogue("... ...");

        yield return new WaitForSeconds(2.5f);

        Destroy(gameObject);
    }
}