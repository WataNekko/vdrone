using UnityEngine;

namespace Robotics.Servos
{
    public class BLDCMotor : Servo
    {
        // Motor's built-in throttle range
        private const int MIN_THROTTLE = 1000;
        private const int MAX_THROTTLE = 2000;

        private enum RotationDirection : sbyte
        {
            Clockwise = 1,
            [InspectorName("Counter-clockwise")]
            CounterClockwise = -1
        }

        [Header("BLDC Motor's Spec")]
        [SerializeField]
        [Tooltip("Maximum angular speed (degrees/second) at full throttle.")]
        [Min(0f)]
        private float _maxSpeed = 7200f; // 0.0698f impulse torque on 0.074 kg rb ~ 7200f deg/s
        [SerializeField]
        [Tooltip("Whether the rotation direction is clockwise or counter-clockwise.")]
        private RotationDirection _direction = RotationDirection.Clockwise;

        [Header("Torque")]
        [SerializeField]
        [Tooltip("Rigidbody of the rotor to which to apply torque.\n\nIf unspecified, rotation is applied to the Rotor Transform if provided.")]
        private Rigidbody _rotorRigidbody;
        [SerializeField]
        [Tooltip("Transform of the rotor to which to apply rotation if Rotor Rigidbody is unspecified.")]
        private Transform _rotorTransform;
        [SerializeField]
        [Tooltip("Rigidbody of the stator to which to apply reaction torque.")]
        private Rigidbody _statorRigidbody;

        [Header("Force")]
        [SerializeField]
        private float _maxForceProduced = 0f;

        private float Direction => (float)_direction;

        private void Update()
        {
            // Rotate transform if rigidbody is unspecified but transform is provided.
            if (_rotorRigidbody == null && _rotorTransform != null)
            {
                // map pulse width to angular speed
                float speed = Mathf.InverseLerp(MIN_THROTTLE, MAX_THROTTLE, readMicroseconds());
                speed = Mathf.Lerp(0f, _maxSpeed, speed);

                // rotate
                _rotorTransform.Rotate(Vector3.up * (Direction * speed * Time.deltaTime));
            }
        }

        private void FixedUpdate()
        {
            if (_rotorRigidbody != null)
            {

            }

            if (_statorRigidbody != null)
            {

            }
        }
    }
}
