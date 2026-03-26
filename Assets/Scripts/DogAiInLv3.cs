using UnityEngine;
using UnityEngine.AI;
public class DogAiInLv3 : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform playerHand;
    public Transform npc; 
    private Patrol npcScript;

    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private float timer;

    public float sightRange = 15f;

    void Start()
    {
        timer = wanderTimer;

        npcScript = npc.GetComponent<Patrol>();
    }

    void Update()
    {
        bool canSeePlayer = HasLineOfSight();
        bool holdingBone = IsHoldingBone();

        // Chase NPC if it is running
        if (npcScript != null && npcScript.isRunning)
        {
            agent.SetDestination(npc.position);
            agent.speed = 6f;
        }
        // Chase player if holding bone + visible
        else if (holdingBone && canSeePlayer)
        {
            agent.SetDestination(player.position);
            agent.speed = 5f;
        }
        // DEFAULT: Wander
        else
        {
            Wander();
        }
    }

    //LOS check
    bool HasLineOfSight()
    {
        RaycastHit hit;
        Vector3 direction = (player.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, direction, out hit, sightRange))
        {
            if (hit.transform == player)
                return true;
        }
        return false;
    }

    //Check if holding bone
    bool IsHoldingBone()
    {
        if (playerHand.childCount > 0)
        {
            Transform heldObj = playerHand.GetChild(0);
            return heldObj.CompareTag("Bone");
        }
        return false;
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.SetDestination(newPos);
            agent.speed = 2f;
            timer = 0;
        }
    }

    // random point
    public static Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, -1);

        return navHit.position;
    }
}
