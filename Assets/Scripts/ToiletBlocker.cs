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
                npcAnim.SetBool("isAttacking", true);

                Invoke(nameof(ResetAttackAnimation), 1.5f);
            }

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
            npcAnim.SetBool("isAttacking", false);
        }

        hasTriggered = false; 
    }
}