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
        [Tooltip("Maximum angular speed (rad/s) at full throttle.")]
        [Min(0f)]
        private float _maxSpeed = 125.664f; // 0.0698f impulse torque on 0.074 kg rb ~ 7200f deg/s = 125.664f rad/s
        [SerializeField]
        [Tooltip("Whether the rotation direction is clockwise or counter-clockwise.")]
        private RotationDirection _direction = RotationDirection.Clockwise;

        [Header("Torque")]
        [SerializeField]
        [Tooltip("Transform of the rotor on which to apply rotation if Rotor Rigidbody is unspecified.")]
        private Transform _rotorTransform;
        [SerializeField]
        [Tooltip("Rigidbody of the rotor on which to apply torque. If unspecified, rotation is applied to the Rotor Transform if transform is provided.")]
        private Rigidbody _rotorRigidbody;
        [SerializeField]
        [Tooltip("Rigidbody of the stator on which to apply reaction torque when Rotor Rigidbody is specified.")]
        private Rigidbody _statorRigidbody;

        [Header("Force")]
        [SerializeField]
        [Tooltip("Maximum vertical force produced by the motor at full throttle, to be applied on Force Rigidbody. This value may be positive or negative to specify the force's direction.")]
        private float _maxForceProduced = 0f;
        [SerializeField]
        [Tooltip("Rigidbody on which to apply the vertical force produced by this motor.")]
        private Rigidbody _forceRigidbody;

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
            // Apply torque on rotor if specified
            if (_rotorRigidbody != null)
            {

                // Apply reaction torque on stator if specified
                if (_statorRigidbody != null)
                {

                }
            }

            // Apply force on rigidbody if specified
            if (_forceRigidbody != null)
            {
                float force = MathUtil.MapClamped(readMicroseconds(), from: (MIN_PULSE_WIDTH, MAX_PULSE_WIDTH), to: (0f, _maxForceProduced));
                _forceRigidbody.AddRelativeForce(Vector3.up * force);
            }
        }
    }
}
