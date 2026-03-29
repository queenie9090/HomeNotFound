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

        Vector3 originalPos = rbLegL.transform.localPosition;

        while (timer < struggleDuration)
        {
            // move slightly left
            rbLegL.transform.localPosition = originalPos + new Vector3(-0.05f, 0, 0);
            rbLegR.transform.localPosition = originalPos + new Vector3(-0.05f, 0, 0);
            rbHandL.transform.localPosition = originalPos + new Vector3(-0.05f, 0, 0);
            rbHandR.transform.localPosition = originalPos + new Vector3(-0.05f, 0, 0);

            yield return new WaitForSeconds(0.2f);

            // move back
            rbLegL.transform.localPosition = originalPos;

            yield return new WaitForSeconds(0.2f);

            timer += 0.4f;
        }
    }
}