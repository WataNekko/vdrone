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
            // Apply torque and force on rigidbody if specified
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
            // Caches rotor's transform for later possible uses
            Transform rotorTf = _rotorRigidbody.transform;

            #region Calculate torque

            // Implements a PI controller to rotate the rotor at a desired angular speed.
            // 
            // WARNING: These values are designed specifically for a default rotor's rigidbody of mass 0.074 and angular drag 0.5.
            // Other specs may NOT WORK as smoothly and correctly!
            const float P_GAIN = 0.02776236f;
            const float I_GAIN = 0.0002804293f;

            float dir = (float)_direction;

            // maps pulse width to speed
            float targetSpeed = dir * MathUtil.MapClamped(readMicroseconds(), from: (MIN_PULSE_WIDTH, MAX_PULSE_WIDTH), to: (0f, _maxSpeed));
            // actual speed is relative to stator if stator is available, otherwise relative to rotor.
            float actualSpeed = ((_statorRigidbody != null)
                ? _statorRigidbody.transform
                : rotorTf).InverseTransformDirection(_rotorRigidbody.angularVelocity).y;
            // adjusts rotor solver iteration to increase physics accuracy for higher speeds
            _rotorRigidbody.solverIterations = MathUtil.Map((int)actualSpeed, from: (0, (int)(dir * _maxSpeed)), to: (6, 30));

            float error = targetSpeed - actualSpeed;

            // This variable is for dealing with the steady-state error from the P controller.
            // The value is only integrated when the error is small.
            torqueIntegral = (error >= -0.001f && error <= 0.001f)
                ? torqueIntegral + (I_GAIN * error)
                : I_GAIN * targetSpeed;

            float torque = (P_GAIN * error) + (torqueIntegral);
            // Rounds torques that are too small to 0 to prevent updating the rigidbody.
            if (torque > -1e-7f && torque < 1e-7f)
            {
                torque = 0f;
            }

            #endregion

            // Apply torque on rotor
            _rotorRigidbody.AddRelativeTorque(0f, torque, 0f);

            // Apply reaction torque on stator if specified
            if (_statorRigidbody != null)
            {
                _statorRigidbody.AddTorque(rotorTf.TransformDirection(0f, -torque, 0f));
            }

            // Apply force on rigidbody if specified
            if (_forceRigidbody != null)
            {
                float force = dir * actualSpeed * _forcePerSpeedUnit;
                // Similar to torque, small forces are rounded to 0 to prevent updating the rigidbody.
                if (force > -1e-5f && force < 1e-5f)
                {
                    force = 0f;
                }

                _forceRigidbody.AddForce(rotorTf.TransformDirection(0f, force, 0f));
            }
        }
    }
}
