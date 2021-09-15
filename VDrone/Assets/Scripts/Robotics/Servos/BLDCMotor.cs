using UnityEngine;
using Util;
using GD.MinMaxSlider;
using System.Collections;

namespace Robotics.Servos
{
    /// <summary>
    /// Represents a BLDC motor.
    /// </summary>
    /// <remarks>
    /// The motor starts up on enable. On start up, the pulse width must be at the minimum value (1000) before the motor enters ready mode and can then spin according to the pulse width.
    /// </remarks>
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
        [SerializeField, Tooltip("The audio clip to play when the motor starts up.")]
        private AudioClip _startUpSound;
        [SerializeField, Tooltip("The audio clip to play when the motor is ready to spin.")]
        private AudioClip _readySound;
        [SerializeField, Tooltip("The audio clip to play when the motor rotates.")]
        private AudioClip _rotateSound;
        [SerializeField, MinMaxSlider(-3, 3)]
        private Vector2 _rotatePitchRange = Vector2.one;

        private AudioSource _audio;
        private bool _isReady = false;
        private Coroutine _startUpRoutine;

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
        /// Initializes audio source, plays start up sounds and sets ready flag to true on enable.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (_rotorTransform != null)
            {
                if (_audio == null)
                {
                    // Places audio source on the rotor.
                    _audio = _rotorTransform.gameObject.AddComponent<AudioSource>();
                    _audio.playOnAwake = false;
                    _audio.spatialBlend = 1f;
                }

                // Plays sounds and sets flag
                _startUpRoutine = StartCoroutine(MotorStartUp());
            }

            IEnumerator MotorStartUp()
            {
                _audio.loop = false;
                _audio.volume = _audio.pitch = 1f;

                _audio.clip = _startUpSound;
                _audio.Play();

                yield return new WaitForSeconds(GetClipLength(_startUpSound) + 1f);
                yield return new WaitUntil(() => readMicroseconds() <= MIN_PULSE_WIDTH);

                _audio.clip = _readySound;
                _audio.Play();

                yield return new WaitForSeconds(GetClipLength(_readySound) + 0.5f);

                _audio.clip = _rotateSound;
                _audio.loop = _isReady = true;

                _startUpRoutine = null;

                float GetClipLength(AudioClip clip) => clip != null ? clip.length : 0f;
            }
        }

        /// <summary>
        /// Sets ready flag to false and stops sounds on disable.
        /// </summary>
        protected virtual void OnDisable()
        {
            if (_startUpRoutine != null)
            {
                StopCoroutine(_startUpRoutine);
            }
            if (_audio != null)
            {
                _audio.Stop();
            }
            _isReady = false;
        }

        /// <summary>
        /// Updates motor's rotation and physics.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (_rotorTransform == null || !_isReady)
            {
                return;
            }

            #region Calculate angular speed
            float throttle = Mathf.InverseLerp(MIN_PULSE_WIDTH, MAX_PULSE_WIDTH, readMicroseconds());
            float speed = Mathf.Lerp(0f, _maxSpeed, throttle);
            float currVel = (float)_direction * speed;

            float error = currVel - _prevVel;
            _prevVel = currVel;
            #endregion

            // Rotates rotor
            _rotorTransform.Rotate(Vector3.up * (currVel * Time.deltaTime));

            if (_rigidbody != null)
            {
                ApplyReactionTorqueAndForce();
            }

            if (_audio != null && _audio.enabled)
            {
                UpdateMotorSound();
            }

            #region Motor's update functions
            void ApplyReactionTorqueAndForce()
            {
                Vector3 rotorUp = _rotorTransform.up;
                float acceleration = error * 100f;
                float reactionTorque = -(currVel + acceleration) * _torqueAmount;

                _rigidbody.AddTorque(rotorUp * reactionTorque);
                _rigidbody.AddForceAtPosition(rotorUp * (speed * _forceAmount), _rotorTransform.position);
            }

            void UpdateMotorSound()
            {
                _audio.volume = Mathf.Lerp(0f, 1f, throttle);
                _audio.pitch = Mathf.Lerp(_rotatePitchRange.x, _rotatePitchRange.y, throttle);

                if (throttle != 0 && !_audio.isPlaying)
                {
                    _audio.Play();
                }
                else if (throttle == 0 && _audio.isPlaying)
                {
                    _audio.Stop();
                }
            }
            #endregion
        }
    }
}
