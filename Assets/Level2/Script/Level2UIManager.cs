using UnityEngine;
using TMPro;

public class Level2UIManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI moneyText;
    //public TextMeshProUGUI priceText;

    private Level2Manager gm;

    void Start()
    {
        gm = Level2Manager.Instance;
        UpdateUI();
    }

    void Update()
    {
        UpdateMoney();
        UpdatePrice();
        UpdateObjective();
    }

    void UpdateMoney()
    {
        moneyText.text = "Money: RM " + gm.playerMoney;
    }

    void UpdatePrice()
    {
        //priceText.text = "Food Price: RM " + gm.foodPrice;
    }

    void UpdateObjective()
    {
        switch (gm.currentState)
        {
            case Level2Manager.GameState.EnterLevel:
                objectiveText.text = "Find food, you are starving...";
                break;

            case Level2Manager.GameState.NeedMoney:
                objectiveText.text = "You need money to buy food.";
                break;

            case Level2Manager.GameState.DiscoverRecycle:
                objectiveText.text = "Find recycle items to sell...";
                break;

            case Level2Manager.GameState.PriceIncreased:
                objectiveText.text = "Great! Now I've enough money!";
                break;

            case Level2Manager.GameState.DiscoveredPriceIncreased:
                objectiveText.text = "WHAT?! The food price has increased!";
                break;

            case Level2Manager.GameState.DiscoverBegging:
                objectiveText.text = "Try begging people for money.";
                break;

            case Level2Manager.GameState.EarningBegging:
                objectiveText.text = "Beg NPCs to earn money.";
                break;

            case Level2Manager.GameState.CanBuyFood:
                objectiveText.text = "I can now buy food!";
                break;

            case Level2Manager.GameState.BoughtFood:
                objectiveText.text = "Eat the food.";
                break;

            case Level2Manager.GameState.LevelComplete:
                objectiveText.text = "Level Complete!";
                break;
        }
    }

    void UpdateUI()
    {
        UpdateMoney();
        UpdatePrice();
        UpdateObjective();
    }
}