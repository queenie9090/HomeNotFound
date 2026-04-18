using UnityEngine;

public class ToiletBlocker : MonoBehaviour
{
    public Animator npcAnim;
   

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Hand")) return;

        if (!NpcBoneDetector.npcDistracted && !hasTriggered)
        {
            hasTriggered = true;

            Debug.Log("NPC attacks!");

            if (npcAnim != null)
            {
                // FIXED: Changed SetTrigger("Hit") to SetBool("isAttacking", true)
                npcAnim.SetBool("isAttacking", true);

                // NEW: Turn off the animation after 1.5 seconds so it doesn't loop forever
                Invoke(nameof(ResetAttackAnimation), 1.5f);
            }

            // Play the sound
            NpcBoneDetector detector = npcAnim.GetComponent<NpcBoneDetector>();
            if (detector != null)
            {
                detector.PlayWarningSound();
            }
        }
    }

    void ResetAttackAnimation()
    {
        if (npcAnim != null)
        {
            // Turn the boolean off to return to Idle
            npcAnim.SetBool("isAttacking", false);
        }

        hasTriggered = false; 
    }
}