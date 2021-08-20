using UnityEngine;

namespace Robotics.Servos
{
    public class BLDCMotor : Servo
    {
        [SerializeField]
        private bool _isClockwise;
        [SerializeField]
        private Transform _rotor;
        [SerializeField]
        private Rigidbody _rigidbody;

        // rotation direction
        private int _direction;

        protected override void Reset()
        {
            base.Reset();
            OnValidate();

            _rigidbody = GetComponentInParent<Rigidbody>()
                ?? gameObject.AddComponent<Rigidbody>();

        }

        private void OnValidate()
        {
            _direction = _isClockwise ? 1 : -1;
        }

        protected override void Update()
        {
        }

        protected override void FixedUpdate()
        {
        }
    }
}
