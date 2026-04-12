using UnityEngine;

public class BegTrigger : MonoBehaviour
{
    public BegPunchPlayer npc;
    public StealPunchPlayer stealNPC;
    public string dialogueText; //I need to earn some money... Maybe I can beg for some change?

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var gm = Level2Manager.Instance;
            if (gm.currentState == Level2Manager.GameState.DiscoveredPriceIncreased)
            { 
            DialogueManager.Instance.ShowDialogue(dialogueText);
            if(npc != null && stealNPC == null)
                npc.SetCanBeg(true);
            else if(stealNPC != null && npc == null)
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
                npc.SetCanBeg(false);
            else if (stealNPC != null && npc == null)
                stealNPC.SetCanSteal(false);
        }
    }
}
