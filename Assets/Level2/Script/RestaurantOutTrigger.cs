using UnityEngine;

public class RestaurantOutTrigger : MonoBehaviour
{

    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var gm = Level2Manager.Instance;

            if (gm.currentState == Level2Manager.GameState.NeedMoney)
            {
                DialogueManager.Instance.ShowDialogue("Need to find a way to make money.");
            }
            else if (gm.currentState == Level2Manager.GameState.DiscoveredPriceIncreased)
            {
                DialogueManager.Instance.ShowDialogue("Maybe that's a way to make money faster...");
            }

            gameObject.SetActive(false);

        }
    }
}
