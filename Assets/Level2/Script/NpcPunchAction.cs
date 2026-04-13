using UnityEngine;
using System.Collections;

public class NpcPunchAction : MonoBehaviour
{
    [Header("Targeting")]
    public Transform playerHead;
    public PlayerAcceptPunch playerAcceptPunch;

    [Header("Animation & Audio")]
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip punchDialogueClip;

    [Header("Settings")]
    public float turnSpeed = 5f;
    public float punchWaitTime = 2.0f;

    private Vector3 originalPosition; // Re-add this
    private Quaternion originalRotation;
    private bool isAttacking = false;


    void Start()
    {
        // Remember which way they were facing when the game started
        originalRotation = transform.rotation;
        originalPosition = transform.position; 
    }

    public void ExecutePunch(BegPunchPlayer mainScript)
    {
        if (!isAttacking)
        {
            StartCoroutine(PunchSequence(mainScript));
        }
    }

    private IEnumerator PunchSequence(BegPunchPlayer mainScript)
    {
        isAttacking = true;

        playerAcceptPunch.ReactToPunch();

        // 1. Turn to face the player instantly
        Vector3 directionToPlayer = (playerHead.position - transform.position).normalized;
        directionToPlayer.y = 0; // Keep them standing straight
        if (directionToPlayer != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }

        // 2. Play the voice line 
        if (audioSource != null && punchDialogueClip != null)
        {
            audioSource.PlayOneShot(punchDialogueClip);
        }

        // 3. Trigger the punch animation
        if (animator != null)
        {
            animator.SetTrigger("Punch");
        }

        // 4. Wait for the punch animation to finish playing
        yield return new WaitForSeconds(punchWaitTime);

        // 5. Smoothly rotate back to the original facing direction
        while (Quaternion.Angle(transform.rotation, originalRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * turnSpeed);
            yield return null;
        }

        // 6. Snap to exact original rotation just to be perfectly aligned
        transform.rotation = originalRotation;
        transform.position = originalPosition;

        isAttacking = false;

        // 7. Tell the main script we are done so it can unlock and resume chatting!
        if (mainScript != null)
        {
            mainScript.ResumeNormalActivity();
        }
    }
}