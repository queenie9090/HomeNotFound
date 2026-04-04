using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BegPunchPlayer : MonoBehaviour
{
    public Level2Manager manager;

    [Header("Player References")]
    public Transform playerHead;
    public Transform leftHand;
    public Transform rightHand;

    [Header("Settings")]
    public float begDistance = 3.0f;
    public float handsTogetherThreshold = 0.4f;
    public float actionCooldown = 2.0f;
    private float lastActionTime = 0f;

    [Header("Fleeing")]
    public float fleeSpeed = 5f;
    public float stopDistance = 15f;
    private bool isFleeing = false;
    private bool isScared = false;

    [Header("Components")]
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip giveClip;
    public AudioClip rejectClip;
    public AudioClip screamClip;

    [Header("State Tracking")]
    public bool canBeg = false;
    private bool hasBeggedThisTime = false; // The "One-Shot" Latch

    void Update()
    {
        if (isFleeing)
        {
            FleeFromPlayer();
            return;
        }

        if (isScared) return;

        if (canBeg)
        {
            CheckForBegging();
        }
    }

    // 1. ADDED THE MISSING FUNCTION
    bool CanAct()
    {
        return Time.time > lastActionTime + actionCooldown;
    }

    void CheckForBegging()
    {
        if (leftHand == null || rightHand == null || playerHead == null) return;

        Vector3 npcPos = transform.position;
        Vector3 playerPos = playerHead.position;
        npcPos.y = 0; playerPos.y = 0;

        float distToNPC = Vector3.Distance(playerPos, npcPos);
        float handDist = Vector3.Distance(leftHand.position, rightHand.position);

        // Logic check
        bool isInRange = distToNPC < begDistance;
        bool isHandsTogether = handDist < handsTogetherThreshold;
        bool readyToAct = Time.time > lastActionTime + actionCooldown;

        // 1. TRIGGER: If in range AND hands together AND not locked
        if (isInRange && isHandsTogether)
        {
            if (!hasBeggedThisTime && readyToAct)
            {
                ExecuteBeggingLogic();
                hasBeggedThisTime = true; // Lock it!
                Debug.Log("<color=cyan>Begging Locked.</color>");
            }
        }
        // 2. UNLOCK: If the player moves hands apart OR walks out of range
        else if (!isHandsTogether || !isInRange)
        {
            if (hasBeggedThisTime)
            {
                hasBeggedThisTime = false;
                Debug.Log("<color=white>Begging Unlocked.</color> (Hands separated or stepped back)");
            }
        }
    }

    public void OnPunch(XRBaseController controller)
    {
        if (isScared) return;

        lastActionTime = Time.time;
        isScared = true;
        isFleeing = true;

        if (audioSource && screamClip) audioSource.PlayOneShot(screamClip);
        if (animator) animator.SetTrigger("RunAway");

        Debug.Log("<color=red>NPC Punched!</color>");
    }

    void ExecuteBeggingLogic()
    {
        lastActionTime = Time.time;
        int chance = Random.Range(0, 100);

        Debug.Log("<color=green>Begging Triggered!</color> Roll: " + chance);

        if (chance < 60)
        {
            Level2Manager.Instance.AddMoney(Random.Range(1, 6));
            if (animator) animator.SetTrigger("Give");
            if (audioSource) audioSource.PlayOneShot(giveClip);
        }
        else
        {
            if (animator) animator.SetTrigger("Reject");
            if (audioSource) audioSource.PlayOneShot(rejectClip);
        }
    }

    void FleeFromPlayer()
    {
        Vector3 directionAway = (transform.position - playerHead.position).normalized;
        directionAway.y = 0;
        transform.position += directionAway * fleeSpeed * Time.deltaTime;
        transform.forward = directionAway;

        if (Vector3.Distance(transform.position, playerHead.position) > stopDistance)
        {
            isFleeing = false;
        }
    }

    public void SetCanBeg(bool value)
    {
        canBeg = value;

        // NEW: If the player leaves the NPC's area, 
        // we MUST reset the lock so they can beg again when they return.
        if (canBeg == false)
        {
            hasBeggedThisTime = false;
            Debug.Log("<color=white>Player left. Begging state fully reset.</color>");
        }
    }
}