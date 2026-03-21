using UnityEngine;

public class RecycleBossAnim : MonoBehaviour
{
    public Animator animator; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isPlayerNearby", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isPlayerNearby", false);
        }
    }
}
