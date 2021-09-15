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
        // Default pulse width range
        protected const int MIN_PULSE_WIDTH = 1000;
        protected const int MAX_PULSE_WIDTH = 2000;

        [SerializeField]
        [Tooltip("Current pulse width, in microseconds, of this servo.")]
        private RangedInt _pulseWidth = new RangedInt(value: 1500, range: (MIN_PULSE_WIDTH, MAX_PULSE_WIDTH));

        /// <summary>
        /// Attaches the given Servo to the out variable and sets min and max pulse width values for writes.
        /// </summary>
        /// 
        /// <remarks>
        /// This method is equivalent to:
        /// <code>var = servo;</code>
        /// and then setting the pulse width range, which is unintuitive but its purpose is to simulate working with the Arduino's Servo library.
        /// </remarks>
        /// 
        /// <param name="var">The variable to attach the Servo to.</param>
        /// <param name="servo">The Servo object to attach to the out variable.</param>
        /// <param name="min">Min pulse width.</param>
        /// <param name="max">Max pulse width.</param>
        public static void attach(out Servo var, Servo servo, int min, int max)
        {
            if (servo == null)
            {
                throw new System.ArgumentNullException(nameof(servo), "Servo to be attached must not be null.");
            }

            var = servo;
            servo._pulseWidth.Range = (min, max);
        }

        /// <summary>
        /// Attaches the given Servo to the out variable and sets pulse width range to default (1000, 2000).
        /// </summary>
        /// 
        /// <remarks>
        /// <inheritdoc cref="attach(out Servo, Servo, int, int)"/>
        /// </remarks>
        /// 
        /// <param name="var">The variable to attach the Servo to.</param>
        /// <param name="servo">The Servo object to attach to the out variable.</param>
        public static void attach(out Servo var, Servo servo) => attach(out var, servo, MIN_PULSE_WIDTH, MAX_PULSE_WIDTH);

        /// <summary>
        /// Detaches the Servo instance from the ref variable.
        /// </summary>
        /// 
        /// <remarks>
        /// This method is equivalent to:
        /// <code>var = null;</code>
        /// which is unintuitive but its purpose is to simulate working with the Arduino's Servo library.
        /// </remarks>
        /// 
        /// <param name="var">The variable to detach the Servo from.</param>
        public static void detach(ref Servo var) => var = null;

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
    }
}
