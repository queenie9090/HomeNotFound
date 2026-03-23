using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public GameObject gate;

    void Update()
    {
        if (Level2Manager.Instance.currentState ==
            Level2Manager.GameState.LevelComplete)
        {
            gate.SetActive(false); // open path
        }
    }
}