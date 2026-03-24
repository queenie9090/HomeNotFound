using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    public static Level2Manager Instance;

    public enum GameState
    {
        EnterLevel,
        NeedMoney,
        DiscoverRecycle,
        //EarnedRecycle,
        PriceIncreased,
        DiscoveredPriceIncreased,
        DiscoverBegging,
        EarningBegging,
        CanBuyFood,
        BoughtFood,
        LevelComplete
    }

    public GameState currentState;

    public int playerMoney = 0;
    public int foodPrice = 13;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetState(GameState.EnterLevel);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Game State: " + newState);

        switch (newState)
        {
            case GameState.EnterLevel:
                break;

            case GameState.NeedMoney:
                // Show UI: "You need money"
                break;

            case GameState.DiscoverRecycle:
                // Highlight recycle shop
                break;

            case GameState.PriceIncreased:
                foodPrice = 25;
                break;

            case GameState.DiscoverBegging:
                // Enable begging mechanic
                break;

            case GameState.CanBuyFood:
                // Allow buying
                break;

            case GameState.BoughtFood:
                // Allow eating
                break;

            case GameState.LevelComplete:
                // Unlock next level
                break;
        }
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        CheckProgress();
    }

    public void DecreaseMoney(int amount)
    {
        playerMoney -= amount;
        CheckProgress();
    }

    void CheckProgress()
    {
        if (playerMoney >= foodPrice)
        {
            if (currentState == GameState.DiscoverRecycle)
            {
                SetState(GameState.PriceIncreased);
            }
            else if (currentState == GameState.EarningBegging)
            {
                SetState(GameState.CanBuyFood);
            }
        }
    }
}