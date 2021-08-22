using UnityEngine;

public class Test : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 Torque;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = float.PositiveInfinity;
    }

    private void FixedUpdate()
    {
        rb.AddRelativeTorque(Torque);
    }
}
