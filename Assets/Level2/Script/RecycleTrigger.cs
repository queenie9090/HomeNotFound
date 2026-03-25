using UnityEngine;
using System.Collections.Generic;

public class RecycleTrigger : MonoBehaviour
{

    public EnableRecycle enableRecycle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var gm = Level2Manager.Instance;

            if (gm.currentState == Level2Manager.GameState.NeedMoney)
            {
                enableRecycle.SetActiveRecycle();
                gm.SetState(Level2Manager.GameState.DiscoverRecycle);
            }
        }
    }
}