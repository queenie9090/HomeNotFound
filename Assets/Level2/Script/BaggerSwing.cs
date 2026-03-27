using UnityEngine;
using System.Collections;

public class BaggerSwing : MonoBehaviour
{
    public Rigidbody rbHead;
    public Rigidbody rbHandL;
    public Rigidbody rbHandR;
    public Rigidbody rbLegL;
    public Rigidbody rbLegR;

    public float struggleDuration = 8f;

    void Start()
    {
        StartCoroutine(Struggling());
    }

    IEnumerator Struggling()
    {
        float timer = 0f;

        while (timer < struggleDuration)
        {
            rbHead.AddForce(Vector3.forward * 2f, ForceMode.Impulse);
            rbHandL.AddForce(Vector3.forward * 2f, ForceMode.Impulse);
            rbHandR.AddForce(Vector3.forward * 2f, ForceMode.Impulse);
            rbLegL.AddForce(Vector3.forward * 2f, ForceMode.Impulse);
            rbHead.AddForce(Vector3.forward * 2f, ForceMode.Impulse);

            timer += Time.deltaTime;
        }
        yield return new WaitForSeconds(1.5f);
    }
}