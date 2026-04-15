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

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isAttacking = false;


    void Start()
    {
        // Remember which way they were facing
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

        // Turn to face the player 
        Vector3 directionToPlayer = (playerHead.position - transform.position).normalized;
        directionToPlayer.y = 0; // Keep them standing straight
        if (directionToPlayer != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }

        if (audioSource != null && punchDialogueClip != null)
        {
            audioSource.PlayOneShot(punchDialogueClip);
        }

        if (animator != null)
        {
            animator.SetTrigger("Punch");
        }

        yield return new WaitForSeconds(punchWaitTime);


        while (Quaternion.Angle(transform.rotation, originalRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * turnSpeed);
            yield return null;
        }

        transform.rotation = originalRotation;
        transform.position = originalPosition;

        isAttacking = false;

        if (mainScript != null)
        {
            mainScript.ResumeNormalActivity();
        }
    }
}