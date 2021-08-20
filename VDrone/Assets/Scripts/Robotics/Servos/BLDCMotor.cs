using UnityEngine;

namespace Robotics.Servos
{
    public class BLDCMotor : Servo
    {
        public bool _isClockwise;
        private Rigidbody _rigidbody;

        protected override void Update()
        {
        }

        protected override void FixedUpdate()
        {
            //Mathf.Sin(Time.time);
        }
    }
}
