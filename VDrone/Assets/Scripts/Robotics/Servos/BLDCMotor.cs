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

        [Header("Motor's Spec")]
        [SerializeField]
        [Tooltip("Maximum angular speed (deg/s) at full throttle.")]
        [Min(0f)]
        private float _maxSpeed = 120_000f;
        [SerializeField]
        [Tooltip("Whether the rotation direction is clockwise or counter-clockwise.")]
        private RotationDirection _direction = RotationDirection.Clockwise;

        [Header("Torque & Force")]
        [SerializeField]
        [Tooltip("Transform of the rotor on which to apply rotation.")]
        private Transform _rotorTransform;
        [SerializeField]
        [Tooltip("The amount of reaction torque per 1 deg/s angular speed of the rotor to apply on the rigidbody.")]
        [Min(0f)]
        private float _torqueAmount = 1e-6f;
        [SerializeField]
        [Tooltip("The vertical force produced by the motor per 1 deg/s angular speed, to be applied on the rigidbody. This value may be positive or negative to specify the force's direction.")]
        private float _forceAmount = 0f;
        [SerializeField]
        [Tooltip("Rigidbody on which to apply the reaction torque and vertical force produced by this motor.")]
        private Rigidbody _rigidbody;

        private 

        /// <summary>
        /// Updates motor's physics.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            // Applies rotation on rotor
            if (_rotorTransform != null)
            {
                float dir = (float)_direction;
                float speed = MathUtil.MapClamped(readMicroseconds(), from: (MIN_PULSE_WIDTH, MAX_PULSE_WIDTH), to: (0f, _maxSpeed));

                _rotorTransform.Rotate(Vector3.up * (dir * speed * Time.deltaTime));

                // Applies reaction torque and force on rigidbody
                if (_rigidbody != null)
                {
                    Vector3 rotorUp = _rotorTransform.up;

                    _rigidbody.AddTorque(rotorUp * (-dir * speed * _torqueAmount));
                    _rigidbody.AddForceAtPosition(rotorUp * (speed * _forceAmount), _rotorTransform.position);
                }
            }
        }
    }
}
