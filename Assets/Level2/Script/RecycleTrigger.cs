using UnityEngine;

public class RecycleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var gm = Level2Manager.Instance;

            if (gm.currentState == Level2Manager.GameState.NeedMoney)
            {
                gm.SetState(Level2Manager.GameState.DiscoverRecycle);
            }
        }
    }
}