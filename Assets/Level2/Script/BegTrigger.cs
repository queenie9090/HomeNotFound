using UnityEngine;

public class BegTrigger : MonoBehaviour
{
    public BegPunchPlayer npc;
    public StealPunchPlayer stealNPC;
    public string dialogueText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var gm = Level2Manager.Instance;
            if (gm.currentState == Level2Manager.GameState.DiscoveredPriceIncreased)
            {
                DialogueManager.Instance.ShowDialogue(dialogueText);

                if (npc != null && stealNPC == null)
                    npc.SetCanInteract(true);
                else if (stealNPC != null && npc == null)
                    stealNPC.SetCanSteal(true);
            }
            else
                return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (npc != null && stealNPC == null)
                npc.SetCanInteract(false);
            else if (stealNPC != null && npc == null)
                stealNPC.SetCanSteal(false);
        }
    }
}