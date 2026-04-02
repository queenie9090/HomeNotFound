using UnityEngine;

public class NpcBoneDetector : MonoBehaviour
{
    public bool isTargetedByDog = false;

    [Header("Animation")]
    public Animator anim; // Drag your NPC's Animator here

    [Header("Running Settings")]
    public Transform[] waypoints;
    public float runSpeed = 4f;
    public float rotationSpeed = 5f;

    private int currentWaypointIndex = 0;
    private bool isRunning = false;

    private void Start()
    {
        // Automatically try to find the animator if not assigned
        if (anim == null) anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bone")) StartRunning();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bone")) StartRunning();
    }

    void StartRunning()
    {
        if (waypoints.Length > 0 && !isRunning)
        {
            isTargetedByDog = true;
            isRunning = true;

            // TRIGGER ANIMATION
            if (anim != null) anim.SetBool("isChasing", true);
        }
    }

    void Update()
    {
        if (isRunning) RunThroughWaypoints();
    }

    void RunThroughWaypoints()
    {
        if (currentWaypointIndex >= waypoints.Length)
        {
            isRunning = false;
            if (anim != null) anim.SetBool("isChasing", false); // Stop animation
            return;
        }

        Transform target = waypoints[currentWaypointIndex];

        // Rotation and Move logic
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
        }

        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 1.0f)
        {
            currentWaypointIndex++;
        }
    }
}