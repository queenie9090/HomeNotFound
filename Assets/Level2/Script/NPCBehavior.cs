using UnityEngine;

using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;

    public int minMoney = 1;
    public int maxMoney = 5;

    public void OnBeg()
    {
        int chance = Random.Range(0, 100);

        if (chance < 50)
        {
            // GIVE MONEY
            int money = Random.Range(minMoney, maxMoney);
            Level2Manager.Instance.AddMoney(money);

            animator.SetTrigger("Give");
            // play dialogue
        }
        else
        {
            // REJECT
            animator.SetTrigger("Reject");
        }
    }

    public void OnPunch()
    {
        animator.SetTrigger("RunAway");
        // play scream
        // disable interaction
        GetComponent<Collider>().enabled = false;
    }
}