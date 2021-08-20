using UnityEngine;

namespace Robotics
{
    /// <summary>
    /// Base class for all servo types.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Servo : MonoBehaviour
    {
        private int _minPulseWidth;
        private int _maxPulseWidth;
        private int _pulseWidth;

        // Default values
        protected virtual void Reset() => (_minPulseWidth, _pulseWidth, _maxPulseWidth) = (1000, 1500, 2000);

        /// <summary>
        /// Gets min and max pulse width values.
        /// </summary>
        public (int min, int max) PulseWidthRange => (_minPulseWidth, _maxPulseWidth);

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

        /// <summary>
        /// Updates servo's visual every frame.
        /// </summary>
        protected abstract void Update();

        /// <summary>
        /// Updates servo's physics every physics frame.
        /// </summary>
        protected abstract void FixedUpdate();
    }
}
