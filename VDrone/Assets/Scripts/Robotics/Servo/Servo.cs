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
        /// Sets min and max pulse width values for writes.
        /// </summary>
        /// <param name="min">Min pulse width</param>
        /// <param name="max">Max pulse width</param>
        public void attach(int min, int max)
        {
            if (min > max)
            {
                throw new System.ArgumentException("Min is larger than max value.");
            }

            (_minPulseWidth, _maxPulseWidth) = (min, max);
        }

        /// <summary>
        /// Writes pulse width in microseconds.
        /// </summary>
        /// <param name="value">Pulse width</param>
        public void writeMicroseconds(int value) => _pulseWidth = Mathf.Clamp(value, _minPulseWidth, _maxPulseWidth);

        /// <summary>
        /// Returns current pulse width in microseconds for this Servo.
        /// </summary>
        /// <returns>Current pulse width in microseconds.</returns>
        public int readMicroseconds() => _pulseWidth;

        protected abstract void Update();
    }
}
