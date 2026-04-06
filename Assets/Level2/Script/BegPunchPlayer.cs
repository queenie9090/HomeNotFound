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

    [Header("Fleeing Settings")]
    public Transform fleePoint; 
    public float fleeSpeed = 5f;
    public float stopDistance = 15f;
    private bool isFleeing = false;
    private bool isScared = false;

    [Header("Components & Scripts")]
    public Animator animator;
    public AudioSource audioSource;
    public MonoBehaviour boysChattingScript;

    [Header("Audio Clips")]
    public AudioClip giveClip;
    public AudioClip rejectClip;
    public AudioClip screamClip;

    [Header("State Tracking")]
    public bool canBeg = false;
    private bool hasBeggedThisTime = false;

    [HideInInspector] public float currentLeftSpeed = 0f;
    [HideInInspector] public float currentRightSpeed = 0f;
    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;

    void Start()
    {
        if (leftHand) lastLeftPos = leftHand.position;
        if (rightHand) lastRightPos = rightHand.position;
    }

    void Update()
    {
        // Continuously track hand speed
        if (leftHand)
        {
            currentLeftSpeed = Vector3.Distance(leftHand.position, lastLeftPos) / Time.deltaTime;
            lastLeftPos = leftHand.position;
        }
        if (rightHand)
        {
            currentRightSpeed = Vector3.Distance(rightHand.position, lastRightPos) / Time.deltaTime;
            lastRightPos = rightHand.position;
        }

        if (isFleeing)
        {
            FleeLogic();
            return;
        }

        if (isScared) return;

        if (canBeg)
        {
            CheckForBegging();
        }
    }

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
                hasBeggedThisTime = true; // Lock it
                Debug.Log("<color=cyan>Begging Locked.</color>");
            }
        }
        // 2. UNLOCK: If the player moves hands apart OR walks out of range
        else if (!isInRange)
        {
            if (hasBeggedThisTime)
            {
                hasBeggedThisTime = false;
                Debug.Log("<color=white>Begging Unlocked.</color> (Hands separated or stepped back)");
            }
        }
    }

    public void OnPunch(ActionBasedController controller)
    {
        if (isScared) return;

        lastActionTime = Time.time;
        isScared = true;
        isFleeing = true;

        // 1. Disable the chatting script
        if (boysChattingScript != null)
        {
            boysChattingScript.enabled = false;
        }

        // 2. Play Audio
        if (audioSource && screamClip) audioSource.PlayOneShot(screamClip);

        // 3. Set Animation State
        if (animator)
        {
            //animator.SetTrigger("RunAway");       // Trigger the start of the transition
            animator.SetBool("IsRunning", true);
        }

        if (controller != null) controller.SendHapticImpulse(0.8f, 0.2f);
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

    void FleeLogic()
    {
        Vector3 targetDirection;
        float distanceToTarget;

        if (fleePoint != null)
        {
            targetDirection = (fleePoint.position - transform.position).normalized;
            distanceToTarget = Vector3.Distance(transform.position, fleePoint.position);
        }
        else
        {
            // FALLBACK: Run away from the player
            targetDirection = (transform.position - playerHead.position).normalized;
            distanceToTarget = Vector3.Distance(transform.position, playerHead.position);
        }

        targetDirection.y = 0; // Keep NPC on the floor

        transform.position += targetDirection * fleeSpeed * Time.deltaTime;

        // rotate the NPC to face the direction they are running
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }

        if (fleePoint != null)
        {
            if (distanceToTarget < 1.5f) StopFleeing(); // Reached the flee point
        }
        else
        {
            if (distanceToTarget > stopDistance) StopFleeing();
        }
    }

    void StopFleeing()
    {
        isFleeing = false;

        Debug.Log("<color=grey>NPC reached the flee point and disappeared.</color>");

        Destroy(gameObject);
    }


    public void SetCanBeg(bool value)
    {
        canBeg = value;

        if (canBeg == false)
        {
            hasBeggedThisTime = false;
            Debug.Log("<color=white>Player left. Begging state fully reset.</color>");
        }
    }
}