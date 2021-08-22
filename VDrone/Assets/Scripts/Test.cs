using UnityEngine;

public class Test : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 Force;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Force);
    }
}
