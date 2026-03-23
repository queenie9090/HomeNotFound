using UnityEngine;

public class RestaurantTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var gm = Level2Manager.Instance;

            if (gm.currentState == Level2Manager.GameState.EnterLevel)
            {
                gm.SetState(Level2Manager.GameState.NeedMoney);
            }
            if (gm.currentState == Level2Manager.GameState.PriceIncreased)
            {
                gm.SetState(Level2Manager.GameState.DiscoveredPriceIncreased);
            }
            else if (gm.currentState == Level2Manager.GameState.CanBuyFood) //need to change
            {
                gm.SetState(Level2Manager.GameState.LevelComplete);
            }
        }
    }
}