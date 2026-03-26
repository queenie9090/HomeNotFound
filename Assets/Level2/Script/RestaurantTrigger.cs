using UnityEngine;

public class RestaurantTrigger : MonoBehaviour
{
    public GameObject journalManager1;
    public GameObject journalManager2;
    public GameObject journalNote1;
    public GameObject journalNote2;

    void Start()
    {
        journalManager2.SetActive(false);
        journalNote2.SetActive(false);
    }

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
                journalManager1.SetActive(false);
                journalManager2.SetActive(true);
                journalNote1.SetActive(false);
                journalNote2.SetActive(true);
                DialogueManager.Instance.ShowDialogue("WHAT?! The food price has increased? Due to increased cost... of course...");
                Debug.Log("Dialog should display");
                gm.SetState(Level2Manager.GameState.DiscoveredPriceIncreased);
            }
            else if (gm.currentState == Level2Manager.GameState.CanBuyFood) //need to change
            {
                gm.SetState(Level2Manager.GameState.LevelComplete);
            }
        }
    }
}