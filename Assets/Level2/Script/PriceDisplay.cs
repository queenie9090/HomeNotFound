using UnityEngine;

using UnityEngine;

public class PriceDisplay : MonoBehaviour
{
    public Renderer renderer;
    public Texture cheapPrice;
    public Texture expensivePrice;

    void Update()
    {
        var state = Level2Manager.Instance.currentState;

        if (state == Level2Manager.GameState.PriceIncreased ||
            state == Level2Manager.GameState.DiscoverBegging ||
            state == Level2Manager.GameState.EarningBegging ||
            state == Level2Manager.GameState.CanBuyFood ||
            state == Level2Manager.GameState.BoughtFood ||
            state == Level2Manager.GameState.LevelComplete)
        {
            renderer.material.mainTexture = expensivePrice;
        }
        else
        {
            renderer.material.mainTexture = cheapPrice;
        }
    }
}