using UnityEngine;

public class Test : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 Force;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = Mathf.Infinity;
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(Force);
    }
}
