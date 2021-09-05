using UnityEngine;

public class Test : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 Force;
    public Vector3 Torque;
    public bool ApplyImpulse = false;
    public Vector3 ForceImpulse;
    public Vector3 TorqueImpulse;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = Mathf.Infinity;
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(Force);
        rb.AddRelativeTorque(Torque);
        if (ApplyImpulse)
        {
            rb.AddRelativeForce(ForceImpulse, ForceMode.Impulse);
            rb.AddRelativeTorque(TorqueImpulse, ForceMode.Impulse);
            ApplyImpulse = false;
        }
        if (joint != null)
        {
            //AngVel = Tf.InverseTransformDirection(rb.angularVelocity);
            AngVel = joint.currentTorque;
            //Ang = AngVel.magnitude;
            Ang = rb.solverIterations;
        }
    }

    public ConfigurableJoint joint;
    public Vector3 AngVel;
    public float Ang;
    public Transform Tf;
}
