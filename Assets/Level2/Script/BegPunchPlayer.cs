using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BegPunchPlayer : MonoBehaviour
{
    public Level2Manager manager;

    [Header("Player References")]
    public Transform playerHead;
    public Transform leftHand;
    public Transform rightHand;

    [Header("Retaliation Settings")]
    public NpcPunchAction punchActionScript;
    [Range(0, 100)] public int fightBackChance = 50;

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
    public bool canInteract = false;
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

        if (canInteract)
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

        
        bool isInRange = distToNPC < begDistance;
        bool isHandsTogether = handDist < handsTogetherThreshold;
        bool readyToAct = Time.time > lastActionTime + actionCooldown;

        if (isInRange && isHandsTogether)
        {
            if (!hasBeggedThisTime && readyToAct)
            {
                ExecuteBeggingLogic();
                hasBeggedThisTime = true;
                Debug.Log("<color=cyan>Begging Locked.</color>");
            }
        }
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
        if (!canInteract)
        {
            Debug.Log("<color=grey>Punch ignored. Player has not unlocked interaction yet.</color>");
            return;
        }

        if (isScared) return;

        lastActionTime = Time.time;
        isScared = true;

        if (controller != null) controller.SendHapticImpulse(0.8f, 0.2f);

        int roll = Random.Range(0, 100);

        if (roll < fightBackChance && punchActionScript != null)
        {
            Debug.Log("<color=red>NPC is Fighting Back!</color>");

            if (boysChattingScript != null) boysChattingScript.enabled = false;

            punchActionScript.ExecutePunch(this);
        }
        else
        {
            isFleeing = true;
            if (boysChattingScript != null) boysChattingScript.enabled = false;
            if (audioSource && screamClip) audioSource.PlayOneShot(screamClip);
            if (animator) animator.SetBool("IsRunning", true);

            Debug.Log("<color=red>NPC Punched! Fleeing...</color>");
        }
    }

    public void ResumeNormalActivity()
    {
        isScared = false;

        if (boysChattingScript != null) boysChattingScript.enabled = true;

        Debug.Log("<color=white>NPC calmed down and resumed normal activity.</color>");
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
            targetDirection = (transform.position - playerHead.position).normalized;
            distanceToTarget = Vector3.Distance(transform.position, playerHead.position);
        }

        targetDirection.y = 0;

        transform.position += targetDirection * fleeSpeed * Time.deltaTime;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }

        if (fleePoint != null)
        {
            if (distanceToTarget < 1.5f) StopFleeing();
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

    public void SetCanInteract(bool value)
    {
        canInteract = value;

        if (canInteract == false)
        {
            hasBeggedThisTime = false;
            Debug.Log("<color=white>Player left. Interaction state fully reset.</color>");
        }
    }
}