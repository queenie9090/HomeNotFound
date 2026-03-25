using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Header("Navigation Settings")]
    public GameObject[] waypoints;
    private int currentWP = 0;

    [Header("Movement Settings")]
    public float speed = 2.0f;
    public float rotationSpeed = 5.0f;
    public float accuracy = 1.0f;

    [Header("AI Behavior")]
    public Transform player;
    public float detectRange = 10f;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Ensure the enemy starts at the first waypoint if they exist
        if (waypoints.Length > 0)
        {
            currentWP = 0;
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // --- CHASE LOGIC ---
        if (distanceToPlayer < detectRange)
        {
            ChasePlayer();
        }
        // --- PATROL LOGIC ---
        else
        {
            PatrolWaypoints();
        }
    }

    void ChasePlayer()
    {
        anim.SetBool("isChasing", true);

        Vector3 direction = player.position - transform.position;
        // Ignore Y axis to prevent leaning/tilting if player is taller/shorter
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void PatrolWaypoints()
    {
        anim.SetBool("isChasing", false);

        if (waypoints.Length == 0) return;

        // Target the current waypoint
        GameObject targetNode = waypoints[currentWP];
        Vector3 direction = targetNode.transform.position - transform.position;
        direction.y = 0;

        // Rotate and move towards the waypoint
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }

        transform.Translate(0, 0, speed * Time.deltaTime);

        // Check if we reached the waypoint
        if (Vector3.Distance(transform.position, targetNode.transform.position) < accuracy)
        {
            currentWP++;

            // Loop back to the first waypoint if we reached the end
            if (currentWP >= waypoints.Length)
            {
                currentWP = 0;
            }
        }
    }
}