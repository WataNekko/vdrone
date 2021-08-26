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
        private float _maxSpeed = 125.664f;
        [SerializeField]
        [Tooltip("Whether the rotation direction is clockwise or counter-clockwise.")]
        private RotationDirection _direction = RotationDirection.Clockwise;

        [Header("Torque")]
        [SerializeField]
        [Tooltip("Rigidbody of the rotor on which to apply torque.")]
        private Rigidbody _rotorRigidbody;
        [SerializeField]
        [Tooltip("Rigidbody of the stator on which to apply reaction torque when Rotor Rigidbody is specified.")]
        private Rigidbody _statorRigidbody;

        [Header("Force")]
        [SerializeField]
        [Tooltip("The vertical force produced by the motor per 1 rad/s angular speed, to be applied on Force Rigidbody.\nThe actual force applied is proportional to the rotor's current speed. This value may be positive or negative to specify the force's direction.")]
        private float _forcePerSpeedUnit = 0f;
        [SerializeField]
        [Tooltip("Rigidbody on which to apply the vertical force produced by this motor.\nThe force is only applied if both Rotor Rigidbody and Force Rigidbody are specified (which can be the same rigidbody).")]
        private Rigidbody _forceRigidbody;

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
        /// Updates motor's physics.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            // Apply torque and force on rigidbodies if specified
            if (_rotorRigidbody != null)
            {
                ApplyTorqueAndForce();
            }
        }

        // field to store torque's integrated value
        private float torqueIntegral = 0f;

        /// <summary>
        /// Applies torque according to the throttle value and force corresponding to the rotor's actual angular speed.
        /// </summary>
        private void ApplyTorqueAndForce()
        {
            // Implements a PI controller to rotate the rotor at a desired angular speed.
            // 
            // WARNING: These values are designed specifically for a default rotor's rigidbody of mass 0.074 and angular drag 0.98.
            // Other specs may not work as smoothly and correctly. And masses below 0.036 with angular drag 0.98 WILL BREAK!
            const float P_GAIN = 0.0266f;
            const float I_GAIN = 0.0005318f;

            float dir = (float)_direction;

            float targetSpeed = dir * MathUtil.MapClamped(readMicroseconds(), from: (MIN_PULSE_WIDTH, MAX_PULSE_WIDTH), to: (0f, _maxSpeed));
            float actualSpeed = _rotorRigidbody.transform.InverseTransformDirection(_rotorRigidbody.angularVelocity).y;

            float error = targetSpeed - actualSpeed;


            torqueIntegral += I_GAIN * error;
            float torque = (P_GAIN * error) + (torqueIntegral);

            // Rounds torque to 7 decimal places to prevent values too small from updating the rigidbody.
            torque = Mathf.Round(torque * 1e7f) * 1e-7f;


            // Apply torque on rotor
            _rotorRigidbody.AddRelativeTorque(0f, torque, 0f);

            // Apply reaction torque on stator if specified
            if (_statorRigidbody != null)
            {
                _statorRigidbody.AddRelativeTorque(0f, -torque, 0f);
            }

            // Apply force on rigidbody if specified
            if (_forceRigidbody != null)
            {
                _forceRigidbody.AddRelativeForce(0f, dir * actualSpeed * _forcePerSpeedUnit, 0f);
            }
        }
    }
}
