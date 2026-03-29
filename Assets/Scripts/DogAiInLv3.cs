using UnityEngine;

public class DogAiInLv3 : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform playerHand;
    public Transform npc;
    private NpcBoneDetector npcDetector;
    private GameObject cachedBone;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 8f;
    public float stopDistance = 1.5f;
    public LayerMask obstacleMask; // <--- ADD THIS LINE HERE

    [Header("Wander")]
    public float wanderRadius = 5f;
    private Vector3 wanderTarget;
    private float wanderTimer;

    void Start()
    {
        cachedBone = GameObject.FindGameObjectWithTag("Bone");
        if (npc != null) npcDetector = npc.GetComponent<NpcBoneDetector>();
        PickNewWanderTarget();
    }

    void Update()
    {
        bool blocked = IsPathBlocked();

        if (blocked)
        {
            // SPIN if hitting wall or edge
            transform.Rotate(Vector3.up * rotationSpeed * 15f * Time.deltaTime);
        }
        else
        {
            // LOGIC
            if (npcDetector != null && npcDetector.isTargetedByDog)
            {
                PursueTarget(npc.position);
            }
            else if (IsHoldingBone())
            {
                PursueTarget(player.position);
            }
            else
            {
                Wander();
            }
        }
    }

    // Updated Block Check
    bool IsPathBlocked()
    {
        // Offset the start so it doesn't hit the dog's own model
        Vector3 forwardOffset = transform.forward * 0.6f;
        Vector3 rayOrigin = transform.position + Vector3.up + forwardOffset;

        // Wall Check
        if (Physics.Raycast(rayOrigin, transform.forward, 1.0f, obstacleMask)) return true;

        // Edge Check
        Vector3 edgePos = transform.position + (transform.forward * 1.5f) + Vector3.up;
        if (!Physics.Raycast(edgePos, Vector3.down, 2.5f, obstacleMask)) return true;

        return false;
    }

    void PursueTarget(Vector3 targetPos)
    {
        float distance = Vector3.Distance(transform.position, targetPos);

        // Rotate to face target
        Vector3 dir = (targetPos - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
        }

        // Move forward
        if (distance > stopDistance)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    bool IsHoldingBone()
    {
        if (cachedBone == null)
        {
            cachedBone = GameObject.FindGameObjectWithTag("Bone");
            return false;
        }
        return Vector3.Distance(playerHand.position, cachedBone.transform.position) < 0.6f;
    }

    void Wander()
    {
        wanderTimer += Time.deltaTime;

        // Move forward at half speed while wandering
        transform.Translate(Vector3.forward * (moveSpeed * 0.5f) * Time.deltaTime);

        // Look towards the wander target
        Vector3 dir = (wanderTarget - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
        }

        // Pick a new target if we reached the old one or time ran out
        if (wanderTimer > 4f || Vector3.Distance(transform.position, wanderTarget) < 1.5f)
        {
            PickNewWanderTarget();
        }
    }

    void PickNewWanderTarget()
    {
        wanderTarget = transform.position + (Random.insideUnitSphere * wanderRadius);
        wanderTarget.y = transform.position.y;
        wanderTimer = 0;
    }
}