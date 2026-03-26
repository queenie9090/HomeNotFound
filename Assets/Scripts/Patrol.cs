using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public GameObject[] waypoints;
    private int currentWP = 0;

    public float speed = 3.5f;
    public float rotationSpeed = 5.0f;
    public float accuracy = 1.0f;

    public bool isRunning = false;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning", false); // default idle
    }

    void Update()
    {
        if (isRunning)
        {
            RunWaypoints();
        }
        else
        {
            Idle();
        }
    }

    void Idle()
    {
        anim.SetBool("isRunning", false);
    }

    void RunWaypoints()
    {
        anim.SetBool("isRunning", true);

        if (waypoints.Length == 0) return;

        GameObject targetNode = waypoints[currentWP];
        Vector3 direction = targetNode.transform.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime
            );
        }

        transform.Translate(0, 0, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetNode.transform.position) < accuracy)
        {
            currentWP++;

            if (currentWP >= waypoints.Length)
            {
                currentWP = 0;
            }
        }
    }

    // Detect bone hit
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bone"))
        {
            isRunning = true; // start running when hit by bone
        }
    }
}