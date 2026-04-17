using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StealPunchPlayer : MonoBehaviour
{

    public GameObject moneyTrue;
    public GameObject moneyFalse;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyTrue.SetActive(false);
        moneyFalse.SetActive(true);
    }

    public void SetCanSteal(bool canSteal)
    {
        if(canSteal)
            {
            moneyTrue.SetActive(true);
            moneyFalse.SetActive(false);
        }
        else
        {
            //moneyTrue.SetActive(false);
            moneyFalse.SetActive(true);
        }

    }

    void Update()
    {
        
    }
}
