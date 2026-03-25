using UnityEngine;

public class BegTrigger : MonoBehaviour
{
    public BegPunchPlayer npc;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var gm = Level2Manager.Instance;
            if (gm.currentState == Level2Manager.GameState.DiscoveredPriceIncreased)
                npc.SetCanBeg(true);
            else
                return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npc.SetCanBeg(false);
        }
    }
}
