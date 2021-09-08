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
        [SerializeField, Min(0f), Tooltip("Maximum angular speed (deg/s) at full throttle.")]
        private float _maxSpeed = 120_000f;
        [SerializeField, Tooltip("Whether the rotation direction is clockwise or counter-clockwise.")]
        private RotationDirection _direction = RotationDirection.Clockwise;

        [Header("Torque & Force")]
        [SerializeField, Tooltip("Transform of the rotor on which to apply rotation.")]
        private Transform _rotorTransform;
        [SerializeField, Min(0f), Tooltip("The amount of reaction torque per 1 deg/s angular speed of the rotor to apply on the rigidbody.")]
        private float _torqueAmount = 1e-6f;
        [SerializeField, Tooltip("The vertical force produced by the motor per 1 deg/s angular speed, to be applied on the rigidbody. This value may be positive or negative to specify the force's direction.")]
        private float _forceAmount = 0f;
        [SerializeField, Tooltip("Rigidbody on which to apply the reaction torque and vertical force produced by this motor.")]
        private Rigidbody _rigidbody;

        [Header("Sounds")]
        [SerializeField, Tooltip("The audio clip to play when the motor rotates.")]
        private AudioClip _rotateSound;
        private Vector2 _rotatePitchRange;

        private AudioSource _rotateSoundSrc;

        /// <summary>
        /// Field storing the previous angular speed with direction of the rotor.
        /// </summary>
        private float _prevVel;

        /// <summary>
        /// Logs warnings if transform or rigidbody is not assigned on awake.
        /// </summary>
        protected virtual void Awake()
        {
            if (_rotorTransform == null)
            {
                Debug.LogWarning($"{nameof(BLDCMotor)} (ID: {GetInstanceID()}): Rotor's transform is not assigned.");
            }
            if (_rigidbody == null)
            {
                Debug.LogWarning($"{nameof(BLDCMotor)} (ID: {GetInstanceID()}): Rigidbody is not assigned.");
            }
        }

        /// <summary>
        /// Updates motor's rotation and physics.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (_rotorTransform == null)
            {
                return;
            }

            #region Calculate angular speed
            float speed = MathUtil.MapClamped(readMicroseconds(), from: (MIN_PULSE_WIDTH, MAX_PULSE_WIDTH), to: (0f, _maxSpeed));
            float currVel = (float)_direction * speed;

            float error = currVel - _prevVel;
            _prevVel = currVel;
            #endregion

            // Rotates rotor
            _rotorTransform.Rotate(Vector3.up * (currVel * Time.deltaTime));

            // Applies reaction torque and force on rigidbody
            if (_rigidbody != null)
            {
                Vector3 rotorUp = _rotorTransform.up;
                float acceleration = error * 100f;
                float reactionTorque = -(currVel + acceleration) * _torqueAmount;

                _rigidbody.AddTorque(rotorUp * reactionTorque);
                _rigidbody.AddForceAtPosition(rotorUp * (speed * _forceAmount), _rotorTransform.position);
            }

            
        }

        /// <summary>
        /// Initialize audio source on enable.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (_rotorTransform != null && _rotateSoundSrc == null)
            {
                // Audio source is on the rotor.
                _rotateSoundSrc = _rotorTransform.gameObject.AddComponent<AudioSource>();
                _rotateSoundSrc.clip = _rotateSound;
                _rotateSoundSrc.loop = true;
                _rotateSoundSrc.spatialBlend = 1f;

                _rotateSoundSrc.Play();
            }
        }
    }
}
