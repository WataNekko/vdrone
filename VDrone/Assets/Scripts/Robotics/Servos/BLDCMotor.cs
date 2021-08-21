using UnityEngine;

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

        [Header("BLDC Motor")]
        [SerializeField]
        [Tooltip("Whether the rotation direction is clockwise or counter-clockwise.")]
        private RotationDirection _direction = RotationDirection.Clockwise;

        [Header("Torque's bodies")]
        [SerializeField]
        [Tooltip("Rigidbody of the rotor to which to apply torque.\n\nIf unspecified, rotation is applied to the Rotor Transform if provided.")]
        private Rigidbody _rotorRigidbody;
        [SerializeField]
        [Tooltip("Transform of the rotor to which to apply rotation if Rotor Rigidbody is unspecified.")]
        private Transform _rotorTransform;
        [SerializeField]
        [Tooltip("Rigidbody of the stator to which to apply reaction torque.")]
        private Rigidbody _statorRigidbody;

        private float Direction => (float)_direction;

        protected override void Update()
        {
        }

        protected override void FixedUpdate()
        {
        }
    }
}
