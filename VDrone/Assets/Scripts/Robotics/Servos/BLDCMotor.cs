using UnityEngine;
using Util;

namespace Robotics.Servos
{
    public class BLDCMotor : Servo
    {
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
                float speed = MathUtil.MapClamped(readMicroseconds(), from: (MIN_PULSE_WIDTH, MAX_PULSE_WIDTH), to: (0f, _maxSpeed));

                // rotate
                _rotorTransform.Rotate(Vector3.up * (Direction * speed * Time.deltaTime));
            }
        }

        private void FixedUpdate()
        {
            // Apply torque to rotor if specified
            if (_rotorRigidbody != null)
            {

                // Apply reaction torque to stator if specified
                if (_statorRigidbody != null)
                {

                }
            }
        }
    }
}
