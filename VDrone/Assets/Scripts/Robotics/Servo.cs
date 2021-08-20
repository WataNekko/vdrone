using UnityEngine;
using Util;

namespace Robotics
{
    /// <summary>
    /// Base class for all servo types.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Servo : MonoBehaviour
    {
        [SerializeField]
        private IntInRange _pulseWidth;

#if UNITY_EDITOR
        // Default values
        protected virtual void Reset() => _pulseWidth = new IntInRange(1500, 1000, 2000);
#endif

        /// <summary>
        /// Sets min and max pulse width values for writes.
        /// </summary>
        /// <param name="min">Min pulse width</param>
        /// <param name="max">Max pulse width</param>
        public void attach(int min, int max) => _pulseWidth.Range = (min, max);

        /// <summary>
        /// Writes pulse width in microseconds.
        /// </summary>
        /// <param name="value">Pulse width</param>
        public void writeMicroseconds(int value) => _pulseWidth.Value = value;

        /// <summary>
        /// Returns current pulse width in microseconds for this Servo.
        /// </summary>
        /// <returns>Current pulse width in microseconds.</returns>
        public int readMicroseconds() => _pulseWidth.Value;

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
