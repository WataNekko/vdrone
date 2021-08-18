using UnityEngine;

namespace Robotics.Servo
{
    [DisallowMultipleComponent]
    public abstract class Servo : MonoBehaviour
    {
        private int _minPulseWidth = 1000;
        private int _maxPulseWidth = 2000;
        [SerializeField]
        private int _pulseWidth = 1500;

        /// <summary>
        /// Writes pulse width in microseconds.
        /// </summary>
        /// <param name="value">Pulse width</param>
        public void writeMicroseconds(int value)
        {
            _pulseWidth = Mathf.Clamp(value, _minPulseWidth, _maxPulseWidth);
        }

        /// <summary>
        /// Returns current pulse width in microseconds for this Servo.
        /// </summary>
        /// <returns>Current pulse width in microseconds.</returns>
        public int readMicroseconds()
        {
            return _pulseWidth;
        }
    }
}
