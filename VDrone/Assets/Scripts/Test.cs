using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = float.PositiveInfinity;
        rb.AddRelativeTorque(Vector3.up * .0698f,ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
    }

    private void Reset()
    {
    }
}
