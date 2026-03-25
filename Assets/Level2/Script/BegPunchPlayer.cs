using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BegPunchPlayer : MonoBehaviour
{
    public Level2Manager manager;

    [Header("Player")]
    public Transform player;
    public float activationDistance = 2.0f;

    [Header("Flee Settings")]
    public float fleeSpeed = 5f;
    public float safeDistance = 10f;
    public float stopDistance = 15f;
    private bool isFleeing = false;

    [Header("Audio")]
    public AudioClip giveClip;
    public AudioClip rejectClip;
    public AudioClip screamClip;

    [Header("Hand References")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("Interaction Settings")]
    public float interactDistance = 1.5f;
    public float punchVelocityThreshold = 2.5f;
    public float begVelocityThreshold = 1.0f;

    [Header("Direction Check")]
    public float punchDotThreshold = 0.5f;

    [Header("Cooldown")]
    public float actionCooldown = 1.5f;
    private float lastActionTime = 0f;

    [Header("Money")]
    public int minMoney = 1;
    public int maxMoney = 5;

    [Header("Components")]
    public Animator animator;
    public AudioSource audioSource;

    [Header("Optional Haptics")]
    public XRBaseController leftController;
    public XRBaseController rightController;

    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;

    private bool isScared = false;
    public bool canBeg = false;
    private bool hasTriggeredBegging = false;

    void Start()
    {
        if (leftHand != null) lastLeftPos = leftHand.position;
        if (rightHand != null) lastRightPos = rightHand.position;
    }

    void Update()
    {
        if (isFleeing)
        {
            FleeFromPlayer();
            return;
        }

        if (!canBeg) return;
        if (isScared) return;

        DetectHand(leftHand, ref lastLeftPos, leftController);
        DetectHand(rightHand, ref lastRightPos, rightController);
    }

    public void SetCanBeg(bool value)
    {
        canBeg = value;

        if (canBeg && !hasTriggeredBegging)
        {
            manager.SetState(Level2Manager.GameState.DiscoverBegging);
            hasTriggeredBegging = true;
        }
    }

    void DetectHand(Transform hand, ref Vector3 lastPos, XRBaseController controller)
    {
        if (hand == null) return;

        float distance = Vector3.Distance(hand.position, transform.position);

        // Calculate velocity
        float velocity = (hand.position - lastPos).magnitude / Time.deltaTime;

        // Direction check (is hand moving toward NPC)
        Vector3 toNPC = (transform.position - hand.position).normalized;
        Vector3 handDir = (hand.position - lastPos).normalized;
        float dot = Vector3.Dot(handDir, toNPC);

        if (distance < interactDistance && CanAct())
        {
            if (velocity > punchVelocityThreshold && dot > punchDotThreshold)
            {
                OnPunch(controller);
            }
            else if (velocity > 0.1f && velocity < begVelocityThreshold)
            {
                OnBeg(controller);
            }
        }

        lastPos = hand.position;
    }

    bool CanAct()
    {
        return Time.time > lastActionTime + actionCooldown;
    }

    void OnBeg(XRBaseController controller)
    {
        lastActionTime = Time.time;

        int chance = Random.Range(0, 100);

        if (chance < 60)
        {
            int money = Random.Range(minMoney, maxMoney + 1);
            Level2Manager.Instance.AddMoney(money);

            animator.SetTrigger("Give");
            audioSource.PlayOneShot(giveClip);

            SendHaptics(controller, 0.3f, 0.2f);
        }
        else
        {
            animator.SetTrigger("Reject");
            audioSource.PlayOneShot(rejectClip);
        }
    }

    void FleeFromPlayer()
    {
        if (player == null) return;

        Vector3 directionAway = (transform.position - player.position).normalized;

        // Move away
        transform.position += directionAway * fleeSpeed * Time.deltaTime;

        // Face movement direction
        transform.forward = directionAway;

        float distance = Vector3.Distance(transform.position, player.position);

        // Stop when far enough
        if (distance > stopDistance)
        {
            isFleeing = false;
        }
    }

    void OnPunch(XRBaseController controller)
    {
        lastActionTime = Time.time;

        animator.SetTrigger("RunAway");

        SendHaptics(controller, 0.8f, 0.3f);

        isScared = true;
        isFleeing = true;

        // Optional: disable interaction
        GetComponent<Collider>().enabled = false;
    }

    void SendHaptics(XRBaseController controller, float amplitude, float duration)
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(amplitude, duration);
        }
    }
}