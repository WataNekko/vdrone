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

        public float TargetSpeed;
        public float ActualSpeed;

        [Header("BLDC Motor's Spec")]
        [SerializeField]
        [Tooltip("Maximum angular speed (rad/s) at full throttle.")]
        [Min(0f)]
        private float _maxSpeed = 125.664f;
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

        /// <summary>
        /// Sets max angular velocity to Infinity on awake.
        /// </summary>
        protected virtual void Awake()
        {
            if (_rotorRigidbody != null)
            {
                _rotorRigidbody.maxAngularVelocity = Mathf.Infinity;
            }
        }

        /// <summary>
        /// Rotates transform manually if rigidbody is not specified.
        /// </summary>
        protected virtual void Update()
        {
            // Rotate transform if rigidbody is unspecified but transform is provided.
            if (_rotorRigidbody == null && _rotorTransform != null)
            {
                // map pulse width to angular speed
                float speed = MathUtil.MapClamped(readMicroseconds(), from: (MIN_PULSE_WIDTH, MAX_PULSE_WIDTH), to: (0f, _maxSpeed));
                speed *= Mathf.Rad2Deg;

                // rotate
                _rotorTransform.Rotate(Vector3.up * (Direction * speed * Time.deltaTime));
            }
        }

        /// <summary>
        /// Applies torque and force according to throttle value.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (_rotorRigidbody == null && _forceRigidbody == null)
            {
                return;
            }

            // Get the throttle value (from 0 to 1) for later calculations
            float throttle = Mathf.InverseLerp(MIN_PULSE_WIDTH, MAX_PULSE_WIDTH, readMicroseconds());

            // Apply torque on rigidbodies if specified
            if (_rotorRigidbody != null)
            {
                ApplyTorque(throttle);
            }

            // Apply force on rigidbody if specified
            if (_forceRigidbody != null)
            {
                ApplyForce(throttle);
            }
        }

        /// <summary>
        /// Applies torque according to throttle value.
        /// </summary>
        private void ApplyTorque(float throttle)
        {
            // Implements a PID controller to rotate the rotor at a desired angular speed
            const float P_GAIN = 0.055f;

            float targetSpeed = Direction * Mathf.Lerp(0f, _maxSpeed, throttle);
            float actualSpeed = _rotorRigidbody.transform.InverseTransformDirection(_rotorRigidbody.angularVelocity).y;

            float error = targetSpeed - actualSpeed;
            float torque = error * P_GAIN;

            TargetSpeed = targetSpeed;
            ActualSpeed = actualSpeed;

            _rotorRigidbody.AddRelativeTorque(0f, torque, 0f);

            // Apply reaction torque on stator if specified
            if (_statorRigidbody != null)
            {
                _statorRigidbody.AddRelativeTorque(0f, -torque, 0f);
            }
        }

        /// <summary>
        /// Applies force corresponding to throttle value.
        /// </summary>
        private void ApplyForce(float throttle)
        {
            float force = Mathf.Lerp(0f, _maxForceProduced, throttle);
            _forceRigidbody.AddRelativeForce(0f, force, 0f);
        }
    }
}
