using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FoodBuyButton : MonoBehaviour
{
    public Level2Manager manager;

    [Header("Button Animation")]
    public float pressDepth = 0.02f;
    public float returnSpeed = 5f;
    public int foodCost = 25;

    public GameObject foodPrefab;
    private bool boughtFood = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip failSound; 

    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.localPosition;
        foodPrefab.SetActive(false);
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            initialPos,
            Time.deltaTime * returnSpeed
        );
    }

    public void OnRaySelect()
    {
        transform.localPosition = initialPos - new Vector3(0, 0, pressDepth);

        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        if (boughtFood)
        {
            DialogueManager.Instance.ShowDialogue("Don't waste money on extra food...");
            return;
        }


        // Not enough money
        if (manager.playerMoney < foodCost)
        {
            if (audioSource != null && failSound != null)
                audioSource.PlayOneShot(failSound);

            return; 
        }

        if (foodPrefab != null)
        {
            foodPrefab.SetActive(true);
            boughtFood = true;
        }

        manager.DecreaseMoney(foodCost);
    }
}